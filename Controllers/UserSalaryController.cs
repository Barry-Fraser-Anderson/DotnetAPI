using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryController : ControllerBase
{
  DataContextDapper _dapper;

  public UserSalaryController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
  }

  [HttpGet("UserSalary/{userId}")]
  public UserSalary GetUserSalary(int userId)
  {
    string sql =
      "SELECT UserId, Salary" +
      " FROM TutorialAppSchema.UserSalary" +
      " WHERE UserId = " + userId.ToString();

    UserSalary? userSalary = _dapper.LoadDataSingle<UserSalary>(sql);

    if (userSalary != null) return userSalary;

    throw new Exception("Failed to get UserSalary");
  }

  [HttpPost("UserSalary")]
  public IActionResult PostUserSalary(UserSalary userSalary)
  {
    string sql =
      "INSERT INTO TutorialAppSchema.UserSalary(UserId, Salary)" +
      " VALUES(" + userSalary.UserId.ToString() + "," + userSalary.Salary.ToString() + ")";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to post UserSalary");
  }

  [HttpPut("UserSalary")]
  public IActionResult PutUserSalary(UserSalary userSalary)
  {
    string sql =
      "UPDATE TutorialAppSchema.UserSalary" +
      " SET Salary = " + userSalary.Salary.ToString() +
      " WHERE UserId = " + userSalary.UserId.ToString();

    Console.WriteLine(sql);
    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to update UserSalary");
  }


  [HttpDelete("UserSalary/{userId}")]
  public IActionResult DeleteUserSalary(int userId)
  {
    string sql =
      "DELETE FROM TutorialAppSchema.UserSalary" +
      " WHERE Userid = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }
}
