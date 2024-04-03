using AutoMapper;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using ChatHubProject.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

[ApiController]
[Route("/api/[controller]")]
public class ServerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ChatHubContext _db;
    private readonly string _uploadPath;

    public ServerController(ChatHubContext db, IMapper mapper, Microsoft.Extensions.Configuration.IConfiguration config)
    {
        _db = db;
        _mapper = mapper;
        _uploadPath = config["UploadFilePath"] ?? "upload_files";
    }

    [HttpGet("all_servers")]
    public IActionResult GetAllServers()
    {
        var servers = _db.Servers.OrderBy(s => s.Name)
            .Select(s => new
            {
                s.Name,
                s.ImageFilename,
                UserGuid = s.User.Guid,
                Guid = s.Guid,
            }).ToList();
        return Ok(servers);
    }

    [HttpGet("one_server")]
    public async Task<IActionResult> GetOneServer(Guid guid)
    {
        var server = await _db.Servers.
            Where(s => s.Guid == guid)
            .Select(s => new
            {
                s.Guid,
                s.Name,
                UserGuid = s.User.Guid,
            }).FirstOrDefaultAsync();
        if (server == null) return NotFound();
        return Ok(server);
    }

    [HttpPost("add_server")]
    public async Task<IActionResult> AddServer([FromForm] NewServerCmd serverCmd)
    {
        if (serverCmd.File is null || serverCmd.File.Length > 1 << 20) return BadRequest();

        var filename = $"{serverCmd.Name + '_' + serverCmd.File.FileName}";
        using (var filestream = serverCmd.File.OpenReadStream())
        using (var destFileStream = new FileStream(Path.Combine(_uploadPath, filename), FileMode.Create, FileAccess.Write))
        {
            await filestream.CopyToAsync(destFileStream);
        }

        var user = _db.Users.First(u => u.Guid == serverCmd.UserGuid);
        if (user is null) return BadRequest();
        var server = new Server(serverCmd.Name, user, filename);
        if (serverCmd.File is null) return BadRequest();
        if (serverCmd.File.Length > 1 << 20) return BadRequest();
        await _db.Servers.AddAsync(server);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return Ok(new { server.Guid, server.Name, serverCmd.UserGuid});
    }

    [HttpDelete("delete_server")]
    public async Task<IActionResult> DeleteServer(Guid guid)
    {
        var server = await _db.Servers.FirstOrDefaultAsync(s => s.Guid == guid);
        if (server == null) return NotFound();
        _db.Servers.Remove(server);

        var oldFilePath = Path.Combine(_uploadPath, server.ImageFilename);
        if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);

        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }

    [HttpPut("edit_server/{guid}")]
    public async Task<IActionResult> EditServer(Guid guid, [FromForm] EditServerCmd serverCmd)
    {
        if (guid != serverCmd.Guid) { return BadRequest(); }
        var server = await _db.Servers.FirstOrDefaultAsync(s => s.Guid == guid);
        if (server == null) return NotFound();

        var oldFileName = $"{server.ImageFilename}";
        var oldFilePath = Path.Combine(_uploadPath, oldFileName);
        if (System.IO.File.Exists(oldFilePath)) System.IO.File.Delete(oldFilePath);

        server.Name = serverCmd.Name;
        server.User = _db.Users.FirstOrDefault(u => u.Guid == serverCmd.UserGuid)!;
        server.ImageFilename = $"{serverCmd.Name + '_' + serverCmd.File!.FileName}"; ;

        using (var filestream = serverCmd.File.OpenReadStream())
        using (var destFileStream = new FileStream(Path.Combine(_uploadPath, server.ImageFilename), FileMode.Create, FileAccess.Write))
        {
            await filestream.CopyToAsync(destFileStream);
        }

        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
}