using AutoMapper;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
  IUserRepository _userRepository;
  IMapper _mapper;

  public UserEFController(IConfiguration config, IUserRepository userRepository)
  {
    _userRepository = userRepository;

    _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<UserDto, User>();
    }));
  }

  [HttpGet("GetUsers")]
  public IEnumerable<User> GetUsers()
  {
    return _userRepository.GetUsers();
  }

  [HttpGet("GetSingleUser/{userId}")]
  public User GetSingleUser(int userId)
  {
    return _userRepository.GetSingleUser(userId);
  }

  [HttpPut("EditUser")]
  public IActionResult EditUser(User user)
  {
    User? userDb = _userRepository.GetSingleUser(user.UserId);
    if (userDb != null)
    {
      userDb.FirstName = user.FirstName;
      userDb.LastName = user.LastName;
      userDb.Email = user.Email;
      userDb.Gender = user.Gender;
      userDb.Active = user.Active;

      if (_userRepository.SaveChanges())
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

    _userRepository.AddEntity<User>(userDb);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }

    throw new Exception("Failed to add User");
  }

  [HttpDelete("DeleteUser/{userId}")]
  public IActionResult DeleteUser(int userId)
  {
    User? userDb = _userRepository.GetSingleUser(userId);
    if (userDb != null)
    {
      _userRepository.RemoveEntity<User>(userDb);
      if (_userRepository.SaveChanges())
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
    return _userRepository.GetUserJobInfo(userId);
  }

  [HttpPut("UserJobInfo")]
  public IActionResult PutUserJobInfo(UserJobInfo userJobInfo)
  {
    UserJobInfo? userJobInfoDb = _userRepository.GetUserJobInfo(userJobInfo.UserId);
    if (userJobInfoDb != null)
    {
      userJobInfoDb.UserId = userJobInfo.UserId;
      userJobInfoDb.JobTitle = userJobInfo.JobTitle;
      userJobInfoDb.Department = userJobInfo.Department;

      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
    }

    throw new Exception("Failed to update UserJobInfo");
  }

  [HttpPost("UserJobInfo")]
  public IActionResult PostUserJobInfo(UserJobInfo userJobInfo)
  {
    _userRepository.AddEntity<UserJobInfo>(userJobInfo);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }

    throw new Exception("Failed to post UserJobInfo");
  }

  [HttpDelete("UserJobInfo/{userId}")]
  public IActionResult DeleteUserJobInfo(int userId)
  {
    UserJobInfo? UserJobInfoDb = _userRepository.GetUserJobInfo(userId);
    if (UserJobInfoDb != null)
    {
      _userRepository.RemoveEntity<UserJobInfo>(UserJobInfoDb);
      if (_userRepository.SaveChanges())
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
    return _userRepository.GetUserSalary(userId);
  }

  [HttpPut("UserSalary")]
  public IActionResult PutUserSalary(UserSalary userSalary)
  {
    UserSalary? userSalaryDb = _userRepository.GetUserSalary(userSalary.UserId);
    if (userSalaryDb != null)
    {
      userSalaryDb.UserId = userSalary.UserId;
      userSalaryDb.Salary = userSalary.Salary;

      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
    }

    throw new Exception("Failed to update UserSalary");
  }

  [HttpPost("UserSalary")]
  public IActionResult PostUserSalary(UserSalary userSalary)
  {
    _userRepository.AddEntity<UserSalary>(userSalary);
    if (_userRepository.SaveChanges())
    {
      return Ok();
    }

    throw new Exception("Failed to post UserSalary");
  }

  [HttpDelete("UserSalary/{userId}")]
  public IActionResult DeleteUserSalary(int userId)
  {
    UserSalary? userSalaryDb = _userRepository.GetUserSalary(userId);
    if (userSalaryDb != null)
    {
      _userRepository.RemoveEntity<UserSalary>(userSalaryDb);
      if (_userRepository.SaveChanges())
      {
        return Ok();
      }
    }

    throw new Exception("Failed to delete UserSalary");
  }
}
