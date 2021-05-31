using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class PostAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public String Path { get; set; }
    }
}
