using System;

namespace EntityFrameworkCore.Domain
{
    public class Audit
    {
        public int Id { get; set; }
        public string Action { get; set; }          
        public string TableName { get; set; }          
        public DateTime DateTime { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }

    }
}