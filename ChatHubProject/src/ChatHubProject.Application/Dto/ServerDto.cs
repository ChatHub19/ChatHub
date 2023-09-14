using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Dto
{
    public record ServerDto(
        Guid Guid,

        [StringLength(30, MinimumLength = 2, ErrorMessage = "username must at least contain 2 letters")]
        string Name,

        Guid CreatorGuid,

        [Range(3, 50, ErrorMessage = "server must at least contain 3 members")]
        int MaxCapacity,

        [MaxLength(300, ErrorMessage = "description must not exceed 300 characters")]
        string Description
    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetRequiredService<ChatHubContext>();
            if(!db.Users.Any(u => u.Guid == CreatorGuid)){
                yield return new ValidationResult("User does not exist", new[] { nameof(CreatorGuid) });
            }
        }
    }
}
