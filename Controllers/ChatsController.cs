using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIdemo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using APIdemo.DTOs;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace APIdemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatsController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/chat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chats = await _context.Chats
                 .Include(c => c.Users)
                 .Include(c => c.Messages)
                 .Where(c => c.ApplicationUserChats.Any(c => c.IsDeleted==false&&c.UserId==userId))
                 .ToListAsync();
            if (chats.IsNullOrEmpty()) return Ok(new { status = true, message = "no chats found!!" });
            var chatsDto = new List<ChatDto>();
            foreach (var chat in chats)
            {
                ChatToChatDto(chat, out ChatDto chatDto);
                chatsDto.Add(chatDto);
            }
            return Ok(new { status = true, chats = chatsDto });
        }

        // GET: api/chat/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChat(int id)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id
                    &&c.ApplicationUserChats.Any(c => c.IsDeleted==false&&c.UserId==User.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (chat == null)
            {
                return NotFound();
            }
            ChatToChatDto(chat, out ChatDto chatDto);
            return Ok(new {status=true,chat=chatDto });
            /* return Ok(new
             {
                 id = chat.Id,
                 Messages = chat.Messages.Select(u => new MessageDto
                 { Id=u.Id, SenderId=u.SenderId, Content=u.Content, Image=u.Image, Status=u.Status })
             });
            */
        }

        // POST: api/chat
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateChat(string userId)
        {

            if (userId == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return BadRequest(new { message = "Same user", status = false });
            }
            var user = await _userManager.FindByIdAsync(userId);
            var currentUser = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new { message = "user not found", status = false });
            }
            var chat = await _context.Chats.Include(c => c.ApplicationUserChats).FirstOrDefaultAsync(c => c.Users.Contains(user)&&c.Users.Contains(currentUser));
            if (chat != null)
            {
                var applicationChat = chat.ApplicationUserChats.FirstOrDefault(a => a.UserId==currentUser.Id);
                if (applicationChat==null) return NotFound();
                if (applicationChat?.IsDeleted==true)
                {
                    applicationChat.IsDeleted = false;
                    _context.Update(applicationChat);
                    await _context.SaveChangesAsync();
                    ChatToChatDto(chat, out ChatDto _chatDto);
                    return CreatedAtAction(nameof(GetChat),new { status = true, chat = _chatDto });
                }
                return BadRequest(new { status = false, message = "chat aready exists" });
            }
            chat = new Chat();
            
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            _context.ApplicationUserChats.Add(new ApplicationUserChat { UserId=user.Id,ChatId=chat.Id});
            _context.ApplicationUserChats.Add(new ApplicationUserChat { UserId=currentUser.Id, ChatId=chat.Id });
            await _context.SaveChangesAsync();
            ChatToChatDto(chat, out ChatDto chatDto);
            return CreatedAtAction(nameof(GetChat), new {id = chat.Id},new { status = true, chat = chatDto });
        }
        [NonAction]
        public void ChatToChatDto(Chat chat, out ChatDto chatDto)
        {
            chatDto = new ChatDto
            {
                Id=chat.Id,
                Messages= chat.Messages
                      .Select(u => new MessageDto
                      {
                          SenderId = u.SenderId,
                          Image = u.Image,
                          Status = u.Status,
                          Content = u.Content,
                          Id = u.Id
                      }).ToList(),
                Users=chat.Users.Select(c => new ChatUserDto
                { Id=c.Id, FirstName=c.FirstName, LastName=c.LastName, Picture=c.ProfilePicture })
                        .ToList()
            };
        }

        /*// PUT: api/chat/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChat(int id, ChatDto chat)
        {
            if (id != chat.Id)
            {
                return BadRequest();
            }
            _context.Entry(chat).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }*/

        // DELETE: api/chat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChat(int id)
        {
            var chat = await _context.Chats.Include(c => c.ApplicationUserChats).FirstOrDefaultAsync(c => c.Id==id);
            if (chat == null)
            {
                return NotFound();
            }
            var userChat = chat.ApplicationUserChats
                .First(u => u.UserId==User.FindFirstValue(ClaimTypes.NameIdentifier)&&u.ChatId==id);
            if (userChat == null)
            {
                return NotFound();
            }

            userChat.IsDeleted=true;
            if (chat.ApplicationUserChats.Any(c => c.IsDeleted==false))
            {
                _context.Update(chat);

            }
            else
            {
                _context.Remove(chat);
            }
            await _context.SaveChangesAsync();
            return Ok(new { status = true, message = "Deleted" });
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }

        // POST: api/chat/{chatId}/message
        [HttpPost("{chatId}/message")]
        public async Task<IActionResult> SendMessage(int chatId, [FromForm] MessageDto message)
        {
            var chat = await _context.Chats.FindAsync(chatId);
            if (chat == null)
            {
                return NotFound();
            }
            var newMessage = new Message
            {
                ChatId = chatId,
                Status = MStatus.UnSent,
                SenderId=User.FindFirstValue(ClaimTypes.NameIdentifier),
                Content=message.Content,
                Image=message.Image
            };
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
            message.Id=newMessage.Id;
            message.SenderId=newMessage.SenderId;
            message.Status=MStatus.UnSent;
            return CreatedAtAction(nameof(GetChat), new { id = chatId }, message);
        }

        // PUT: api/chat/{chatId}/message/{messageId}/{status}
        [HttpPut("{chatId}/message/status")]
        public async Task<IActionResult> UpdateMessageStatus(int chatId)
        {
            var messages = _context.Messages
                .Where(m => m.ChatId == chatId&&m.Status==0);

            if (messages == null)
            {
                return NotFound();
            }
            
            foreach (var message in messages)
            {
                message.Status = MStatus.Read;
            }
            _context.UpdateRange(messages);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
