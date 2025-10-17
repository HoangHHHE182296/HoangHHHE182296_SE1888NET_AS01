using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Core.Constants.Account;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Presentation.Models;
using Presentation.View_Model;
using System.Data;
using System.Text.Json;

namespace Presentation.Controllers {
    public class AccountController : Controller {
        private readonly SystemAccountService _systemAccountService;

        public AccountController(SystemAccountService systemAccountService) {
            _systemAccountService = systemAccountService;
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel accLog) {
            if (!ModelState.IsValid) {

                return View("Login", accLog);
            }
            try {

                var account = await _systemAccountService.LoginAsync(accLog.Email, accLog.Password);

                HttpContext.Session.SetString("Username", account.AccountName ?? string.Empty);
                HttpContext.Session.SetString("UserRole", ((AccountRole)account.AccountRole.Value).ToString());

                return RedirectToAction("Index", "Home");
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError("Email", ex.Message);
                return View(accLog);
            } catch (UnauthorizedAccessException ex) {
                ModelState.AddModelError("Password", ex.Message);
                return View(accLog);
            } catch (Exception ex) {
                ViewBag.MessageError = ex.Message;
                return View(accLog);
            }
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ListAccount(string? nameOrEmail, int? role) {
            ViewBag.NameOrEmail = nameOrEmail;
            ViewBag.Role = role;

            var accounts = await _systemAccountService.SearchAccountAsync(nameOrEmail, role);

            var list = accounts.Select(a => new AccountViewModel {
                Id = a.AccountId,
                Name = a.AccountName,
                Email = a.AccountEmail,
                Role = ((AccountRole)a.AccountRole.Value).ToString()
            }).ToList();

            return View(list);
        }

        public async Task<IActionResult> GetAccountForm(int actionType, int? accountId) {
            AccountViewModel account;
            if (actionType == AccountModalConst.Create.Value) {
                ViewBag.ModalType = AccountModalConst.Create.Value;
                return PartialView("Modal/CreateUpdateModal");
            } else if (actionType == AccountModalConst.Edit.Value) {
                var tmp = await _systemAccountService.GetAccountByIdAsync(accountId.Value);

                account = new AccountViewModel {
                    Id = tmp.AccountId,
                    Name = tmp.AccountName,
                    Email = tmp.AccountEmail,
                    Role = ((AccountRole)tmp.AccountRole.Value).ToString()
                };

                ViewBag.ModalType = AccountModalConst.Edit.Value;
                return PartialView("Modal/CreateUpdateModal", account);
            } else if (actionType == AccountModalConst.Delete.Value) {
                var tmp = await _systemAccountService.GetAccountByIdAsync(accountId.Value);
                account = new AccountViewModel {
                    Id = tmp.AccountId,
                    Name = tmp.AccountName,
                    Email = tmp.AccountEmail,
                    Role = ((AccountRole)tmp.AccountRole.Value).ToString()
                };
                return PartialView("Modal/DeleteModal", account);
            }

            return PartialView("Modal/ChangePasswordModal");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(int actionType, [FromForm] AccountViewModel account) {
            if (!ModelState.IsValid) {
                return PartialView("Modal/CreateUpdateModal", account);
            }

            try {
                if (actionType == AccountModalConst.Create.Value) {
                    var accountDto = new SystemAccountDTO {
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountRole = (int)(AccountRole)Enum.Parse(typeof(AccountRole), account.Role)
                    };
                    Console.WriteLine(accountDto.AccountRole);
                    await _systemAccountService.AddAccountAsync(accountDto);
                } else if (actionType == AccountModalConst.Edit.Value) {
                    // TODO: handle update logic here
                }

                // Sau khi thêm hoặc cập nhật xong → load lại danh sách account
                var allAccounts = await _systemAccountService.SearchAccountAsync(null, null); // null: tìm kế tiếp();
                var list = allAccounts.Select(x => new AccountViewModel {
                    Id = x.AccountId,
                    Name = x.AccountName,
                    Email = x.AccountEmail,
                    Role = ((AccountRole)x.AccountRole.Value).ToString()
                }).ToList();

                return PartialView("_AccountTablePartial", list);
            } catch (InvalidOperationException ex) {
                // Trường hợp lỗi nghiệp vụ (vd: Email đã tồn tại)
                ModelState.AddModelError("Email", ex.Message);
                Response.StatusCode = 400; // báo lỗi cho AJAX biết
                ViewBag.ModalType = AccountModalConst.Create.Value;
                return PartialView("Modal/CreateUpdateModal", account);
            } catch (Exception ex) {
                // Trường hợp lỗi hệ thống (vd: database, network, etc.)
                return Json(new { success = false, message = ex.Message });
            }

        }

    }
}

