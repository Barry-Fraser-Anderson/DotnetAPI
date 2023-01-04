using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
  DataContextDapper _dapper;

  public UserJobInfoController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
  }

  [HttpGet("UserJobInfo/{userId}")]
  public UserJobInfo GetUserJobInfo(int userId)
  {
    string sql =
      "SELECT UserId,JobTitle, Department" +
      " FROM TutorialAppSchema.UserJobInfo" +
      " WHERE UserId = " + userId.ToString();

    UserJobInfo? userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
    if (userJobInfo != null) return userJobInfo;

    throw new Exception("Failed to get UserJobInfo");
  }


  [HttpPost("UserJobInfo")]
  public IActionResult PostUserJobInfo(UserJobInfo userJobInfo)
  {
    string sql =
      "INSERT INTO TutorialAppSchema.UserJobInfo(JobTitle,Department)" +
      " VALUES('" + userJobInfo.JobTitle + "','" + userJobInfo.Department + "')";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to add UserJobInfo");
  }

  [HttpPut("UserJobInfo")]
  public IActionResult PutUserJobInfo(UserJobInfo userJobInfo)
  {
    string sql =
      "UPDATE TutorialAppSchema.UserJobuserJobInfo" +
      " SET JobTitle = '" + userJobInfo.JobTitle + "','Department = '" + userJobInfo.Department + "' " +
      "WHERE UserId = " + userJobInfo.UserId.ToString();

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to update UserJobInfo");
  }

  [HttpDelete("UserJobInfo/{userId}")]
  public IActionResult DeleteUserJobInfo(int userId)
  {
    string sql =
      "DELETE FROM TutorialAppSchema.UserJobInfo" +
      " WHERE Userid = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }
}
