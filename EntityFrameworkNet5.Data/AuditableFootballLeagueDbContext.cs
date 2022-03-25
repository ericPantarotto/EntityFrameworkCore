using System;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data
{
    public class AuditableFootballLeagueDbContext : DbContext
    {
        public async Task<int> SaveChangesAsync(string userName)
        {
            var entries = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified).ToList();
            entries.ForEach(entry => 
            {
                var auditableObject = (BaseDomainObject)entry.Entity;
                auditableObject.ModifiedDate = DateTime.Now;
                auditableObject.ModifiedBy = userName;
                if (entry.State == EntityState.Added)
                {
                    auditableObject.CreatedDate = DateTime.Now;
                    auditableObject.CreatedBy = userName;
                }
            });
            return await base.SaveChangesAsync();
        }
    }
}