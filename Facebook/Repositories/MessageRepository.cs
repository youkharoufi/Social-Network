using AutoMapper;
using Facebook.Data;
using Facebook.Interface;
using Facebook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repositories
{
    public class MessageRepository : IMessageRepository
    {

        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public MessageRepository(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Message>> MessageThread(string username, string otherUsername)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == username);
            var otherUser = await _userManager.Users.FirstOrDefaultAsync(l => l.UserName == otherUsername);

            var messages = await _context.Messages.Where(
                    u => (u.Sender == user) && (u.Target == otherUser)
                                            ||
                    (u.Sender == otherUser) && (u.Target == user))
                .Include(o=>o.Sender)
                .Include(o=>o.Target)
                    
                    .OrderBy(z => z.MessageSent).ToListAsync();

            var unreadMessages = messages.Where(m => m.DateRead == null && m.TargetUsername == user.UserName).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }

            return messages;


        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group);
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups.Include(x => x.Connections).FirstOrDefaultAsync(x => x.Name == groupName);
        }
    }
}
