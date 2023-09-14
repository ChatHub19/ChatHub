using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatHubProject.Application.Model
{
    public class FriendRequest : IEntity<int>
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected FriendRequest() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public FriendRequest(User senderUser, User receiverUser, DateTime createdAt, string url)
        {
            SenderUser = senderUser;
            ReceiverUser = receiverUser;
            CreatedAt = createdAt;
            Url = url;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Url { get; set; }
        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }
    }
}