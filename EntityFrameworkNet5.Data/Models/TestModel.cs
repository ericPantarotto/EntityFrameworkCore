using System;
using EntityFrameworkNet5.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Domain
{   
    [EntityTypeConfiguration(typeof(TestModelConfiguration))]
    public class TestModel: BaseDomainObject
    {
        public int LeagueId { get; set; }
        public virtual League League { get; set; }
        public string Name { get; set; }
        public decimal testDecimal { get; set; }
    }
}