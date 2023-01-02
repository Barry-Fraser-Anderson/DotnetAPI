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

  [HttpGet("TestConnection")]
  public DateTime TestConnection()
  {
    return _dapper.LoadSingle<DateTime>("SELECT GETDATE()");
  }

  [HttpGet("GetUsers")]
  //public IEnumerable<User> GetUsers()
  public string[] GetUsers()
  {
    return new string[] { "user1", "user2", "user3" };
    // return Enumerable.Range(1, 5).Select(index => new User
    // {
    //   Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //   TemperatureC = Random.Shared.Next(-20, 55),
    //   Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    // })
    // .ToArray();
  }
}
