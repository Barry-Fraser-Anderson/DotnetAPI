namespace DotnetAPI.Data
{
  public class UserRepository : IUserRepository
  {
    DataContextEF _entityFramework;

    public UserRepository(IConfiguration config)
    {
      _entityFramework = new DataContextEF(config);
    }

    public bool SaveChanges()
    {
      return _entityFramework.SaveChanges() > 0;
    }

    public void AddEntity<T>(T entity)
    {
      if (entity != null) _entityFramework.Add(entity);
    }

    public void RemoveEntity<T>(T entity)
    {
      if (entity != null) _entityFramework.Remove(entity);
    }

    public IEnumerable<User> GetUsers()
    {
      return _entityFramework.Users.ToList<User>();
    }

    public User GetSingleUser(int userId)
    {
      User? user = _entityFramework.Users
        .Where(u => u.UserId == userId).FirstOrDefault<User>();

      if (user != null) return user;

      throw new Exception("Failed to get User");
    }

    public UserJobInfo GetUserJobInfo(int userId)
    {
      UserJobInfo? userJobInfo = _entityFramework.UserJobInfo
        .Where(u => u.UserId == userId).FirstOrDefault<UserJobInfo>();

      if (userJobInfo != null) return userJobInfo;

      throw new Exception("Failed to get UserJobInfo");
    }

    public UserSalary GetUserSalary(int userId)
    {
      UserSalary? userSalary = _entityFramework.UserSalary
        .Where(u => u.UserId == userId).FirstOrDefault<UserSalary>();

      if (userSalary != null) return userSalary;

      throw new Exception("Failed to get UserSalary");
    }
  }
}
