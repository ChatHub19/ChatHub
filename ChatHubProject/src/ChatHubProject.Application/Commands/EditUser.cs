using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Commands
{
    public record EditUserCmd(

        string Username,

        string Displayname,

        string Email
    );
}
