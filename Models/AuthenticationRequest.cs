using Azure.Identity;

namespace WebAppApi.Models
{
    public class AuthenticationRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
