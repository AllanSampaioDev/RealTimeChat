namespace Api.Contracts;

public class UserCreateRequest
{
    public string Name { get; set; }
    public string Password { get; set; }

    public UserCreateRequest(string name, string password)
    {
        Name = name;
        Password = password;
    }
}
