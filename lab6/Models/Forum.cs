using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class Forum
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ForumCategoryId { get; set; }

        [Required]
        public String Name { get; set; }
        public String Description { get; set; }

        public ForumCategory ForumCategory { get; set; }
        public ICollection<ForumTopic> ForumTopics { get; set; }
    }
}
