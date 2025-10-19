//using BusinessLogic.Services;
//using DataAccess.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Presentation.View_Model;

//namespace Presentation.Controllers {
//    public class CategoryController : Controller {
//        private readonly CategoryService _categoryService;

//        public CategoryController(CategoryService categoryService) {
//            _categoryService = categoryService;
//        }

//        public async Task<IActionResult> Index() {
//            var categories = await _categoryService.GetAllCategoriesAsync();
//            var list = categories.Select(c => new CategoryViewModel {
//                Id = c.CategoryId,
//                Name = c.CategoryName,
//                Description = c.CategoryDescription,
//                ParentId = c.ParentCategoryId,
//                ParentName = c.ParentCategoryName,
//                IsActive = c.IsActive,
//                QuantityArticles = c.QuantityArticles

//            });

//            return View(list);
//        }

//        public async Task<IActionResult> Details(int id) {
//            var category = await _categoryService.GetCategoryByIdAsync(id);
//            if (category == null)
//                return NotFound();
//            return View(category);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(Category category) {
//            if (!ModelState.IsValid)
//                return View(category);

//            await _categoryService.AddCategoryAsync(category);
//            return RedirectToAction(nameof(Index));
//        }

//    }
//}
