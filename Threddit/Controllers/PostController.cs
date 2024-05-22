using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Threddit.Data;
using Threddit.Models;

namespace Threddit.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public PostController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            // Not sure what to do about the nullable stuff
            post.CreatedAt = DateTime.Now;
            User user = await _userManager.GetUserAsync(User);
            post.CreatedBy = await _userManager.GetUserNameAsync(user);
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
