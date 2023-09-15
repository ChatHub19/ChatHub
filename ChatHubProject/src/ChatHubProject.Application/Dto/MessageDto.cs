using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChatHubProject.Application.Dto
{
    public record MessageDto(
         Guid Guid,
        string Text,


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

 
}

