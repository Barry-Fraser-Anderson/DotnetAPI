namespace DotnetAPI.Models
{
  public partial class PostAddDto
  {
    public string PostTitle { get; set; } = "";
    public string PostContent { get; set; } = "";
  }

  public partial class PostEditDto
  {
    public int PostId { get; set; }
    public string PostTitle { get; set; } = "";
    public string PostContent { get; set; } = "";
  }
}
