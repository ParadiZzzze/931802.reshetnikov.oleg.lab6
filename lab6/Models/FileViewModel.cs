using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication8.Models
{
    public class FileViewModel
    {
        public String Name { get; set; }
        public IFormFile FilePath { get; set; }
    }
}
