using System;

namespace ChatHubProject.Application.Model
{
    public interface IEntity<Tkey> where Tkey : struct
    {
        Tkey Id { get; }
        Guid Guid { get; }
    }
}