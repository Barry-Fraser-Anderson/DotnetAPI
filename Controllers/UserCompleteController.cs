using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Data;
using Dapper;
using System.Data;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
  DataContextDapper _dapper;

  public UserCompleteController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
  }

  [HttpGet("GetUsers/{userId}/{isActive}")]
  public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
  {
    string sql =
      "EXEC TutorialAppSchema.spUsers_Get";

    string stringParameters = "";
    var sqlParameters = new DynamicParameters();

    if (userId != 0)
    {
      stringParameters += ", @UserId = @UserIdParameter";
      sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
    }
    if (isActive)
    {
      stringParameters += ", @Active = @ActiveParameter";
      sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean);
    }

    if (stringParameters.Length > 0)
    {
      sql += stringParameters.Substring(1);
    }

    var users = _dapper.LoadDataWithParameters<UserComplete>(sql, sqlParameters);
    return users;
  }

  [HttpPut("UpsertUser")]
  public IActionResult UpsertUser(UserComplete user)
  {
    string sql =
      "EXEC TutorialAppSchema.spUser_Upsert" +
      " @FirstName= @FirstNameParameter" +
      ", @LastName = @LastNameParameter" +
      ", @Email = @EmailParameter" +
      ", @Gender = @GenderParameter" +
      ", @JobTitle = @JobTitleParameter" +
      ", @Department = @DepartmentParameter" +
      ", @Salary = @SalaryParameter" +
      ", @UserId = @UserIdParameter";

    var sqlParameters = new DynamicParameters();

    sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
    sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
    sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
    sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
    sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
    sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
    sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
    sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Int32);
    sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

    if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters))
    {
      return Ok();
    }

    throw new Exception("Failed to update User");
  }

  [HttpDelete("DeleteUser/{userId}")]
  public IActionResult DeleteUser(int userId)
  {
    string sql =
      "EXEC TutorialAppSchema.spUser_Delete" +
      " @UserId = @UserIdParameter";

    var sqlParameters = new DynamicParameters();
    sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);

    _dapper.ExecuteSqlWithParameters(sql, sqlParameters);
    return Ok();
  }
}
