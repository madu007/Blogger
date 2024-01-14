using Blogger.Web.Data;
using Blogger.Web.Models.Domain;
using Blogger.Web.Models.ViewModel;
using Blogger.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogger.Web.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
           _tagRepository=tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddTags addTags)
        {
            ValidateAddTags(addTags);
            if (ModelState.IsValid == false)
            {
                return View(addTags);
            }
            var tag = new Tag
            {
                Name = addTags.Name,
                DisplyName = addTags.DisplayName
            };

            await _tagRepository.AddAsync(tag);
            TempData["message"] = "Tags Added Successfully";
            return RedirectToAction("Show");
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            var tags = await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTags = new EditTags
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplyName
                };
                return View(editTags);
            }
            return View(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTags editTags)
        {            
            var tag = new Tag 
            { 
                Id = editTags.Id,
                Name = editTags.Name, 
                DisplyName = editTags.DisplayName 
            };

            var updateTag = await _tagRepository.UpdateAsync(tag);

            if (updateTag != null)
            {
               
            }
            else
            {

            }
            TempData["message"] = "Tags Updated Successfully";
            return RedirectToAction("Show", new { id = editTags.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EditTags editTags)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTags.Id);
            if (deletedTag != null)
            {
                TempData["message"] = "Tags Deleted Successfully";
                return RedirectToAction("Show");
            }
            TempData["message"] = "Tags Deleted Successfully";
            return RedirectToAction("Edit", new { id = editTags.Id });
        }

        private void ValidateAddTags(AddTags addTags)
        {
            if (addTags.Name != null && addTags.DisplayName != null)
            {
                if (addTags.Name == addTags.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Name cannot be the sane as displayName");
                }
            }
        }
    }
}
