using Blogger.Web.Data;
using Blogger.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BloggerDbContext _bloggerDbContext;
        public TagRepository(BloggerDbContext bloggerDbContext)
        {
            _bloggerDbContext = bloggerDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _bloggerDbContext.Tags.AddAsync(tag);
            await _bloggerDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await _bloggerDbContext.Tags.FindAsync(id);
            if (existingTag != null)
            {
                _bloggerDbContext.Tags.Remove(existingTag);
                await _bloggerDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _bloggerDbContext.Tags.ToListAsync();
        }

        public Task<Tag?> GetAsync(Guid id)
        {
           return _bloggerDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _bloggerDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplyName = tag.DisplyName;

                await _bloggerDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }
    }
}
