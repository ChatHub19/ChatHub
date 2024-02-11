using ChatHubProject.Application.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// using System.IO;
using System.Linq;

namespace ChatHubProject.Application.Dto
{
    public record ServerDto(
        Guid Guid,

        string Name,

        // File Icon,

        Guid UserGuid

    ) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetRequiredService<ChatHubContext>();
            if (!db.Users.Any(u => u.Guid == UserGuid))
            {
                yield return new ValidationResult("User does not exist", new[] { nameof(UserGuid) });
            }
        }
    }
}
