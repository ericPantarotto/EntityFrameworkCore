using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkNet5.Data
{
    public class TeamSeedConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasData(
                new Team{ Id = 20, Name="Eric Carlier - Sample Team", LeagueId = 20 },
                new Team{ Id = 21, Name="Estelle Carlier - Sample Team", LeagueId = 20 },
                new Team{ Id = 22, Name="Francoise Carlier - Sample Team", LeagueId = 20 }
            );
        }
    }
}