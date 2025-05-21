using MessagePack.AspNetCoreMvcFormatter;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Extensions;
using Server.Hubs;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
});

builder.Services.AddControllersWithViews();

builder.Services.AddMvc();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCustomRateLimiters();

builder.Services.AddAppServices();

builder.Services.AddRedisStore();

builder.Services.AddSignalR().AddMessagePackProtocol();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));

builder.Services.AddControllers()
    .AddMvcOptions(options =>
    {
        options.InputFormatters.Insert(0, new MessagePackInputFormatter(MessagePackSerializerOptions.Standard));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGzipRequestDecompression();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();   

app.MapControllers();

app.MapHub<DeviceProvisioningHub>("/devicehub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Captcha}/{action=Index}/{id?}");



app.Run();


