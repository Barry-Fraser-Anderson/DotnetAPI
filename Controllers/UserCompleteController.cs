using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using DotnetAPI.Models;
using DotnetAPI.Data;
using DotnetAPI.Helpers;
using Dapper;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
  private readonly DataContextDapper _dapper;
  private readonly ReusableSQL _reusableSql;
  public UserCompleteController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
    _reusableSql = new ReusableSQL(config);
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
    if (_reusableSql.UpsertUser(user))
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
