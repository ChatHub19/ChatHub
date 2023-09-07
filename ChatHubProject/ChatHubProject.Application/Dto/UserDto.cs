using ChatHubProject.Application.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatHubProject.Application.Dto
{
    public record UserDto(
        Guid Guid,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "name must contain at least 3 letters")]
        string Username,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "password must contain at least 3 letters")]
        string Password,

        [StringLength(100, MinimumLength = 3, ErrorMessage = "email must contain at least 3 letters")]
        [EmailAddress]
        string Email,

        [Range(1, 2, ErrorMessage = "range of userrole is invalid")]
        Userrole Role);
}