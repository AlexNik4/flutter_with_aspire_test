using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCors("AllowSpecificOrigins");

app.MapGet("/get-env-variable", async context =>
{
	var envVariable = app.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "Environment variable not set";
	var hostname = app.Configuration["CUSTOM_VARIABLE"] ?? "Environment variable not set";
	var realHostname = app.Environment.IsDevelopment() ? "localhost" : "apiservice";
	var response = new { envVariable, hostname, realHostname };
	context.Response.ContentType = "application/json";
	await context.Response.WriteAsync(JsonSerializer.Serialize(response));
});

app.MapFallbackToFile("/index.html");

app.Run();
