using System.Collections;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("Posts")]
    public IEnumerable<Post> GetPosts()
    {
      string sql =
        "SELECT PostId UserId, PostTitle, PostContent, PostCreated ,PostUpdated" +
        " FROM TutorialAppSchema.Posts";

      return _dapper.LoadData<Post>(sql);
    }

    [HttpGet("PostSingle/{postId}")]
    public Post PostSingle(int postId)
    {
      string sql =
        "SELECT PostId, UserId, PostTitle, PostContent, PostCreated, PostUpdated" +
        " FROM TutorialAppSchema.Posts" +
        " WHERE PostId = " + postId.ToString();

      Post post = _dapper.LoadDataSingle<Post>(sql);
      if (post != null) return post;

      throw new Exception("Failed to get Post");
    }

    [HttpGet("PostsByUser/{userId}")]
    public IEnumerable PostsByUser(int userId)
    {
      string sql =
        "SELECT PostId, UserId, PostTitle, PostContent, PostCreated, PostUpdated" +
        " FROM TutorialAppSchema.Posts" +
        " WHERE UserId = " + userId.ToString();

      IEnumerable<Post> posts = _dapper.LoadData<Post>(sql);
      return posts;
    }

    [HttpGet("MyPosts")]
    public IEnumerable GetMyPosts()
    {
      string sql =
        "SELECT PostId, UserId, PostTitle, PostContent, PostCreated, PostUpdated" +
        " FROM TutorialAppSchema.Posts" +
        " WHERE UserId = " + this.User.FindFirst("userId")?.Value;

      IEnumerable<Post> posts = _dapper.LoadData<Post>(sql);
      return posts;
    }

    [HttpPost("Post")]
    public IActionResult AddPost(PostAddDto postAdd)
    {
      string sql =
        "INSERT INTO TutorialAppSchema.Posts" +
        " (UserId, PostTitle, PostContent, PostCreated, PostUpdated) VALUES ('" +
        this.User.FindFirst("userId")?.Value + "'," +
        $" '{postAdd.PostTitle}', '{postAdd.PostContent}', GETDATE(), GETDATE() )";

      if (_dapper.ExecuteSql(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to add Post");
    }

    [HttpPut("Post")]
    public IActionResult EditPost(PostEditDto postEdit)
    {
      string sql =
        "UPDATE TutorialAppSchema.Posts SET" +
        " PostTitle = '" + postEdit.PostTitle + "'" +
        ", PostContent = '" + postEdit.PostContent + "'" +
        ", PostUpdate = GETDATE()" +
        " WHERE PostId = " + postEdit.PostId.ToString() +
        " AND UserId = " + this.User.FindFirst("userId")?.Value;

      if (_dapper.ExecuteSql(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to edit Post");
    }

    [HttpDelete("Post/{postId}")]
    public IActionResult DeletePost(int postId)
    {
      string sql =
        "DELETE FROM TutorialAppSchema.Posts" +
        " WHERE PostId = " + postId.ToString();

      if (_dapper.ExecuteSql(sql))
      {
        return Ok();
      }

      throw new Exception("Failed to delete Post");
    }
  }
}
