namespace SchoolLineup.Domain.Entities
{
    public class School : EntityWithId
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ManagerName { get; set; }
        public string Address { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}