namespace Furukawa.Requests.Account;

public class SetPasswordRequest
{
    public string NewPasswordSha512 { get; set; }
}