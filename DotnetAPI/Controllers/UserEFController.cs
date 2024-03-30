using AutoMapper;
using DotNetAPI.Data;
using DotNetAPI.Dtos;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

[Authorize]
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
            cfg.CreateMap<UserToAddDto, User>();
            cfg.CreateMap<UserSalary, UserSalary>();
            cfg.CreateMap<UserJobInfo, UserJobInfo>();

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

            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Update User");
        }

        throw new Exception("Failed to Get User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(User user)
    {
        User userDb = _mapper.Map<User>(user);

        _userRepository.AddEntity(userDb);
        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Add User");

    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Delete User");
        }

        throw new Exception("Failed to Get User");

    }

    [HttpGet("GetUserSalary/{userId}")]
    public UserSalary GetUserSalary(int userId)
    {
        return _userRepository.GetUserSalary(userId);
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEF(UserSalary userForUpdate)
    {
        UserSalary? userToUpdate = _userRepository.GetUserSalary(userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Edit User Salary on Save");
        }
        throw new Exception("Failed to find UserSalary for Update");
    }

    [HttpPost("PostUserSalary")]
    public IActionResult PostUserSalaryEF(UserSalary userSalary)
    {
        _userRepository.AddEntity(userSalary);
        if (_userRepository.SaveChanges()) return Ok();
        throw new Exception("Failed to add user Salary");
    }

    [HttpDelete("DeletUserSalary")]
    public IActionResult DeleteUserSalaryEF(int userId)
    {
        UserSalary? userDb = _userRepository.GetUserSalary(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Delete UserSalary");
        }

        throw new Exception("Failed to Get UserSalary");
    }

    [HttpGet("GetUserJobInfo/{userId}")]
    public UserJobInfo GetUserJobInfo(int userId)
    {
        return _userRepository.GetUserJobInfo(userId);
    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEF(UserJobInfo userForUpdate)
    {
        UserJobInfo? userToUpdate = _userRepository.GetUserJobInfo(userForUpdate.UserId);

        if (userToUpdate != null)
        {
            _mapper.Map(userForUpdate, userToUpdate);
            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Edit User Salary on Save");
        }
        throw new Exception("Failed to find UserJobInfo for Update");
    }

    [HttpPost("PostUserJobInfo")]
    public IActionResult PostUserJobInfoEF(UserJobInfo userJobInfo)
    {
        _userRepository.AddEntity(userJobInfo);
        if (_userRepository.SaveChanges()) return Ok();
        throw new Exception("Failed to add user Salary");
    }

    [HttpDelete("DeletUserJobInfo")]
    public IActionResult DeleteUserJobInfoEF(int userId)
    {
        UserJobInfo? userDb = _userRepository.GetUserJobInfo(userId);

        if (userDb != null)
        {
            _userRepository.RemoveEntity(userDb);
            if (_userRepository.SaveChanges()) return Ok();
            throw new Exception("Failed to Delete UserJobInfo");
        }

        throw new Exception("Failed to Get UserJobInfo");
    }

}