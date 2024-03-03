using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Controllers
{
    public class MessageController : EntityReadController<Message>
    {
        private readonly IConfiguration _config;
        private readonly bool _isDevelopment;

        public MessageController(ChatHubContext db, IMapper mapper, IConfiguration config, IHostEnvironment _env) : base(db, mapper)
        {
            _config = config;
            _isDevelopment = _env.IsDevelopment();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessage() => await GetAll<MessageDto>();

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetMessage(Guid guid)
        {
            return await GetByGuid(guid, m => new
            {
                m.Guid,
                m.Text,
                m.Time,
                Userguid = m.User.Guid,
            });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(MessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto,
                opt => opt.AfterMap((Dto, entity) =>
                {
                    entity.User = _db.Users.First(a => a.Guid == messageDto.UserGuid);
                }));
            await _db.Messages.AddAsync(message);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); } 
            return Ok(_mapper.Map<Message, MessageDto>(message));
        }

        [HttpPut("{guid:Guid}")]
        public IActionResult EditMessage(Guid guid, MessageDto messageDto)
        {
            if (guid != messageDto.Guid) { return BadRequest(); }
            var message = _db.Messages.FirstOrDefault(a => a.Guid == guid);
            if (message is null) { return NotFound(); }
            _mapper.Map(messageDto, message,
                opt => opt.AfterMap((Dto, entity) =>
                {
                    entity.User = _db.Users.First(a => a.Guid == messageDto.UserGuid);
                }));
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } 
            return NoContent();
        }

        [HttpDelete("{guid:Guid}")]
        public IActionResult DeleteMessage(Guid guid)
        {
            var message = _db.Messages.FirstOrDefault(a => a.Guid == guid);
            if (message is null) { return NotFound(); }
            _db.Messages.Remove(message);
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } 
            return NoContent();
        }
    }
}
