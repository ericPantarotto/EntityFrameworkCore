namespace EntityFrameworkCore.Domain
{
    public class BaseDomainObject
    {
        public int Id { get; set; }
        public DateTime DateCrated { get; set; }
        public DateTime LastModified { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }
}