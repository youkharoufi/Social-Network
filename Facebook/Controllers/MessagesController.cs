using AutoMapper;
using Facebook.ClaimsUsers;
using Facebook.Data;
using Facebook.Interface;
using Facebook.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IMessageRepository _messageRepository;
        private IMapper _mapper;   
        public MessagesController(UserManager<User> userManager, DataContext context, IMessageRepository messageRepository, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _messageRepository = messageRepository;
            _mapper = mapper;

        }

        [HttpPost("createMessage")]
        public async Task<ActionResult<Message>> CreateMessage([FromBody]MessageSent message)
        {

            var meUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == message.SenderUsername);

            var targetUser = await _userManager.Users.FirstOrDefaultAsync(r => r.Id == message.targetId);

            var newMessage = new Message
            {

                SenderId = meUser.Id,
                SenderUsername = meUser.UserName,
                Sender = meUser,

                TargetId = message.targetId,
                TargetUsername = targetUser.UserName,
                Target = targetUser,

                Content = message.Content
            };


                _context.Messages.Add(newMessage);

                await _context.SaveChangesAsync(); 
                
                return Ok();

        }


        [HttpGet("inbox/{id}")]
        public async Task<List<Message>> Inbox(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var messages = await _context.Messages.Where(m => m.Target == user).OrderBy(e => e.Sender).ToListAsync();

            return messages;
        }

        [HttpGet("outbox/{id}")]
        public async Task<List<Message>> Outbox(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var messages = await _context.Messages.Where(m => m.Sender == user).OrderBy(e => e.Target).ToListAsync();

            return messages;
        }

        [HttpGet("thread/{username}/{otherUsername}")]
        public async Task<ActionResult<List<Message>>> MessagesThread(string currentUsername, string otherUsername)
        {
            return Ok(await _messageRepository.MessageThread(currentUsername, otherUsername));


        }

        //[HttpPut("{otherUsername}")]
        //public async Task<List<Message>> MessagesRead(string otherUsername)
        //{
        //    var usersMessages = await _context.Messages.Where(i => i.SenderUsername == otherUsername && i.DateRead == null).ToListAsync();

        //    foreach(var message in usersMessages)
        //    {
        //        message.DateRead = DateTime.Now;
        //    }

        //    return usersMessages;
        //}


        [HttpGet("unread-messages-count/{meid}")]
        public async Task<int> GetUnreadMessagesCount(string meId)
        {

            List<Message> unreadMessages = await _context.Messages.Where(o => o.TargetId == meId && o.DateRead == null).ToListAsync();

            var messagesCount = unreadMessages.Count();

            return messagesCount;
        }

    }
}
