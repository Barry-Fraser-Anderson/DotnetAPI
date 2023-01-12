using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Models;
using DotnetAPI.Data;

namespace DotnetAPI.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[Controller]")]
  public class PostController : ControllerBase
  {
    private readonly DataContextDapper _dapper;
    public PostController(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
    }

    [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
    public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "None")
    {
      string sql =
        "EXEC TutorialAppSchema.spPosts_Get";

      string parameters = "";
      if (postId != 0)
      {
        parameters += ", @PostId = " + postId;
      }
      if (userId != 0)
      {
        parameters += ", @UserId = " + userId;
      }
      if (searchParam != "None")
      {
        parameters += ", @SearchValue = '" + searchParam + "'";
      }
      if (parameters.Length > 0)
      {
        sql += parameters.Substring(1);
      }

      return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("MyPosts")]
    public IEnumerable GetMyPosts()
    {
      string sql =
        "EXEC TutorialAppSchema.spPosts_Get @UserId = " +
        this.User.FindFirst("userId")?.Value;

      IEnumerable<Post> posts = _dapper.LoadData<Post>(sql);
      return posts;
    }

    [HttpPost("UpsertPost")]
    public IActionResult UpsertPost(Post post)
    {
      string sql =
        "EXEC TutorialAppSchema.spPosts_Upsert" +
        "  @UserId = " + this.User.FindFirst("userId")?.Value +
        ", @PostTitle = '" + post.PostTitle + "'" +
        ", @PostContent = '" + post.PostContent + "'";
      if (post.PostId != 0)
      {
        sql += ", @PostId = " + post.PostId;
      }

      if (_dapper.ExecuteSql(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to upsert Post");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
      string sql =
        "EXEC TutorialAppSchema.spPost_Delete" +
        "  @PostId = " + postId.ToString() +
        ", @UserID = " + this.User.FindFirst("userId")?.Value;

      if (_dapper.ExecuteSql(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to delete Post");
    }
  }
}
