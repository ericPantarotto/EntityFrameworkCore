using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace EntityFrameworkCore.Data
{
    internal class AuditEntry
    {
        public EntityEntry EntityEntry { get; set; }
        public AuditEntry(EntityEntry entityEntry)
        {
            EntityEntry = entityEntry;
        }
        public string TableName { get; set; } 
        public string Action { get; set; }         
        public DateTime DateTime { get; set; }
        public Dictionary<string, object> KeyValues { get; set; } = new();
        public Dictionary<string, object> OldValues { get; set; } = new();
        public Dictionary<string, object> NewValues { get; set; } = new();
        public List<PropertyEntry> TemporaryProperties { get; set; } = new();
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public Audit ToAudit()
        {
            Audit audit = new()
            {
                DateTime = DateTime.Now,
                TableName = TableName,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues),
                Action = Action
            };
            return audit;
            
        }

    }
}