namespace DataExporter.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!; // Null forgiving operator because EF Core will always initialise this property. There are no methods to create Notes other than that.
        public int PolicyId { get; set; }
    }
}
