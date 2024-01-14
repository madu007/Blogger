using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogger.Web.Models.ViewModel
{
    public class EditBlogPost
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string Author { get; set; }
        public string Visible { get; set; }


        public IEnumerable<SelectListItem> Tags { get; set; }
        public string[] SelectedTags { get; set; } = Array.Empty<string>();
    }
}
