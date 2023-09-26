
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using System.Collections.Generic;

namespace SchoolManagementSystem.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
    
      

        public DbSet<RoleDetails> RoleDetails { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Register> Register { get; set; }
        public DbSet<Categories> Categories { get; set; }

        public DbSet<ClassMaster> ClassMaster { get; set; }

        public DbSet<SubjectMaster> SubjectMaster { get; set; }

        public DbSet<CountryMaster> CountryMaster { get; set; }

        public DbSet<DistrictMaster> DistrictMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
             public DbSet<Module> Module { get; set; }
        public DbSet<ModuleRoleMapping> ModuleRoleMapping { get; set; }
    }
}
