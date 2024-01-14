using Blogger.Web.Data;
using Blogger.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;

        public BlogPostRepository(BloggerDbContext bloggerDbContext)
        {
            _bloggerDbContext=bloggerDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _bloggerDbContext.AddAsync(blogPost);
            await _bloggerDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _bloggerDbContext.BlogPosts.Include(b => b.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _bloggerDbContext.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await _bloggerDbContext.BlogPosts.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.Content = blogPost.Content;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Tags = blogPost.Tags;

                await _bloggerDbContext.SaveChangesAsync();                
                return existingBlog;
            }
            return null;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await _bloggerDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                _bloggerDbContext.BlogPosts.Remove(existingBlog);
                await _bloggerDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await _bloggerDbContext
                .BlogPosts
                .Include(b => b.Tags)
                .FirstOrDefaultAsync(b => b.UrlHandle == urlHandle);
        }
    }
}
