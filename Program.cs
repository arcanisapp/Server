using Microsoft.EntityFrameworkCore;
using Server.Crypto;
using Server.Data;
using Server.Services;
using Server.Services.Validation;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // например, 10MB
});

builder.Services.AddControllersWithViews();

builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IHCaptchaService, HCaptchaService>();

builder.Services.AddScoped<IProofOfWorkService, ProofOfWorkService>();

builder.Services.AddScoped<IShakeGenerator, ShakeGenerator>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddSingleton<IMlDsaKeyVerifier, MlDsaKeyVerifier>();

builder.Services.AddSingleton<ITimestampValidator>(new TimestampValidator(maxSkewSeconds: 30));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("Content-Encoding", out var encoding) &&
        encoding.ToString().Contains("gzip"))
    {
        context.Request.Body = new GZipStream(context.Request.Body, CompressionMode.Decompress);
    }

    await next();
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Captcha}/{action=Index}/{id?}");



app.Run();


