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
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            post.CreatedBy = await _userManager.GetUserNameAsync(user);
            post.CreatedAt = DateTime.Now;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Post post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Doing this by username alone seems insecure, even though its unique, look into this
            if (post.CreatedBy != await _userManager.GetUserNameAsync(user))
            {
                return Unauthorized();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            Post post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Post post)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Post oldPost = await _context.Posts.FindAsync(post.Id);
            if (oldPost == null)
            {
                return NotFound();
            }

            if (oldPost.CreatedBy != await _userManager.GetUserNameAsync(user))
            {
                return Unauthorized();
            }

            oldPost.Title = post.Title;
            oldPost.Body = post.Body;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = post.Id });
        }   

        [HttpGet]
        public IActionResult Details(int id)
        {
            Post post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Dont really like this, maybe pass a tuple of <Post, List<Comment>> to the view?
            List<Comment> comments = _context.Comments.Where(c => c.PostId == id).ToList();
            ViewData["Comments"] = comments;

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int id)
        {
            Post post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Likes++;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> DislikePost(int id)
        {
            Post post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            post.Dislikes++;
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id });
        }
    }
}
