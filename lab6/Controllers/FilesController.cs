using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.IO;
using WebApplication8.Services;

namespace WebApplication8.Controllers
{
    public class FilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MimeMappingService _mimeMappingService;

        public FilesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment hostingEnvironment,
            MimeMappingService mimeMappingService)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mimeMappingService = mimeMappingService;
        }

      
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Files.Include(f => f.Folder);
            return View(await applicationDbContext.ToListAsync());
        }

       
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = _context.Files
                .SingleOrDefault(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }
    
            
            ViewBag.Folder = file.FolderId;
            return View(file);
        }




     
        public IActionResult Create(Guid? id)
        {
            ViewBag.Id = id;
            return View(new FileViewModel());
        }



       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid id, FileViewModel model)
        {
            if (!ModelState.IsValid)
            { return RedirectToAction("Details","Folder",new { id = id}); }

  
           
            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.FilePath.ContentDisposition).FileName.Trim('"'));
            var fileExt = Path.GetExtension(fileName);

            var filee = new Models.File
            {
                Name = model.Name,
                Extension = fileExt,
                FolderId = id,
                Size = model.FilePath.Length
            };
            if (filee.Name == null) filee.Name = fileName;

            var path = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", filee.Id.ToString("N") + fileExt);
            

            using (var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
            {
                model.FilePath.CopyTo(fileStream);
            }

            _context.Files.Add(filee);
            _context.SaveChanges();
            return this.RedirectToAction("Details","Folders",new { id = id});


        }



  
        public IActionResult Edit(Guid? id)
        {
            ViewBag.Id = id;
            return View(new FileViewModel());
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, FileViewModel model )
        {

            if (!ModelState.IsValid)
            { return View(model); }

            var file = _context.Files.SingleOrDefault(e => e.Id == id);
            file.Name = model.Name;
            _context.SaveChanges();
            return RedirectToAction("Details","Files", new { id = id});
        }

        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .Include(f => f.Folder)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var file = await _context.Files.Include(e=>e.Folder).SingleOrDefaultAsync(m => m.Id == id);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Folders", new { id = file.FolderId});
        }

        private bool FileExists(Guid id)
        {
            return _context.Files.Any(e => e.Id == id);
        }

        public IActionResult Download(Guid id)
        {
            var file = _context.Files
                .SingleOrDefault(e => e.Id == id);
            if (file == null)
                return NotFound();
            var attachmentPath = Path.Combine(_hostingEnvironment.WebRootPath, "attachments", file.Id.ToString("N") + Path.GetExtension(file.Extension));
            return PhysicalFile(attachmentPath, _mimeMappingService.GetContentType(file.Extension), file.Name);
        }
    }
}
