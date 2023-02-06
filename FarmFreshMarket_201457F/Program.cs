using Castle.Core.Smtp;

using FarmFreshMarket_201457F.Data;
using FarmFreshMarket_201457F.Models;
using FarmFreshMarket_201457F.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Twilio.Clients;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();

// On Hold for OTP
//builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
//builder.Services.AddTransient<ISmsSender, AuthMessageSender>();
//builder.Services.Configure<SMSoptions>(Configuration);

// OPT , Twilio
//builder.Services.AddControllersWithViews();
//builder.Services.AddControllers();
//builder.Services.AddHttpClient<ITwilioRestClient, TwilioClient>();

// Google Login
builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
	.AddCookie()
	.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
    {
		options.ClientId = "4815300734-a65m2otnpr1452j561gvmhpt1qeic6a0.apps.googleusercontent.com";
		options.ClientSecret = "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";
	});

//Log Service
builder.Services.AddScoped<LogService>();

//OTP Service
builder.Services.AddScoped<OTPService>();

//Email Service
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<SMTPConfig>(builder.Configuration.GetSection("SMTPConfig"));

//Reset Password Service
builder.Services.AddScoped<ResetPasswordService>();

//Google ReCaptcha
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = true;

	// options.User.RequireUniqueEmail = true;
	//options.Lockout.AllowedForNewUsers = true;
	//options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
	//options.Lockout.MaxFailedAccessAttempts = 3;

})
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();



// Secure CreaditCardNumber
builder.Services.AddDataProtection();

// Session Management
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(60);
});


builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.Name = "AspNetCore.Identity.Application";
	options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
	options.SlidingExpiration = true;
});

builder.Services.Configure<IdentityOptions>(options =>
{
	// Default Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 12;
	options.Password.RequiredUniqueChars = 1;


	// Lockout settings
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
	options.Lockout.MaxFailedAccessAttempts = 3;
	options.Lockout.AllowedForNewUsers = true;


});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Customed Error Message
//app.UseStatusCodePages(context =>
//{
//	context.HttpContext.Response.ContentType = "text/plain";

//	switch (context.HttpContext.Response.StatusCode)
//	{
//		case 404:
//			context.HttpContext.Response.Redirect("/Error404");
//			break;
//		case 403:
//			context.HttpContext.Response.Redirect("/Error403");
//			break;
//		default:
//			context.HttpContext.Response.Redirect("/Error");
//			break;
//	}

//	return Task.CompletedTask;

//});
app.UseStatusCodePagesWithRedirects("/{0}");



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
//app.UseMiddleware<SessionExpiredMiddleware>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();