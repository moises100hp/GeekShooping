using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Adicionado
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeekShopping.IdentityServer.Pages.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    // 1. Trocamos TestUserStore pelos serviços do ASP.NET Identity
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;

    [BindProperty]
    public InputModel Input { get; set; } = default!;

    public Index(
        IIdentityServerInteractionService interaction,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _interaction = interaction;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        if (Input.Button != "create")
        {
            if (context != null)
            {
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                return Redirect(Input.ReturnUrl ?? "~/");
            }
            return Redirect("~/");
        }

        // 2. Lógica de criação usando o banco de dados
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                EmailConfirmed = true,
                FirstName = Input.Name, // Ajuste conforme as propriedades do seu ApplicationUser
                LastName = ""
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Adiciona claims básicas se necessário
                // await _userManager.AddClaimsAsync(user, new List<Claim> { ... });

                // Faz o login automático após criar
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (context != null) return Redirect(Input.ReturnUrl ?? "~/");
                if (Url.IsLocalUrl(Input.ReturnUrl)) return Redirect(Input.ReturnUrl);
                return Redirect("~/");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}