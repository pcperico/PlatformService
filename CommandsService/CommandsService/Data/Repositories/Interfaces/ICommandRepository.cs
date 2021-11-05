using CommandsService.Models;
using System.Collections.Generic;

namespace CommandsService.Data.Repositories.Interfaces
{
    public interface ICommandRepository
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatformExists(int id);
        bool ExternalPlatformExists(int externalPlatformId);


        IEnumerable<Command> GetCommandsForPlatform(int id);
        Command GetCommand(int platformId, int id);
        void CreateCommand(int platformId, Command command);
        
    }
}
