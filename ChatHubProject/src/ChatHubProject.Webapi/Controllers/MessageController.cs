using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace ChatHubProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly bool _isDevelopment;
        private readonly IMapper _mapper;
        private readonly ChatHubContext _db;

        public MessageController(ChatHubContext db, IMapper mapper, IHostEnvironment env)
        {
            _db = db;
            _mapper = mapper;
            _isDevelopment = env.IsDevelopment();
        }

        /// <summary>
        /// GET Request /api/news/
        /// Returns a list of all messages with base information.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllMessages()
        {
            // Project your entities to a custon JSON WITHOUT INTERNAL KEYS, ...
            var message = _db.Messages.OrderBy(a => a.Time)
                .Select(a => new
                {
                    a.Guid,
                    a.Text,
                    a.User.Username,
                    a.Time,
                    UserGuid = a.User.Guid,
                    
                    
                })
                .ToList();
            return Ok(message);
        }

        

        /// <summary>
        /// POST Request /api/news with JSON body
        /// Creates a new message in the database. Validation of the dto class is performed
        /// automatically by ASP.NET Core, so you have to implement this in your dto class!
        /// </summary>
        [HttpPost("send")]
        public IActionResult SendMessage(MessageDto messageDto)
        {
            // After mapping we have to resolve the foreign key guids.
            // First() throws an exception if no data matches the predicate. So you have to check
            // the referenced data in your Validate method of your dto class!
            var message = _mapper.Map<Message>(messageDto,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.User = _db.Users.First(a => a.Guid == messageDto.UserGuid);
                    entity.Time = DateTime.UtcNow;
                }));
            _db.Messages.Add(message);
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } // DB constraint violations, ...
            return Ok(_mapper.Map<Message, MessageDto>(message));
        }

        /// <summary>
        /// PUT Request /api/news/(guid) with JSON body
        /// Updates an Message in the database. Validation of the dto class is performed
        /// automatically by ASP.NET Core, so you have to implement this in your dto class!
        /// </summary>
        [HttpPut("{guid:Guid}")]
        public IActionResult EditMessage(Guid guid, MessageDto messageDto)
        {
            if (guid != messageDto.Guid) { return BadRequest(); }
            var message = _db.Messages.FirstOrDefault(a => a.Guid == guid);
            if (message is null) { return NotFound(); }
           
            _mapper.Map(messageDto, message,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.User = _db.Users.First(a => a.Guid == messageDto.UserGuid);
                    
                }));

            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } // DB constraint violations, ...
            return NoContent();
        }

        /// <summary>
        /// DELETE Request /api/news/(guid) with JSON body
        /// Updates an Message in the database.
        /// </summary>
        [HttpDelete("{guid:Guid}")]
        public IActionResult DeleteMessage(Guid guid)
        {
            // Try to find message in the database.
            var message = _db.Messages.FirstOrDefault(a => a.Guid == guid);
            // Messasge does not exist: return 404.
            if (message is null) { return NotFound(); }
            // TODO: Remove referenced data (if needed)
            // Remove message.
            _db.Messages.Remove(message);
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } // DB constraint violations, ...
            return NoContent();
        }
    }
}
