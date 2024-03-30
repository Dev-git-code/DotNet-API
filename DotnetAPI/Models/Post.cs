namespace DotNetAPI.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
        public DateTime PostCreate { get; set; }
        public DateTime PostUpdate { get; set; }
    }
}

// --create clustered index to sort by userId first and then by PostId to get faster retrival
// CREATE CLUSTERED INDEX cix_Posts_UserId_PostId ON TutorialAppSchema.Posts(UserId, PostId)
