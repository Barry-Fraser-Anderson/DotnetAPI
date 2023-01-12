using System.Text;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using DotnetAPI.Models;
using DotnetAPI.Data;
using Dapper;

namespace DotnetAPI.Helpers
{
  public class AuthHelper
  {
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;
    public AuthHelper(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
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

    public bool SetPassword(UserLoginDto userLogin)
    {
      byte[] passwordSalt = new byte[128 / 8];
      using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
      {
        rng.GetNonZeroBytes(passwordSalt);
      }
      var passwordHash = GetPasswordHash(userLogin.Password, passwordSalt);

      string sqlAddAuth =
        "EXEC TutorialAppSchema.spRegistration_Upsert" +
        " @Email = @EmailParam" +
        ", @PasswordHash = @PasswordHashParam" +
        ", @PasswordSalt = @PasswordSaltParam";

      var sqlParameters = new DynamicParameters();
      sqlParameters.Add("@EmailParam", userLogin.Email, DbType.String);
      sqlParameters.Add("@PasswordHashParam", passwordHash, DbType.Binary);
      sqlParameters.Add("@PasswordSaltParam", passwordSalt, DbType.Binary);

      return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
    }
  }
}
