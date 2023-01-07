using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
  DataContextDapper _dapper;

  public UserController(IConfiguration config)
  {
    _dapper = new DataContextDapper(config);
  }

  [HttpGet("GetUsers")]
  //public IEnumerable<User> GetUsers()
  public IEnumerable<User> GetUsers()
  {
    string sql =
      "SELECT UserId, FirstName, LastName, Email, Gender, Active" +
      " FROM TutorialAppSchema.Users";

    IEnumerable<User> users = _dapper.LoadData<User>(sql);
    return users;
  }

  [HttpGet("GetSingleUser/{userId}")]
  public User GetSingleUser(int userId)
  {
    string sql =
      "SELECT UserId, FirstName, LastName, Email, Gender, Active" +
      " FROM TutorialAppSchema.Users" +
      " WHERE UserId = " + userId.ToString();

    User user = _dapper.LoadDataSingle<User>(sql);
    if (user != null) return user;

    throw new Exception("Failed to get User");
  }

  [HttpPut("EditUser")]
  public IActionResult EditUser(User user)
  {
    string sql =
       "UPDATE TutorialAppSchema.Users" +
      $" SET FirstName = '{user.FirstName}'," +
      $" LastName = '{user.LastName}'," +
      $" Email = '{user.Email}'," +
      $" Gender = '{user.Gender}'," +
      $" Active = '{(user.Active ? 1 : 0)}' " +
      $"WHERE UserId = {user.UserId.ToString()}";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to update User");
  }

  [HttpPost("AddUser")]
  public IActionResult AddUser(UserDto user)
  {
    string sql =
      "INSERT INTO TutorialAppSchema.Users(" +
      " FirstName," +
      " LastName," +
      " Email," +
      " Gender," +
      " Active" +
      ") VALUES (" +
     $" '{user.FirstName}'," +
     $" '{user.LastName}'," +
     $" '{user.Email}'," +
     $" '{user.Gender}'," +
     $" '{(user.Active ? 1 : 0)}'" +
      ")";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to add User");
  }

  [HttpDelete("DeleteUser/{userId}")]
  public IActionResult DeleteUser(int userId)
  {
    string sql =
      "DELETE FROM TutorialAppSchema.Users" +
      " WHERE UserId = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }

  // UserJobInfo
  [HttpGet("UserJobInfo/{userId}")]
  public UserJobInfo GetUserJobInfo(int userId)
  {
    string sql =
      "SELECT UserId, JobTitle, Department" +
      " FROM TutorialAppSchema.UserJobInfo" +
      " WHERE UserId = " + userId.ToString();

    UserJobInfo? userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sql);
    if (userJobInfo != null) return userJobInfo;

    throw new Exception("Failed to get UserJobInfo");
  }

  [HttpPut("UserJobInfo")]
  public IActionResult PutUserJobInfo(UserJobInfo userJobInfo)
  {
    string sql =
      "UPDATE TutorialAppSchema.UserJobuserJobInfo" +
      " SET JobTitle = '" + userJobInfo.JobTitle + "', Department = '" + userJobInfo.Department + "'" +
      " WHERE UserId = " + userJobInfo.UserId.ToString();

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to update UserJobInfo");
  }

  [HttpPost("UserJobInfo")]
  public IActionResult PostUserJobInfo(UserJobInfo userJobInfo)
  {
    string sql =
      "INSERT INTO TutorialAppSchema.UserJobInfo (JobTitle, Department)" +
      " VALUES ('" + userJobInfo.JobTitle + "', '" + userJobInfo.Department + "')";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to add UserJobInfo");
  }

  [HttpDelete("UserJobInfo/{userId}")]
  public IActionResult DeleteUserJobInfo(int userId)
  {
    string sql =
      "DELETE FROM TutorialAppSchema.UserJobInfo" +
      " WHERE UserId = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }

  // UserSalary
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

  [HttpPost("UserSalary")]
  public IActionResult PostUserSalary(UserSalary userSalary)
  {
    string sql =
      "INSERT INTO TutorialAppSchema.UserSalary (UserId, Salary)" +
      " VALUES(" + userSalary.UserId.ToString() + ", " + userSalary.Salary.ToString() + ")";

    if (_dapper.ExecuteSql(sql))
    {
      return Ok();
    }

    throw new Exception("Failed to post UserSalary");
  }

  [HttpDelete("UserSalary/{userId}")]
  public IActionResult DeleteUserSalary(int userId)
  {
    string sql =
      "DELETE FROM TutorialAppSchema.UserSalary" +
      " WHERE UserId = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }
}
