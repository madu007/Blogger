using Blogger.Web.Data;
using Blogger.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;

        public BlogPostCommentRepository(BloggerDbContext bloggerDbContext)
        {
           _bloggerDbContext=bloggerDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _bloggerDbContext.BlogPostComments.AddAsync(blogPostComment);
            await _bloggerDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId)
        {
            return await _bloggerDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
