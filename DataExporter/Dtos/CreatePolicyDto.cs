namespace DataExporter.Dtos
{
    public class CreatePolicyDto
    {
        public string PolicyNumber { get; set; } = null!;
        public decimal Premium { get; set; }
        public string StartDate { get; set; } = null!; // Changed this to accept as string so JSON always deserialises and can let FV enforce format.
    }
}
