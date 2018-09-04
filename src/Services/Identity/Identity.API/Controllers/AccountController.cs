using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AnteyaSidOnContainers.Services.Identity.API.Models;
using AnteyaSidOnContainers.Services.Identity.API.ViewModels.AccountViewModels;
using AnteyaSidOnContainers.Services.Identity.API.Services.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer4.Extensions;

namespace AnteyaSidOnContainers.Services.Identity.API.Controllers
{
    /// <summary>
    /// This sample controller implements a typical login/logout/provision workflow for local and external accounts.
    /// The login service encapsulates the interactions with the user data store. This data store is in-memory only and cannot be used for production!
    /// The interaction service provides a way for the UI to communicate with identityserver for validation and context retrieval
    /// </summary>
    public class AccountController : Controller
    {
        //private readonly InMemoryUserLoginService _loginService;
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersistedGrantService _persistedGrantService;

        public AccountController(

            //InMemoryUserLoginService loginService,
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            IPersistedGrantService persistedGrantService,
            IClientStore clientStore,
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _loginService = loginService;
            _persistedGrantService = persistedGrantService;
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // if IdP is passed, then bypass showing the login screen
                return ExternalLogin(context.IdP, returnUrl);
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(model.Email);
                if (await _loginService.ValidateCredentials(user, model.Password))
                {
                    AuthenticationProperties props = null;
                    if (model.RememberMe)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddYears(10)
                        };
                    };

                    await _loginService.SignIn(user);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
            };
        }

        async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Email = model.Email;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            Console.WriteLine("Test 1");
            Console.WriteLine(User.Identity);
            Console.WriteLine(User.Identity.IsAuthenticated);
            if (User.Identity.IsAuthenticated == false)
            {
                // if the user is not authenticated, then just show logged out page
                Console.WriteLine("Test 2");
                Console.WriteLine(logoutId);
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            //Test for Xamarin. 
            Console.WriteLine("Test 3");
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            Console.WriteLine("Test 4");
            if (context?.ShowSignoutPrompt == false)
            {
                Console.WriteLine("Test 5");
                //it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.

            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };
            Console.WriteLine("Test 6");
            Console.WriteLine(logoutId);
            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            Console.WriteLine("Test 1.1");
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            Console.WriteLine("Test 1.2");
            Console.WriteLine(idp);
            var subjectId = HttpContext.User.Identity.GetSubjectId();
            Console.WriteLine("Test 1.3");
            Console.WriteLine(subjectId);

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                Console.WriteLine("Test 1.4");
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    Console.WriteLine("Test 1.5");
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;

                try
                {
                    Console.WriteLine("Test 1.6");
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Test 1.7");
                    _logger.LogCritical(ex.Message);
                }
            }

            Console.WriteLine("Test 1.8");
            // delete authentication cookie
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            Console.WriteLine("Test 1.9");

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);
            Console.WriteLine("Test 1.9.1");

            //// full user logout. Every time new credentials will be required.
            //await _persistedGrantService.RemoveAllGrantsAsync(subjectId, "mvc");


            return Redirect(logout?.PostLogoutRedirectUri);
        }

        public async Task<IActionResult> DeviceLogOut(string redirectUrl)
        {
            // delete authentication cookie
            await HttpContext.SignOutAsync();

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Redirect(redirectUrl);
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            if (returnUrl != null)
            {
                returnUrl = UrlEncoder.Default.Encode(returnUrl);
            }
            returnUrl = "/account/externallogincallback?returnUrl=" + returnUrl;

            // start challenge and roundtrip the return URL
            var props = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                Items = { { "scheme", provider } }
            };
            return new ChallengeResult(provider, props);
        }


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Errors.Count() > 0)
                {
                    AddErrors(result);
                    // If we got this far, something failed, redisplay form
                    return View(model);
                }
            }

            if (returnUrl != null)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    return Redirect(returnUrl);
                }
                else if (ModelState.IsValid)
                {
                    var user = await _loginService.FindByUsername(model.Email);
                    if (await _loginService.ValidateCredentials(user, model.Password))
                    { 
                        await _loginService.SignIn(user);

                        // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                        if (_interaction.IsValidReturnUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    
                    return RedirectToAction("login", "account", new { returnUrl = returnUrl });
                }
                else
                {
                    return View(model);
                }
            }

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult Redirecting()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
