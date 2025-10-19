using BusinessLogic.DTOs.Requests;
using BusinessLogic.Services;
using Core.Constants.Account;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Presentation.Models;
using Presentation.View_Model;
using Presentation.View_Model.Data;
using Presentation.View_Model.Params;
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
        public async Task<IActionResult> Login([FromForm] LoginParams loginParams) {
            if (!ModelState.IsValid) {
                return View("Login", loginParams);
            }

            try {
                var account = await _systemAccountService.LoginAsync(new LoginRequest { AccountEmail = loginParams.Email, AccountPassword = loginParams.Password });
                HttpContext.Session.SetString("Username", account.AccountName ?? string.Empty);
                HttpContext.Session.SetString("UserRole", ((AccountRole)account.AccountRole.Value).ToString());

                return RedirectToAction("Index", "Home");
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError("Email", ex.Message);
                return View(loginParams);
            } catch (UnauthorizedAccessException ex) {
                ModelState.AddModelError("Password", ex.Message);
                return View(loginParams);
            } catch (Exception ex) {
                ViewBag.MessageError = ex.Message;
                return View(loginParams);
            }
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ListAccount([FromQuery] SearchAccountParams searchAccountParams) {
            ViewBag.Keyword = searchAccountParams.Keyword;
            ViewBag.Role = searchAccountParams.Role;

            var accounts = await _systemAccountService.SearchAccountAsync(searchAccountParams.Keyword, searchAccountParams.Role != null ? (int)(AccountRole)Enum.Parse(typeof(AccountRole), searchAccountParams.Role) : null);

            var list = accounts.Select(a => new AccountViewModel {
                Account = new AccountModel {
                    Id = a.AccountId,
                    Name = a.AccountName,
                    Email = a.AccountEmail,
                    Role = ((AccountRole)a.AccountRole.Value).ToString()
                }
            }).ToList();

            return View(list);
        }


        public async Task<IActionResult> GetAccountForm(int actionType, int? accountId) {
            if (actionType == AccountModalConst.Create.Value) {
                ViewBag.ModalType = AccountModalConst.Create.Value;
                return PartialView("Modal/CreateUpdateModal");
            } else if (actionType == AccountModalConst.Edit.Value) {
                var account = await _systemAccountService.GetAccountByIdAsync(accountId.Value);

                ViewBag.ModalType = AccountModalConst.Edit.Value;
                return PartialView("Modal/CreateUpdateModal", new CreateUpdateAccountParams {
                    Name = account.AccountName,
                    Email = account.AccountEmail,
                    Role = ((AccountRole)account.AccountRole.Value).ToString()
                });

            } else if (actionType == AccountModalConst.Delete.Value) {
                var account = await _systemAccountService.GetAccountByIdAsync(accountId.Value);
                return PartialView("Modal/DeleteModal", account.AccountName);
            }

            return PartialView("Modal/ChangePasswordModal");
        }

        [HttpPost]
        public async Task<IActionResult> CreateUpdate(int actionType, [FromForm] CreateUpdateAccountParams createUpdateAccountParams) {
            if (!ModelState.IsValid) {
                return PartialView("Modal/CreateUpdateModal", createUpdateAccountParams);
            }
            Console.WriteLine(actionType);

            try {

                if (actionType == AccountModalConst.Create.Value) {
                    var createParams = new CreateAccountRequest {
                        AccountName = createUpdateAccountParams.Name,
                        AccountEmail = createUpdateAccountParams.Email,
                        AccountPassword = createUpdateAccountParams.Password,
                        AccountRole = (int)Enum.Parse(typeof(AccountRole), createUpdateAccountParams.Role)
                    };

                    await _systemAccountService.AddAccountAsync(createParams);
                } else if (actionType == AccountModalConst.Edit.Value) {

                    var updateParams = new UpdateAccountRequest {
                        AccountId = createUpdateAccountParams.Id,
                        AccountName = createUpdateAccountParams.Name,
                        AccountEmail = createUpdateAccountParams.Email,
                        AccountRole = (int)Enum.Parse(typeof(AccountRole), createUpdateAccountParams.Role)
                    };

                    await _systemAccountService.UpdateAccountAsync(updateParams);
                }

                var allAccounts = await _systemAccountService.SearchAccountAsync(null, null);
                var list = allAccounts.Select(a => new AccountViewModel {
                    Account = new AccountModel {
                        Id = a.AccountId,
                        Name = a.AccountName,
                        Email = a.AccountEmail,
                        Role = ((AccountRole)a.AccountRole.Value).ToString()
                    }
                }).ToList();

                return PartialView("_AccountTablePartial", list);
            } catch (InvalidOperationException ex) {
                ModelState.AddModelError("Email", ex.Message);
                Response.StatusCode = 400;
                ViewBag.ModalType = actionType;
                return PartialView("Modal/CreateUpdateModal", createUpdateAccountParams);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int accountId) {
            try {
                await _systemAccountService.DeleteAccountAsync(accountId);

                var allAccounts = await _systemAccountService.SearchAccountAsync(null, null);
                var list = allAccounts.Select(a => new AccountViewModel {
                    Account = new AccountModel {
                        Id = a.AccountId,
                        Name = a.AccountName,
                        Email = a.AccountEmail,
                        Role = ((AccountRole)a.AccountRole.Value).ToString()
                    }
                }).ToList();

                return PartialView("_AccountTablePartial", list);
            } catch (Exception ex) {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}

