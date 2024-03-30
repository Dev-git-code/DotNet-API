using System.Data;
using System.Diagnostics.Tracing;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.Dtos;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId, int userId, string searchParam = "None")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";

            DynamicParameters sqlParameters = new DynamicParameters();
            string stringParameters = "";

            if (postId != 0)
            {
                stringParameters += ", @PostId = @PostIdParameter";
                sqlParameters.Add("@PostIdParameter", postId, DbType.Int32);
            }

            if (userId != 0)
            {
                stringParameters += ", @UserId = @UserIdParameter";
                sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
            }

            if (searchParam != "None")
            {
                stringParameters += ", @SearchValue = @SearchParam";
                sqlParameters.Add("@SearchParam", searchParam, DbType.String);
            }
            if (stringParameters.Length > 0) sql += stringParameters.Substring(1);

            return _dapper.LoadDataWithParameters<Post>(sql, sqlParameters);
        }


        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string? currentUserId = this.User.FindFirst("userId")?.Value; // this represents our controller here (userId taken out of token)
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId = @UserIdParam";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParam", currentUserId, DbType.Int32);

            return _dapper.LoadDataWithParameters<Post>(sql, sqlParameters);

        }

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(PostDto postToUpsert)
        {
            string? currentUserId = this.User.FindFirst("userId")?.Value;
            string sql = @" EXEC TutorialAppSchema.spPosts_Upsert
                                @UserId = @UserIdParameter , 
                                @PostTitle = @PostTitleParameter , 
                                @PostContent = @PostContentParameter ";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", currentUserId, DbType.Int32);
            sqlParameters.Add("@PostTitleParameter", postToUpsert.PostTitle, DbType.String);
            sqlParameters.Add("@PostContentParameter", postToUpsert.PostContent, DbType.String);


            if (postToUpsert.PostId > 0)
            {
                sql += ", @PostId = " + postToUpsert.PostId;
                sqlParameters.Add("@PostIdParameter", postToUpsert.PostId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
            throw new Exception("Failed to Upsert Post");
        }

        [HttpDelete("Post/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string? currentUserId = this.User.FindFirst("userId")?.Value;
            string sql = "EXEC TutorialAppSchema.spPosts_Delete @PostId = @PostIdParameter , @UserId = @UserIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", currentUserId, DbType.Int32);
            sqlParameters.Add("@PostIdParameter", postId, DbType.Int32);

            if (_dapper.ExecuteSqlWithParameters(sql, sqlParameters)) return Ok();
            throw new Exception("Failed to Delete Post");
        }

    }
}