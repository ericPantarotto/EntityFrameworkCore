using System.Collections.Generic;

namespace EntityFrameworkCore.Domain
{
    public class League: BaseDomainObject
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
    
}