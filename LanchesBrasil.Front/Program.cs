using Blazored.LocalStorage;
using LanchesBrasil.Commons.Service;
using LanchesBrasil.Commons.Service.Interfaces;
using LanchesBrasil.Front;
using LanchesBrasil.Front.Service;
using LanchesBrasil.Front.Service.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpSendService, HttpSendService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IKeycloakService, KeycloakService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
