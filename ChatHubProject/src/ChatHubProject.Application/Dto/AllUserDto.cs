using ChatHubProject.Application.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatHubProject.Application.Dto
{
    public record AllUserDto(Guid Guid, string Username, string Password, string Email, string Role, string? Group);
}
