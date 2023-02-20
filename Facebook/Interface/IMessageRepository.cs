using Facebook.Models;

namespace Facebook.Interface
{
    public interface IMessageRepository
    {

        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<bool> SaveAllAsync();

        Task<List<Message>> MessageThread(string username, string otherUsername);

        void AddGroup(Group group);

        void RemoveConnection(Connection connection);

        Task<Connection> GetConnection(string connectionId);

        Task<Group> GetMessageGroup(string groupName);
        
    }
}
