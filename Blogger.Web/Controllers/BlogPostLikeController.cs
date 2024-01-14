using Blogger.Web.Models.Domain;
using Blogger.Web.Models.ViewModel;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            _blogPostLikeRepository=blogPostLikeRepository;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("AddLike")]
        public async Task<IActionResult> AddLike([FromBody] Addlike addlike)
        {
            var model = new BlogPostLike
            {
                BlogPostId = addlike.BlogPostId,
                UserId = addlike.UserId,
            };
            await _blogPostLikeRepository.AddLikeForBlog(model);

            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromBody] Guid blogPostId)
        {
            var totalLike = await _blogPostLikeRepository.GetTotalLikes(blogPostId);
            return Ok(totalLike);
        }
    }
}
