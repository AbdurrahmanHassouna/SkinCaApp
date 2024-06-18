using APIdemo.DTOs;
using APIdemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIdemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemindersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RemindersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReminders()
        {
            var userId = GetUserid();
            var reminders = _context.Reminders
                .Where(r => r.UserId == userId);

            return Ok(reminders
                .Select(r => new
                {
                    r.Id,
                    Alarm = r.Alarm,
                    Amount = r.Amount,
                    BillType = r.BillType,
                    ExpiresAt = r.ExpiresAt,
                    Name = r.Name
                }));
        }
        // GET: api/Reminders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReminder(int id)
        {
            if (_context.Reminders == null)
            {
                return NotFound();
            }
            var reminder = await _context.Reminders.Where(b => b.Id==id).Select(r =>
                new 
                {
                    r.Id,
                    Alarm = r.Alarm,
                    Amount = r.Amount,
                    BillType = r.BillType,
                    ExpiresAt = r.ExpiresAt,
                    Name = r.Name
                }).FirstOrDefaultAsync();

            if (reminder == null)
            {
                return NotFound();
            }

            return Ok(new { reminder, status = true });
        }
        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] ReminderDto newReminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Reminder reminder = new Reminder();
            var userId = GetUserid();
            reminder.UserId = userId;
            reminder.Alarm=newReminder.Alarm;
            reminder.Amount=newReminder.Amount;
            reminder.Name=newReminder.Name;
            reminder.BillType=newReminder.BillType;
            reminder.ExpiresAt=newReminder.ExpiresAt;
            _context.Reminders.Add(reminder);
            if (reminder.IsExpired)
            {
                return BadRequest(new { status = false, message = "expired date" });
            }
            await _context.SaveChangesAsync();
            newReminder.Id=reminder.Id;
            return Ok(newReminder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReminder(int id,[FromBody] ReminderDto reminderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var existingReminder = await _context.Reminders.FindAsync(reminderDto.Id);
            if (existingReminder == null)
            {
                return NotFound();
            }
            
            existingReminder.Name = reminderDto.Name;
            existingReminder.Amount = reminderDto.Amount;
            existingReminder.ExpiresAt = reminderDto.ExpiresAt;
            existingReminder.Alarm = reminderDto.Alarm;
            existingReminder.BillType = reminderDto.BillType;
            if (existingReminder.IsExpired)
            {
                return BadRequest(new { status = false, message = "expired date" });
            }
            await _context.SaveChangesAsync();

            return Ok(reminderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReminder(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string GetUserid()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
