using System.ComponentModel.DataAnnotations;

namespace ChatHubProject.Application.Commands
{
    public record EditUserCmd(

        string Displayname,

        [EmailAddress(ErrorMessage = "Invalid Email")]
        string Email
    );
}
