using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ChatHubProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RequestsController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ChatHubContext _db;

        public RequestsController(ChatHubContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// GET Request /api/news/
        /// Returns a list of all articles with base information.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllRequests()
        {
            // Project your entities to a custon JSON WITHOUT INTERNAL KEYS, ...
            var requests = _db.FriendRequests.OrderBy(a => a.CreatedAt)
                .Select(a => new
                {
                    a.Id,
                    a.Guid,
                    a.SenderUser,
                    a.ReceiverUser,
                    a.CreatedAt,
                    a.Url,
                    SenderUserGuid = a.SenderUser.Guid,
                    SenderUserName = a.SenderUser.Username,
                    ReceiverUserGuid = a.ReceiverUser.Guid,
                    ReceiverUserName = a.ReceiverUser.Username
                })
                .ToList();
            return Ok(requests);
        }

        /// <summary>
        /// GET Request /api/news/{id}
        /// </summary>
        [HttpGet("{guid:Guid}")]
        public IActionResult GetRequestsDetail(Guid guid)
        {
            // Project your entities to a custon JSON WITHOUT INTERNAL KEYS, ...
            var request = _db.FriendRequests
                 .Where(a => a.Guid == guid)
                 .Select(a => new
                 {
                     a.Id,
                     a.Guid,
                     a.SenderUser,
                     a.ReceiverUser,
                     a.CreatedAt,
                     a.Url,
                     SenderUserGuid = a.SenderUser.Guid,
                     SenderUserName = a.SenderUser.Username,
                     ReceiverUserGuid = a.ReceiverUser.Guid,
                     ReceiverUserName = a.ReceiverUser.Username
                 })
                 .FirstOrDefault(a => a.Guid == guid);
            if (request is null) { return NotFound(); }
            return Ok(request);
        }

        /// <summary>
        /// POST Request /api/news with JSON body
        /// Creates a new article in the database. Validation of the dto class is performed
        /// automatically by ASP.NET Core, so you have to implement this in your dto class!
        /// </summary>
        [HttpPost("addrequest")]
        public IActionResult AddRequest(FriendRequestDto friendRequestDto)
        {
            // After mapping we have to resolve the foreign key guids.
            // First() throws an exception if no data matches the predicate. So you have to check
            // the referenced data in your Validate method of your dto class!
            var request = _mapper.Map<FriendRequest>(friendRequestDto,
                opt => opt.AfterMap((dto, entity) =>
                {
                    entity.SenderUser = _db.Users.First(a => a.Guid == friendRequestDto.SenderUserGuid);
                    entity.ReceiverUser = _db.Users.First(c => c.Guid == friendRequestDto.ReceiverUserGuid);
                    entity.CreatedAt = DateTime.UtcNow;
                }));
            _db.FriendRequests.Add(request);
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } // DB constraint violations, ...
            return Ok(_mapper.Map<FriendRequest, FriendRequestDto>(request));
        }

        /// <summary>
        /// DELETE Request /api/news/(guid) with JSON body
        /// Updates an article in the database.
        /// </summary>
        [HttpDelete("{guid:Guid}")]
        public IActionResult DeleteRequest(Guid guid)
        {
            // Try to find article in the database.
            var request = _db.FriendRequests.FirstOrDefault(a => a.Guid == guid);
            // Article does not exist: return 404.
            if (request is null) { return NotFound(); }
            // TODO: Remove referenced data (if needed)
            // Remove article.
            _db.FriendRequests.Remove(request);
            try { _db.SaveChanges(); }
            catch (DbUpdateException) { return BadRequest(); } // DB constraint violations, ...
            return NoContent();
        }
    }
}