var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddChatGpt()
    .AddI18nForServer("wwwroot/i18n");

builder.Services.AddScoped((_) =>
{
    var message = new HttpClientHandler();
    message.ServerCertificateCustomValidationCallback += (_, _, _, _) => true;
    return new HttpClient(message);
});
var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
