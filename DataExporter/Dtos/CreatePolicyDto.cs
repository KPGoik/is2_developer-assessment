namespace DataExporter.Dtos
{
    public class CreatePolicyDto
    {
        public string PolicyNumber { get; set; }
        public decimal Premium { get; set; }
        public string StartDate { get; set; } // Changed this to accept as string so JSON always deserialises and can let FV can enforce format.
    }
}
