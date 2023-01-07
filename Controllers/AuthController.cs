using System.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DotnetAPI.Helpers;

namespace DotnetAPI.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly DataContextDapper _dapper;
    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
      _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserRegDto userReg)
    {
      if (userReg.Password == userReg.PasswordConfirm)
      {
        string sqlGetAuth =
          "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
            userReg.Email + "'";
        IEnumerable<string> users = _dapper.LoadData<string>(sqlGetAuth);
        if (users.Count() == 0)
        {
          byte[] passwordSalt = new byte[128 / 8];
          using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
          {
            rng.GetNonZeroBytes(passwordSalt);
          }
          var passwordHash = _authHelper.GetPasswordHash(userReg.Password, passwordSalt);

          string sqlAddAuth =
            "INSERT INTO TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt)" +
            " VALUES ('" + userReg.Email + "',@PasswordHash, @PasswordSalt)";

          List<SqlParameter> sqlParameters = new List<SqlParameter>();

          var passwordHashParam = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
          passwordHashParam.Value = passwordHash;
          var passwordSaltParam = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
          passwordSaltParam.Value = passwordSalt;

          sqlParameters.Add(passwordHashParam);
          sqlParameters.Add(passwordSaltParam);

          if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
          {
            string sqlAddUser =
              "INSERT INTO TutorialAppSchema.Users (FirstName,LastName,Email,Gender,Active)" +
              $" VALUES ('{userReg.FirstName}','{userReg.LastName}','{userReg.Email}','{userReg.Gender}','1')";
            if (_dapper.ExecuteSql(sqlAddUser))
            {
              return Ok();
            }
            throw new Exception("Failed to add user");
          }
          throw new Exception("Failed to register user");
        }
        throw new Exception("A user with this email already exists!");
      }

      throw new Exception("Passwords do not match");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserLoginDto userLogin)
    {
      string sqlLoginConfirm =
        "SELECT PasswordHash,PasswordSalt FROM TutorialAppSchema.Auth" +
        " WHERE Email = '" + userLogin.Email + "'";
      var userConfirm = _dapper.LoadDataSingle<UserLoginConfirmDto>(sqlLoginConfirm);
      var passwordHash = _authHelper.GetPasswordHash(userLogin.Password, userConfirm.PasswordSalt);
      for (var index = 0; index < passwordHash.Length; index++)
      {
        if (passwordHash[index] != userConfirm.PasswordHash[index])
        {
          return StatusCode(401, "Incorrect password");
        }
      }

      string userIdSql =
        $"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '{userLogin.Email}'";
      int userId = _dapper.LoadDataSingle<int>(userIdSql);

      return Ok(new Dictionary<string, string> {
        { "token", _authHelper.CreateToken(userId) }
      });
    }

    [HttpGet("RefreshToken")]
    public string RefreshToken()
    {
      string userIdSql =
        "SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" +
        User.FindFirst("userId")?.Value + "'";
      int userId = _dapper.LoadDataSingle<int>(userIdSql);

      return _authHelper.CreateToken(userId);
    }
  }
}
