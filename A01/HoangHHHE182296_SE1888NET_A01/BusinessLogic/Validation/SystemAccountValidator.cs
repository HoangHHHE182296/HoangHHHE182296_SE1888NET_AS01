using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Requests;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Validation {
    public class SystemAccountValidator {
        private readonly ISystemAccountRepository _systemAccountRepository;
        public SystemAccountValidator(ISystemAccountRepository systemAccountRepository) {
            _systemAccountRepository = systemAccountRepository;
        }

        public void ValidateForLogin(LoginRequest loginRequest) {
            this.ValidateGeneric(loginRequest);
        }

        public void ValidateForCreate(CreateAccountRequest createAccountRequest) {
            this.ValidateGeneric(createAccountRequest);
        }

        // Validate generic
        private void ValidateGeneric<T>(T entity) {
            var context = new ValidationContext(entity, null, null);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(entity, context, results, true);

            foreach (var result in results) {
                foreach (var memberName in result.MemberNames) {
                    throw new ArgumentException($"{result.ErrorMessage}");
                }
            }
        }
    }
}
