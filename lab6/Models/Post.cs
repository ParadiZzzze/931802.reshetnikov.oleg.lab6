using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class Post
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public String CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public Guid CategoryId { get; set; }

        public PostCategory Category { get; set; }

        public ICollection<PostAttachment> Attachments { get; set; }

        public ICollection<PostComment> Comments { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        [Required]
        public String Text { get; set; }
    }
}
