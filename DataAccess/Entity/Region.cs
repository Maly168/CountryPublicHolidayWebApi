namespace DataAccess.Entity
{
    public class Region
    {
        public int Id { get; set; }
        public SupportedCountry SupportedCountry { get; set; }
        public string Name { get; set; }
    }
}
