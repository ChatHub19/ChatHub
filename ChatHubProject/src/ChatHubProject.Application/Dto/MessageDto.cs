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
    public record MessageDto(
        Guid Guid,

        string Text,

        string Time,

        Guid UserGuid) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetRequiredService<ChatHubContext>();
            if (!db.Users.Any(a => a.Guid == UserGuid))
            {
                yield return new ValidationResult("User does not exist", new[] { nameof(UserGuid) });
            }
        }
    }

};

