using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandAPI.Data
{
    public class SqlCommandAPIRepo : ICommandAPIRepo
    {
        private readonly CommandContext _context;

        public SqlCommandAPIRepo(CommandContext context)
        {
            _context = context;
        }

        public async Task CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            await _context.CommandItems.AddAsync(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }

            _context.CommandItems.Remove(cmd);
        }

        public void DeleteCommands(IEnumerable<Command> cmds)
        {
            _context.CommandItems.RemoveRange(cmds);
        }

        public async Task<IEnumerable<Command>> GetAllCommands()
        {
            return await _context.CommandItems.ToListAsync();
        }

        public async Task<Command> GetCommandById(int id)
        {
            return await _context.CommandItems.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Command>> GetCommandByIds(IEnumerable<int> ids)
        {
            return await _context.CommandItems.Where(c => ids.Contains(c.Id)).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void UpdateCommand(Command cmd)
        {
            _context.CommandItems.Update(cmd);
        }
    }
}