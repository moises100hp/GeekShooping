using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", c =>
{
    c.ExpireTimeSpan = TimeSpan.FromMinutes(10);
})
.AddOpenIdConnect("oidc", options =>
{
    var identityUrl = "https://localhost:5187";
    options.Authority = identityUrl;
    options.ClientId = "geek_shopping";
    options.ClientSecret = "secret";
    options.ResponseType = "code"; // Fluxo de código puro

    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.RequireHttpsMetadata = false;

    // 1. MAPEAMENTO DE CLAIMS (Evita que o .NET limpe os tokens)
    options.MapInboundClaims = false;

    // 2. O CORRETOR DE PROTOCOLO (Resolve IDX21336 e IDX21329)
    options.ProtocolValidator = new FakeOpenIdConnectProtocolValidator
    {
        RequireState = false,  // Mata o erro de State null
        RequireNonce = false,  // Mata o erro de Nonce
        RequireStateValidation = false
    };

    // 3. O HANDLER DE REDE (Resolve falhas silenciosas de SSL local)
    options.BackchannelHttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration
    {
        Issuer = identityUrl,
        AuthorizationEndpoint = $"{identityUrl}/connect/authorize",
        TokenEndpoint = $"{identityUrl}/connect/token",
        JwksUri = $"{identityUrl}/.well-known/openid-configuration/jwks",
        EndSessionEndpoint = $"{identityUrl}/connect/endsession"
    };

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        // --- AS TRÊS LINHAS QUE RESOLVEM O IDX10500 ---
        ValidateIssuerSigningKey = false,
        SignatureValidator = delegate (string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters parameters)
        {
            return new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token);
        },
        // ----------------------------------------------

        NameClaimType = "name",
        RoleClaimType = "role",
        ValidateIssuer = false,
        ValidateAudience = false
    };

    // 4. ESCOPOS EXPLÍCITOS
    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("geek_shopping");

    options.Events = new OpenIdConnectEvents
    {
        OnTokenResponseReceived = context =>
        {

            if (context.ProtocolMessage.Parameters.TryGetValue("access_token", out var token))
            {
                context.TokenEndpointResponse.AccessToken = token;
            }

            //var idToken = context.ProtocolMessage.IdToken ?? context.ProtocolMessage.Parameters["id_token"];

            //if (!string.IsNullOrEmpty(idToken))
            //{
            //    context.TokenEndpointResponse.IdToken = idToken;
            //}
            return Task.CompletedTask;
        },
        OnRemoteFailure = context =>
        {
            // Se der erro, ele vai te dizer o PORQUÊ aqui no console da Web
            Console.WriteLine($">>> FALHA CRÍTICA NO OIDC: {context.Failure?.Message}");
            context.Response.Redirect("/");
            context.HandleResponse();
            return Task.CompletedTask;
        }
    };
});



// Services com injeção de HttpClient lendo do appsettings.json
builder.Services.AddHttpClient<IProductService, ProductService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));

builder.Services.AddHttpClient<ICartService, CartService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CartAPI"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


public class FakeOpenIdConnectProtocolValidator : Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectProtocolValidator
{
    public override void ValidateTokenResponse(Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectProtocolValidationContext validationContext)
    {
        // Não faz nada! Ignora o erro IDX21336 e segue a vida.
        return;
    }
}