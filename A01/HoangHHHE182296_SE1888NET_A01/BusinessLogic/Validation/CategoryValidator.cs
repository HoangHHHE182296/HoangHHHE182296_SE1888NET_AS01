using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation {
    public class CategoryValidator {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryValidator(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        //public void ValidateForCreate(Category category) {
        //    var categoryExists = _categoryRepository.GetCategoryByNameAsync(category.Name).Result;
        //    if (categoryExists != null) {
        //        throw new Exception("Category name already exists.");
        //    }
        //}


    }
}
