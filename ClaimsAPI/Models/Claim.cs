// Models/Claim.cs
namespace ClaimsAPI.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public string MemberId { get; set; }     // MemberAPI ref
        public string ClaimNumber { get; set; }
        public DateTime DateOfService { get; set; }
        public string DiagnosisCode { get; set; }
        public decimal AmountBilled { get; set; }
        public string Status { get; set; }       // e.g., Submitted, Approved, Denied
    }
}
