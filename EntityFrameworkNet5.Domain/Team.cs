namespace EntityFrameworkCore.Domain
{
    public class Team: BaseDomainObject
    {
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public virtual League League { get; set; }
    } 
}