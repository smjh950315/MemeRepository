using Cyh.EFCore.Interface;
using MemeRepository.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace MemeRepository.Db.Accesser
{
    public class MyDbContext : MemeRepositoryContext, IDbContext
    {
        public MyDbContext() { }
        public MyDbContext(DbContextOptions<MemeRepositoryContext> options)
            : base(options) {
        }
    }
}
