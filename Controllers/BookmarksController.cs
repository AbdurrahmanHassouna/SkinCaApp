using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using APIdemo.DTOs;
using APIdemo.Models;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookmarksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bookmarks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookmarkDto>>> GetUserBookmarks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var bookmarks = await _context.BookMarks
                .Where(b => b.UserId == userId)
                .Include(b => b.Disease)
                .Select(b => new BookmarkDto
                {
                    Id = b.Id,
                    Content = b.Disease.Image,
                    Title = b.Disease.Title
                })
                .ToListAsync();
            if (bookmarks.Count==0)
            {
                return Ok(new { message = "empty" });
            }
            return Ok(bookmarks);
        }
        // Post: api/bookmarks/{diseaseId}
        [HttpPost("{diseaseId}")]
        public async Task<IActionResult> PostBookmarks(int diseaseId)
        {
            var disease = await _context.Diseases.FindAsync(diseaseId);
            if (disease == null)
            {
                return NotFound(new { status = false, message = "disease not found" });
            }
            var newBookmark = new BookMark
            {
                DiseaseId= diseaseId,
                UserId=User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            await _context.BookMarks.AddAsync(newBookmark);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Delete: api/bookmarks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookmark(int id)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookmark = _context.BookMarks.FirstOrDefault(u => u.UserId==userId&&u.Id==id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _context.BookMarks.Remove(bookmark);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
