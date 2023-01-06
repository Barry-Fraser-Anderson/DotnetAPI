namespace DotnetAPI
{
  public partial class UserDto
  {
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Gender { get; set; } = "";
    public bool Active { get; set; }
  }

  public partial class UserRegDto
  {
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string PasswordConfirm { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Gender { get; set; } = "";
  }

  public partial class UserLoginDto
  {
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
  }

  public partial class UserLoginConfirmDto
  {
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];
  }
}
