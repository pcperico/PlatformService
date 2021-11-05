using CommandsService.Data.Repositories.Interfaces;
using CommandsService.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Data.Repositories.Impl
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if(command!=null )
            {
                command.PlatformId = platformId;
                _context.Commands.Add(command);
                SaveChanges();
            }
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform != null)
            {
                _context.Platforms.Add(platform);
                SaveChanges();
            }
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _context.Platforms.Any(x => x.ExternalID == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int id)
        {
            return _context.Commands.FirstOrDefault(x => x.PlatformId == platformId && x.Id == id);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int id)
        {
            return _context.Commands.Where(x => x.PlatformId == id);
        }

        public bool PlatformExists(int id)
        {
            return _context.Platforms.Any(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
