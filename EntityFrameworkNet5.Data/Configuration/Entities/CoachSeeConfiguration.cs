using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkNet5.Data
{
    public class CoachSeedConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasData(
                new Coach{ Id = 20, Name="Eric Carlier", TeamId = 20 },
                new Coach{ Id = 21, Name="Estelle Carlier", TeamId = 21 },
                new Coach{ Id = 22, Name="Francoise Carlier", TeamId = 22 }
            );
        }
    }
}