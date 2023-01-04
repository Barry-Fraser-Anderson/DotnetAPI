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
      " WHERE Userid = " + userId.ToString();

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

    Console.WriteLine(sql);
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
      "FirstName," +
      "LastName," +
      "Email," +
      "Gender," +
      "Active" +
      ") VALUES (" +
     $"'{user.FirstName}'," +
     $"'{user.LastName}'," +
     $"'{user.Email}'," +
     $"'{user.Gender}'," +
     $"'{(user.Active ? 1 : 0)}'" +
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
      " WHERE Userid = " + userId.ToString();

    _dapper.ExecuteSql(sql);
    return Ok();
  }
}
