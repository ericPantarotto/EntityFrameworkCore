using EntityFrameworkCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkNet5.Data
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.HasIndex(h => h.Name);
            
            builder.HasMany(m => m.HomeMatches)
                .WithOne(m => m.HomeTeam)
                .HasForeignKey(m => m.HomeTeamId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            builder.HasMany(m => m.AwayMatches)
                .WithOne(m => m.AwayTeam)
                .HasForeignKey(m => m.AwayTeamId)
                .IsRequired()
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            
            builder.HasData(
                new Team{ Id = 20, Name="Eric Carlier - Sample Team", LeagueId = 20 },
                new Team{ Id = 21, Name="Estelle Carlier - Sample Team", LeagueId = 20 },
                new Team{ Id = 22, Name="Francoise Carlier - Sample Team", LeagueId = 20 }
            );
        }
    }
}