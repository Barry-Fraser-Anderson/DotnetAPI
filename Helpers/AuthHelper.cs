using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
  public class AuthHelper
  {
    private readonly IConfiguration _config;
    public AuthHelper(IConfiguration config)
    {
      _config = config;
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
      string passwordSaltPlusString =
        _config.GetSection("Appsettings:PasswordKey").Value +
          Convert.ToBase64String(passwordSalt);

      return KeyDerivation.Pbkdf2(
        password: password,
        salt: System.Text.Encoding.ASCII.GetBytes(passwordSaltPlusString),
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 1000,
        numBytesRequested: 256 / 8
      );
    }

    public string CreateToken(int userId)
    {
      Claim[] claims = new Claim[] {
        new Claim("userId", userId.ToString())
      };

      string tokenKeyStr = _config.GetSection("AppSettings:TokenKey").Value ??= "";
      var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyStr));

      var credentials = new SigningCredentials(
        tokenKey, SecurityAlgorithms.HmacSha256Signature);

      var descriptor = new SecurityTokenDescriptor()
      {
        Subject = new ClaimsIdentity(claims),
        SigningCredentials = credentials,
        Expires = DateTime.Now.AddDays(1)
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(descriptor);

      return tokenHandler.WriteToken(token);
    }
  }
}
