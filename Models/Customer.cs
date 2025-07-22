using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Customer
    {
        [MinLength(2)]  
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }

}
}
