using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("/api/[controller]")]
public class ServerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ChatHubContext _db;

    public ServerController(ChatHubContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet("all_servers")]
    public IActionResult GetAllServers()
    {
        var servers = _db.Servers.OrderBy(s => s.Name)
            .Select(s => new
            {
                s.Guid,
                s.Name,
                UserGuid = s.User.Guid,
                s.MaxCapacity,
                s.Description,
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
                s.MaxCapacity,
                s.Description,
            }).FirstOrDefaultAsync();
        if (server == null) return NotFound();
        return Ok(server);
    }

    [HttpPost("add_server")]
    public async Task<IActionResult> AddServer(ServerDto serverdto)
    {
        var server = _mapper.Map<Server>(serverdto,
            opt => opt.AfterMap((dto, entity) =>
            {
                entity.User = _db.Users.First(u => u.Guid == serverdto.UserGuid);
            }));
        await _db.Servers.AddAsync(server);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return Ok(_mapper.Map<Server, ServerDto>(server));
    }

    [HttpDelete("delete_server")]
    public async Task<IActionResult> DeleteServer(Guid guid)
    {
        var server = await _db.Servers.FirstOrDefaultAsync(s => s.Guid == guid);
        if(server == null) return NotFound();
        _db.Servers.Remove(server);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }

    [HttpPut("edit_server")]
    public async Task<IActionResult> EditServer(Guid guid, ServerDto serverdto)
    {
        if (guid != serverdto.Guid) { return BadRequest(); }
        var server = await _db.Servers.FirstOrDefaultAsync(s => s.Guid == guid);
        if (server == null) return NotFound();
        _mapper.Map(serverdto, server,
            opt => opt.AfterMap((dto, entity) =>
            {
                entity.User = _db.Users.First(s => s.Guid == serverdto.UserGuid);
            }));
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { return BadRequest(); }
        return NoContent();
    }
}