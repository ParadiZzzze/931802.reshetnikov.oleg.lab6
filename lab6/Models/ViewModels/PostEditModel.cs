using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models.ViewModels
{
    public class PostEditModel
    {
        public Guid CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        [Required]
        public String Text { get; set; }
    }
}
