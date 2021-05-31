using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8.Models
{
    public class File
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public String Name { get; set; }
        public String Extension { get; set; }
        public Int64 Size { get; set; }

        public Folder Folder { get; set; }
        public Guid FolderId{ get; set; }
}
}
