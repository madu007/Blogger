using Blogger.Web.Models.Domain;
using Blogger.Web.Models.ViewModel;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, 
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRepository=blogPostRepository;
            _blogPostLikeRepository=blogPostLikeRepository;
            _signInManager=signInManager;
            _userManager=userManager;
            _blogPostCommentRepository=blogPostCommentRepository;
        }
        public async Task<IActionResult> Index(string UrlHandle)
        {
            var liked = false;

            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(UrlHandle);

            var blogPostLikesViewModel = new BlogPostLikeViewModel();
            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    // Get like for blog foe this user
                    var likesForBlog = await _blogPostLikeRepository.GetLikes(blogPost.Id);

                    var userId = _userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFormUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFormUser != null;
                    }
                }

                // Get comment for post
                var blogCCommentDomainModel = await _blogPostCommentRepository.GetCommentByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCCommentDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded,
                        UserName = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                    });
                }

                blogPostLikesViewModel = new BlogPostLikeViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    ShortDescription = blogPost.ShortDescription,
                    Heading = blogPost.Heading,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    UrlHandle = blogPost.UrlHandle,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }
            return View(blogPostLikesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(BlogPostLikeViewModel blogPostLikeViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogPostLikeViewModel.Id,
                    Description = blogPostLikeViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now,
                };

                await _blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { UrlHandle = blogPostLikeViewModel.UrlHandle });
            }
            return View();
           
        }
    }
}
