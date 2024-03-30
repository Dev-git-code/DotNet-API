using DotNetAPI.Models;

namespace DotNetAPI.Data
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

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null)
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                _entityFramework.Remove(entityToRemove);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }
        public User GetSingleUser(int userId)
        {
            User? userDb = _entityFramework.Users
                        .Where(u => u.UserId == userId)
                        .FirstOrDefault();

            if (userDb != null) return userDb;
            throw new Exception("Failed to Get User");
        }

        public UserSalary GetUserSalary(int userId)
        {
            UserSalary? userSalary = _entityFramework.UserSalary
                        .Where(u => u.UserId == userId)
                        .FirstOrDefault();

            if (userSalary != null) return userSalary;
            throw new Exception("Failed to Get userSalary");
        }

        public UserJobInfo GetUserJobInfo(int userId)
        {
            UserJobInfo? UserJobInfo = _entityFramework.UserJobInfo
                        .Where(u => u.UserId == userId)
                        .FirstOrDefault();

            if (UserJobInfo != null) return UserJobInfo;
            throw new Exception("Failed to Get UserJobInfo");
        }

    }
}