// Data/MemberDbContext.cs
using Microsoft.EntityFrameworkCore;
using MemberAPI.Models;

namespace MemberAPI.Data
{
    public class MemberDbContext : DbContext
    {
        public MemberDbContext(DbContextOptions<MemberDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Demographics> Demographics { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
