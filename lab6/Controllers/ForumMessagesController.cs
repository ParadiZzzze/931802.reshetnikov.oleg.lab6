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

namespace WebApplication8.Controllers
{
    public class ForumMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForumMessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

   
        public async Task<IActionResult> Index(Guid? TopicId)
        {
            if (TopicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == TopicId);

            if (topic == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Topic = topic;
            var messages = await this._context.ForumMessages
                .Include(x=> x.ForumMessageAttachments)
                .Where(x => x.ForumTopicId == TopicId)
                .ToListAsync();

            return this.View(messages);
        }


    
        public async Task<IActionResult> Create(Guid? TopicId)
        {
            if (TopicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == TopicId);

            if (topic == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Topic = topic;
            return this.View(new ForumMessageCreateModel());
        }


     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? TopicId, ForumMessageCreateModel model)
        {
            if (TopicId == null)
            {
                return this.NotFound();
            }

            var topic = await this._context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == TopicId);

            if (topic == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var message = new ForumMessage
                {
                    Text = model.Text,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ApplicationUserId = _userManager.GetUserId(User),
                    ForumTopicId = (Guid)TopicId

                };

                this._context.Add(message);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { topicId = topic.Id });
            }

            this.ViewBag.Topic = topic;
            return this.View(model);
        }


  
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages.SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", forumMessage.ApplicationUserId);
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopics, "Id", "ApplicationUserId", forumMessage.ForumTopicId);
            return View(forumMessage);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ForumTopicId,ApplicationUserId,Created,Modified,Text")] ForumMessage forumMessage)
        {
            if (id != forumMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumMessageExists(forumMessage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", forumMessage.ApplicationUserId);
            ViewData["ForumTopicId"] = new SelectList(_context.ForumTopics, "Id", "ApplicationUserId", forumMessage.ForumTopicId);
            return View(forumMessage);
        }

  
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.ApplicationUser)
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }

            return View(forumMessage);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var forumMessage = await _context.ForumMessages.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumMessages.Remove(forumMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumMessageExists(Guid id)
        {
            return _context.ForumMessages.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Back(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var topic = await this._context.ForumTopics
               .SingleOrDefaultAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "ForumTopics" ,new { topic.ForumId });
        }

    }
}
