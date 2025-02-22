using System;

namespace EntityFrameworkCore.Domain
{
    public class BaseDomainObject
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}