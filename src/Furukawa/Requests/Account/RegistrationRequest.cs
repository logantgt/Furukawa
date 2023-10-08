namespace Furukawa.Requests.Account;

public class RegistrationRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordSha512 { get; set; }
}