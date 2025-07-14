// Models/Member.cs
namespace MemberAPI.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string Email { get; set; }

        public int DemographicsId { get; set; }
        public Demographics Demographics { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }
    }
}

// Models/Demographics.cs
namespace MemberAPI.Models
{
    public class Demographics
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        // Encrypted SSN stored in DB
        public string SSN { get; set; }
    }
}

// Models/Address.cs
namespace MemberAPI.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}
