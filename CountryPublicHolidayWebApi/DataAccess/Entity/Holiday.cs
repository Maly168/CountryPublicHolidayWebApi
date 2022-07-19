namespace DataAccess.Entity
{
    public class Holiday
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int DayOfWeek { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
    }
}
