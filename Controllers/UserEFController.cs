using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
  DataContextEF _entityFramework;
  IMapper _mapper;

  public UserEFController(IConfiguration config)
  {
    _entityFramework = new DataContextEF(config);

    _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<UserDto, User>();
    }));
  }


  [HttpGet("GetUsers")]
  //public IEnumerable<User> GetUsers()
  public IEnumerable<User> GetUsers()
  {
    var users = _entityFramework.Users.ToList<User>();
    return users;
  }

  [HttpGet("GetSingleUser/{userId}")]
  public User GetSingleUser(int userId)
  {
    User? user = _entityFramework.Users
      .Where(u => u.UserId == userId).FirstOrDefault<User>();
    if (user != null) return user;

    throw new Exception("Failed to get User");
  }

  [HttpPut("EditUser")]
  public IActionResult EditUser(User user)
  {
    User? userDb = _entityFramework.Users
      .Where(u => u.UserId == user.UserId).FirstOrDefault<User>();
    if (userDb != null)
    {
      userDb.FirstName = user.FirstName;
      userDb.LastName = user.LastName;
      userDb.Email = user.Email;
      userDb.Gender = user.Gender;
      userDb.Active = user.Active;

      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to update User");
  }

  [HttpPost("AddUser")]
  public IActionResult AddUser(UserDto user)
  {
    User userDb = _mapper.Map<User>(user);

    _entityFramework.Users.Add(userDb);
    if (_entityFramework.SaveChanges() > 0)
    {
      return Ok();
    }

    throw new Exception("Failed to add User");
  }

  [HttpDelete("DeleteUser/{userId}")]
  public IActionResult DeleteUser(int userId)
  {
    User? userDb = _entityFramework.Users
      .Where(u => u.UserId == userId).FirstOrDefault<User>();
    if (userDb != null)
    {
      _entityFramework.Users.Remove(userDb);
      if (_entityFramework.SaveChanges() > 0)
      {
        return Ok();
      }
    }

    throw new Exception("Failed to delete User");
  }

  // UserJobInfo
  [HttpGet("UserJobInfo/{userId}")]
  public UserJobInfo GetUserJobInfo(int userId)
  {
    UserJobInfo? UserJobInfo = _entityFramework.UserJobInfo
      .Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();

    if (UserJobInfo != null) return UserJobInfo;

    throw new Exception("Failed to get UserJobInfo");
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

  // UserSalary
  [HttpGet("UserSalary/{userId}")]
  public UserSalary GetUserSalary(int userId)
  {
    UserSalary? userSalary = _entityFramework.UserSalary
      .Where(u => u.UserId == userId).FirstOrDefault<UserSalary>();

    if (userSalary != null) return userSalary;

    throw new Exception("Failed to get UserSalary");
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
