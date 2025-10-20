namespace DataExporter.Dtos
{
    public class ReadPolicyDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = null!; // Null forgiving operator because this will always be initialised when mapping from Policy entity.
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
    }
}
