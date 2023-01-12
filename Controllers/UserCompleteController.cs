using Microsoft.AspNetCore.Mvc;

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

    string parameters = "";
    if (userId != 0)
    {
      parameters += ", @UserId = " + userId.ToString();
    }
    if (isActive)
    {
      parameters += ", @active = " + isActive.ToString();
    }
    if (parameters.Length > 0)
    {
      sql += parameters.Substring(1);
    }

    IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sql);
    return users;
  }

  [HttpPut("UpsertUser")]
  public IActionResult UpsertUser(UserComplete user)
  {
    string sql =
      "EXEC TutorialAppSchema.spUser_Upsert " +
      $" @FirstName = '{user.FirstName}'" +
      $", @LastName = '{user.LastName}'" +
      $", @Email = '{user.Email}'" +
      $", @Gender = '{user.Gender}" +
      $", @JobTitle = '{user.JobTitle}'" +
      $", @Department = '{user.Department}'" +
      $", @Salary = {user.Salary}" +
      $", @Active = {user.Active}" +
      $", @UserId = {user.UserId}";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to update User");
  }

  [HttpDelete("DeleteUser/{userId}")]
  public IActionResult DeleteUser(int userId)
  {
    string sql =
      "EXEC TutorialAppSchema.spUser_Delete @UserId = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }
}