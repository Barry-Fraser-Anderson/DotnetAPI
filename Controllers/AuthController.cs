using System.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DotnetAPI.Helpers;
using Dapper;

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
        string sqlCheckUserExists =
          "SELECT Email FROM TutorialAppSchema.Auth" +
          " WHERE Email = '" + userReg.Email + "'";

        IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
        if (existingUsers.Count() == 0)
        {
          var userForSetPassword = new UserLoginDto()
          {
            Email = userReg.Email,
            Password = userReg.Password
          };
          if (_authHelper.SetPassword(userForSetPassword))
          {
            string sqlAddUser =
              "EXEC TutorialAppSchema.spUser_Upsert " +
              $" @FirstName = '{userReg.FirstName}'" +
              $", @LastName = '{userReg.LastName}'" +
              $", @Email = '{userReg.Email}'" +
              $", @Gender = '{userReg.Gender}" +
              $", @Active = '1'" +
              $", @JobTitle = '{userReg.JobTitle}'" +
              $", @Department = '{userReg.Department}'" +
              $", @Salary = {userReg.Salary}";
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

    [HttpPut("ResetPassword")]
    public IActionResult ResetPassword(UserLoginDto userForSetPassword)
    {
      if (_authHelper.SetPassword(userForSetPassword))
      {
        return Ok();
      }

      throw new Exception("Failed to update password");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserLoginDto userLogin)
    {
      string sqlForHashAndSalt =
        "EXEC TutorialAppSchema.spLoginConfirmation_Get" +
        " @Email = '" + userLogin.Email + "'";

      var sqlParameters = new DynamicParameters();
      sqlParameters.Add("@EmailParam", userLogin.Email, DbType.String);

      var userConfirm = _dapper.LoadDataSingleWithParams<UserLoginConfirmDto>(sqlForHashAndSalt, sqlParameters);
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
        "SELECT UserId FROM TutorialAppSchema.Users" +
        " WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";
      int userId = _dapper.LoadDataSingle<int>(userIdSql);

      return _authHelper.CreateToken(userId);
    }
  }
}
