namespace Furukawa.Requests.Account
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string PasswordSha512 { get; set; }
    }
}