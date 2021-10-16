using System;
using PlatformService.Data.Repositories.Interfaces;
using PlatformService.Models;
using System.Collections.Generic;
using System.Linq;

namespace PlatformService.Data.Repositories.Impl
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentException(nameof(platform));
            _context.Platforms.Add(platform);
            _context.SaveChanges();
        }

        public void DeletePlatform(int id)
        {
            var platform = GetPlatformById(id);
            if(platform!=null)
                _context.Platforms.Remove(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
