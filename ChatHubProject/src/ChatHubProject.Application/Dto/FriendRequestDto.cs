using ChatHubProject.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Dto
{
    public record FriendRequestDto
    (
        Guid Guid,
        string Url,

        Guid SenderUserGuid,
        Guid ReceiverUserGuid ): IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
            var db = validationContext.GetRequiredService<ChatHubContext>();
            if (!db.Users.Any(a => a.Guid == SenderUserGuid))
            {
                yield return new ValidationResult("Sender does not exist", new[] { nameof(SenderUserGuid) });
            }
            if (!db.Users.Any(c => c.Guid == ReceiverUserGuid))
            {
                yield return new ValidationResult("Receiver does not exist", new[] { nameof(ReceiverUserGuid) });
            }
        }
    }
}
