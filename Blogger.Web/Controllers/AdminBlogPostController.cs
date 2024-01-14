using Blogger.Web.Models.Domain;
using Blogger.Web.Models.ViewModel;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogger.Web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository=blogPostRepository;
        }
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPost
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                })
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddBlogPost addBlogPost)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPost.Heading,
                PageTitle = addBlogPost.PageTitle,
                Content = addBlogPost.Content,
                ShortDescription = addBlogPost.ShortDescription,
                FeaturedImageUrl = addBlogPost.FeaturedImageUrl,
                UrlHandle = addBlogPost.UrlHandle,
                PublishedDate = addBlogPost.PublishedDate,
                Author = addBlogPost.Author,
                Visible = addBlogPost.Visible
            };
            // Map Tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var slectedTagId in addBlogPost.SelectedTags)
            {
                var selectedTagIdGuid = Guid.Parse(slectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagIdGuid);
                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }
            // Mapping tags back to domain model
            blogPost.Tags = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);
            TempData["message"] = "Blog post Added Successfully";
            return RedirectToAction("Show");
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
          var blogPost = await _blogPostRepository.GetAllAsync();

            return View(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogPostRepository.GetAsync(id);

            var tagsDomainModel = await _tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                var model = new EditBlogPost
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id.ToString(),
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogPost editBlogPost)
        {           
            var blogPostDomainModel = new BlogPost
            {
                Id=editBlogPost.Id,
                Heading = editBlogPost.Heading,
                PageTitle = editBlogPost.PageTitle,
                Content = editBlogPost.Content,
                ShortDescription = editBlogPost.ShortDescription,
                PublishedDate = editBlogPost.PublishedDate,
                Author = editBlogPost.Author,
                FeaturedImageUrl=editBlogPost.FeaturedImageUrl,
                UrlHandle = editBlogPost.UrlHandle,
                Visible=editBlogPost.Visible,
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPost.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

            var updateedBlog = await _blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updateedBlog != null)
            {
                TempData["message"] = "Blog Post Updated Successfully";
                return RedirectToAction("Show");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EditBlogPost editBlogPost)
        {         
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPost.Id);
            if (deletedBlogPost != null)
            {
                TempData["message"] = "Blog post delete Successfully";
                return RedirectToAction("Show");
            }
            return RedirectToAction("Edit", new { id = editBlogPost.Id });
            
        }
    }
}
