namespace Blogger.Web.Models.ViewModel
{
    public class BlogComment
    {
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string UserName { get; set; }
    }
}
