using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIdemo.Models;
using APIdemo.DTOs;
using APIdemo.Tools;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Expressions;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class BannersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BannersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Banners
        [HttpGet]
        public async Task<IActionResult> GetBanners()
        {
            if (_context.Banners == null)
            {
                return NotFound();
            }
            var banners = await _context.Banners.Select(b =>
                new BannerDto { Id=b.Id, Image=b.Image, Description=b.Description, Title=b.Title })
                .ToListAsync();
            return Ok(new
            {
                banners
                ,
                status = true
            });
        }

        // GET: api/Banners/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner(int id)
        {
            if (_context.Banners == null)
            {
                return NotFound();
            }
            var banner = await _context.Banners.Where(b => b.Id==id).Select(b =>
                new BannerDto { Id=b.Id, Image=b.Image, Description=b.Description, Title=b.Title }).FirstOrDefaultAsync();

            if (banner == null)
            {
                return NotFound();
            }

            return Ok(new { banner, status = true });
        }

        // PUT: api/Banners/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanner(int id, [FromForm] BannerDto banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (banner.File != null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await banner.File.CopyToAsync(memoryStream);
                    banner.Image = memoryStream.ToArray();
                }
                if (banner.Image != null)
                {
                    string? type = ImageTools.GetImageType(banner.Image);
                    if (type == null)
                    {
                        return BadRequest(new { status = false, message = "unsupported picture type" });
                    }
                }
            }
            var Banner = await _context.Banners.FindAsync(id);

            Banner.Title=banner.Title;
            Banner.Description=banner.Description;
            Banner.Image=banner.Image;


            _context.Entry(Banner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BannerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Banners
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBanner([FromForm] BannerDto banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (banner.File != null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await banner.File.CopyToAsync(memoryStream);
                    banner.Image = memoryStream.ToArray();
                }
                if (banner.Image != null)
                {
                    string? type = ImageTools.GetImageType(banner.Image);
                    if (type == null)
                    {
                        return BadRequest(new { status = false, message = "unsupported picture type" });
                    }
                }
            }
            var newBanner = new Banner
            {
                Title=banner.Title,
                Description=banner.Description,
                Image=banner.Image,
                UserId= User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            _context.Banners.Add(newBanner);
            await _context.SaveChangesAsync();
            banner.Id=newBanner.Id;
            return CreatedAtAction(nameof(GetBanner), new { id = newBanner.Id }, banner);
        }

        // DELETE: api/Banners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            if (_context.Banners == null)
            {
                return NotFound();
            }
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return Ok(new { status = true });
        }

        private bool BannerExists(int id)
        {
            return (_context.Banners?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
