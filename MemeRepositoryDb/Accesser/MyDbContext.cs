using Cyh.EFCore.Interface;
using MemeRepository.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace MemeRepository.Db.Accesser
{
    /// <summary>
    /// This class is used to access the database.
    /// </summary>
    public class MyDbContext : MemeRepositoryContext, IDbContext
    {
        public MyDbContext() { }
        public MyDbContext(DbContextOptions<MemeRepositoryContext> options)
            : base(options) {
        }
    }
}
