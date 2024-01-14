using Blogger.Web.Data;
using Blogger.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;

        public BlogPostLikeRepository(BloggerDbContext bloggerDbContext)
        {
            _bloggerDbContext=bloggerDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _bloggerDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await _bloggerDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikes(Guid blogPostId)
        {
            return await _bloggerDbContext.BlogPostLikes.Where(x => x.BlogPostId ==blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _bloggerDbContext.BlogPostLikes
                .CountAsync(l => l.BlogPostId == blogPostId);
        }
    }
}
