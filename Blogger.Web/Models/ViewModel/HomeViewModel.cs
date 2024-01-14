using Blogger.Web.Models.Domain;

namespace Blogger.Web.Models.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<BlogPost> BlopPosts { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
