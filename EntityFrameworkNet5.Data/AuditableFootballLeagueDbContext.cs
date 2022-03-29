using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Data
{
    public class AuditableFootballLeagueDbContext : DbContext
    {
        public DbSet<Audit> Audits { get; set; }
        public async Task<int> SaveChangesAsync(string userName)
        {
            var auditEntries = OnBeforeSaveChanges(userName);

            var saveResults = await base.SaveChangesAsync();
            if (auditEntries != null || auditEntries.Count > 0)
            {
                await OnAfterSaveChanges(auditEntries); 
            }

            return saveResults;
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                Audits.Add(auditEntry.ToAudit());
            }

            return SaveChangesAsync();

        }

        private List<AuditEntry> OnBeforeSaveChanges(string userName)
        {
            List<AuditEntry> auditEntries = new();
            var entries = ChangeTracker.Entries().Where(q => q.State == EntityState.Added || q.State == EntityState.Modified || q.State == EntityState.Deleted).ToList();

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

                AuditEntry auditEntry = new(entry);
                auditEntry.TableName = entry.Metadata.GetTableName();
                auditEntry.Action = entry.State.ToString();
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch(entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;

                    }
                }

            });

            foreach (var pendingAuditEntry in auditEntries.Where(q => q.HasTemporaryProperties == false))
            {
                Audits.Add(pendingAuditEntry.ToAudit());
            }
            
            return auditEntries.Where(q => q.HasTemporaryProperties).ToList();
        }
    }
}