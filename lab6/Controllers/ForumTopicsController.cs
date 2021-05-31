using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using WebApplication8.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using WebApplication8.Services;

namespace WebApplication8.Controllers
{
    public class ForumTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumTopicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _userManager = userManager;
            _context = context;
            this.userPermissions = userPermissions;
        }


        public async Task<IActionResult> Index(Guid? ForumId)
        {
            if (ForumId == null)
            {
                return this.NotFound();
            }

            var Forum = await this._context.Forums
                .SingleOrDefaultAsync(x => x.Id == ForumId);

            if (Forum == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Forum = Forum;
            var topics = await this._context.ForumTopics
                .Include(w => w.Forum)
                .Include(w => w.ForumMessages)
                .Include(w => w.ApplicationUser)
                .Where(x => x.ForumId == ForumId)
                .ToListAsync();

            return this.View(topics);

        }

  
        public async Task<IActionResult> Create(Guid? ForumId)
        {
            if (ForumId == null)
            {
                return this.NotFound();
            }

            var forum = await this._context.Forums
                .SingleOrDefaultAsync(x => x.Id == ForumId);

            if (forum == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Forum = forum;
            return this.View(new ForumTopicCreateModel());
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? ForumId, ForumTopicCreateModel model)
        {
            if (ForumId == null)
            {
                return this.NotFound();
            }

            var forum = await this._context.Forums
                .SingleOrDefaultAsync(x => x.Id == ForumId);

            if (forum == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var topic = new ForumTopic
                {
                    ForumId = forum.Id,
                    Name = model.Name,
                    Created  = DateTime.Now,
                    ApplicationUserId = _userManager.GetUserId(User)
            
                };

                this._context.Add(topic);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { forumId = forum.Id });
            }

            this.ViewBag.Forum = forum;
            return this.View(model);
        }


       
        public async Task<IActionResult> Edit(Guid? TopicId)
        {
            if (TopicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(m => m.Id == TopicId);
       
            if (topic == null)
            {
                return this.NotFound();
            }

            var model = new ForumTopicCreateModel
            {
                Name = topic.Name
            };

            return this.View(model);
        }


    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? TopicId, ForumTopicCreateModel model)
        {
            if (TopicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(m => m.Id == TopicId);
            if (topic == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                topic.Name = model.Name;
                topic.Created = DateTime.Now;
                topic.ApplicationUserId = _userManager.GetUserId(User);

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { forumId = topic.ForumId });
            }

            return this.View(model);
        }


 
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.ApplicationUser)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumTopics.Remove(forumTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumTopicExists(Guid id)
        {
            return _context.ForumTopics.Any(e => e.Id == id);
        }
    }
}
