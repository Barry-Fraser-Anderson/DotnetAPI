using System.Data;
using DotnetAPI.Data;
using DotnetAPI.Models;
using Dapper;

namespace DotnetAPI.Helpers
{
  public class ReusableSQL
  {
    private readonly DataContextDapper _dapper;
    public ReusableSQL(IConfiguration config)
    {
      _dapper = new DataContextDapper(config);
    }

    public bool UpsertUser(UserComplete user)
    {
      string sql =
        "EXEC TutorialAppSchema.spUser_Upsert" +
        " @FirstName= @FirstNameParameter" +
        ", @LastName = @LastNameParameter" +
        ", @Email = @EmailParameter" +
        ", @Gender = @GenderParameter" +
        ", @JobTitle = @JobTitleParameter" +
        ", @Department = @DepartmentParameter" +
        ", @Salary = @SalaryParameter" +
        ", @UserId = @UserIdParameter";

      var sqlParameters = new DynamicParameters();

      sqlParameters.Add("@FirstNameParameter", user.FirstName, DbType.String);
      sqlParameters.Add("@LastNameParameter", user.LastName, DbType.String);
      sqlParameters.Add("@EmailParameter", user.Email, DbType.String);
      sqlParameters.Add("@GenderParameter", user.Gender, DbType.String);
      sqlParameters.Add("@ActiveParameter", user.Active, DbType.Boolean);
      sqlParameters.Add("@JobTitleParameter", user.JobTitle, DbType.String);
      sqlParameters.Add("@DepartmentParameter", user.Department, DbType.String);
      sqlParameters.Add("@SalaryParameter", user.Salary, DbType.Int32);
      sqlParameters.Add("@UserIdParameter", user.UserId, DbType.Int32);

      return _dapper.ExecuteSqlWithParameters(sql, sqlParameters);
    }
  }
}
