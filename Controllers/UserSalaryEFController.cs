using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryEFController : ControllerBase
{
  DataContextEF _entityFramework;

  public UserSalaryEFController(IConfiguration config)
  {
    _entityFramework = new DataContextEF(config);
  }

  [HttpGet("UserSalary/{userId}")]
  public UserSalary GetUserSalary(int userId)
  {
    UserSalary? userSalary = _entityFramework.UserSalary
      .Where(u => u.UserId == userId).FirstOrDefault<UserSalary>();

    if (userSalary != null) return userSalary;

    throw new Exception("Failed to get UserSalary");
  }

  [HttpPost("UserSalary")]
  public IActionResult PostUserSalary(UserSalary userSalary)
  {
    _entityFramework.UserSalary.Add(userSalary);
    if (_entityFramework.SaveChanges() > 0)
    {
      return Ok();
    }

    throw new Exception("Failed to post UserSalary");
  }

  [HttpPut("UserSalary")]
  public IActionResult PutUserSalary(UserSalary userSalary)
  {
    UserSalary? userSalaryDb = _entityFramework.UserSalary
      .Where(u => u.UserId == userSalary.UserId).FirstOrDefault<UserSalary>();
    if (userSalaryDb != null)
    {
      userSalaryDb.UserId = userSalary.UserId;
      userSalaryDb.Salary = userSalary.Salary;

      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to update UserSalary");
  }


  [HttpDelete("UserSalary/{userId}")]
  public IActionResult DeleteUserSalary(int userId)
  {
    UserSalary? userSalaryDb = _entityFramework.UserSalary
      .Where(u => u.UserId == userId).FirstOrDefault<UserSalary>();
    if (userSalaryDb != null)
    {
      _entityFramework.UserSalary.Remove(userSalaryDb);
      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to delete UserSalary");
  }
}
