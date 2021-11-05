using System.Collections.Generic;
using System.Threading.Tasks;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public interface ICommandAPIRepo
    {
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Command>> GetAllCommands();

        Task<Command> GetCommandById(int id);

        Task<IEnumerable<Command>> GetCommandByIds(IEnumerable<int> ids);

        Task CreateCommand(Command cmd);

        void UpdateCommand(Command cmd);

        void DeleteCommand(Command cmd);
        void DeleteCommands(IEnumerable<Command> cmds);
    }
}