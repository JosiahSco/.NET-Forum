using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Threddit.Data;
using Threddit.Models;

namespace Threddit.Controllers
{
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost] 
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            Post post = await _context.Posts.FindAsync(comment.PostId);
            if (post == null)
            {
                return NotFound();
            }

            comment.CreatedAt = DateTime.Now;
            User user = await _userManager.GetUserAsync(User);
            comment.CreatedBy = await _userManager.GetUserNameAsync(user);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Post", new { id = comment.PostId });
        }
    }
}
