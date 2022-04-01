using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkNet5.Data
{
    public class TestModelConfiguration : IEntityTypeConfiguration<TestModel>
    {
        public void Configure(EntityTypeBuilder<TestModel> builder)
        {
            builder.HasOne(p => p.League);  

            builder.Property(p => p.Name).HasMaxLength(50);
            builder.HasIndex(h => h.Name);
            builder.Property(p => p.testDecimal).HasPrecision(18,2);
            
            builder.HasData(
                new TestModel{ Id = 20, LeagueId = 20, Name="Eric Carlier - Sample TestModel", testDecimal = 5.252M }                
            );
        }
    }
}