using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoEFController : ControllerBase
{
  DataContextEF _entityFramework;

  public UserJobInfoEFController(IConfiguration config)
  {
    _entityFramework = new DataContextEF(config);
  }

  [HttpGet("UserJobInfo/{userId}")]
  public UserJobInfo GetUserJobInfo(int userId)
  {
    UserJobInfo? UserJobInfo = _entityFramework.UserJobInfo
      .Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();

    if (UserJobInfo != null) return UserJobInfo;

    throw new Exception("Failed to get UserJobInfo");
  }

  [HttpPost("UserJobInfo")]
  public IActionResult PostUserJobInfo(UserJobInfo UserJobInfo)
  {
    _entityFramework.UserJobInfo.Add(UserJobInfo);
    if (_entityFramework.SaveChanges() > 0)
    {
      return Ok();
    }

    throw new Exception("Failed to post UserJobInfo");
  }

  [HttpPut("UserJobInfo")]
  public IActionResult PutUserJobInfo(UserJobInfo UserJobInfo)
  {
    UserJobInfo? UserJobInfoDb = _entityFramework.UserJobInfo
      .Where(u => u.UserId == UserJobInfo.UserId).FirstOrDefault<UserJobInfo>();
    if (UserJobInfoDb != null)
    {
      UserJobInfoDb.UserId = UserJobInfo.UserId;
      UserJobInfoDb.JobTitle = UserJobInfo.JobTitle;
      UserJobInfoDb.Department = UserJobInfo.Department;

      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to update UserJobInfo");
  }


  [HttpDelete("UserJobInfo/{userId}")]
  public IActionResult DeleteUserJobInfo(int userId)
  {
    UserJobInfo? UserJobInfoDb = _entityFramework.UserJobInfo
      .Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();
    if (UserJobInfoDb != null)
    {
      _entityFramework.UserJobInfo.Remove(UserJobInfoDb);
      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to delete UserJobInfo");
  }
}
