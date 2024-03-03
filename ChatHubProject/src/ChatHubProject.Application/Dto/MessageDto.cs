using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Dto
{
    public record MessageDto(
        Guid Guid,

        string Text, 
        
        string Time,
        
        Guid UserGuid);
}
