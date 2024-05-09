﻿using System.ComponentModel.DataAnnotations;
using Memories_backend.Models.Domain.Interfaces;

namespace Memories_backend.Models.Authorization
{
    public class OwnerIdBase : IOwnerId
    {
        [Required]
        [MaxLength(40)]
        public Guid OwnerId { get; private set; }

        public void SetOwnerId(Guid protectKey)
        {
            OwnerId = protectKey;
        }
    }

}