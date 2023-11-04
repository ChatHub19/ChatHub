using ChatHubProject.Application.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatHubProject.Application.Dto
{
    public record UserDto(
        Guid Guid,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must contain at least 3 letters")]
        string Username,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password must contain at least 3 letters")]
        string Password,

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Email must contain at least 3 letters")]
        [EmailAddress]
        string Email,

        string Role,

        string? Group);
}