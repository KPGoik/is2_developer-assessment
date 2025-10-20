using System.ComponentModel.DataAnnotations.Schema;

namespace DataExporter.Model
{
    public class Policy
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = null!; //Null forgiving operator because EF Core will initialise, or be validated by PostPoliciesValidator.
        public decimal Premium { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
