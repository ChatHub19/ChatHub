using ChatHubProject.Application.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
// using System.IO;
using System.Linq;

namespace ChatHubProject.Webapi.Dto
{

    public record NewServerCmd(string Name, IFormFile? File, Guid UserGuid);
    public record EditServerCmd(Guid Guid, string Name, IFormFile? File, Guid UserGuid);
}

