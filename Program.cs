using BasicAPI.Repository;
using BasicAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Version = "v1",
        Title = "Basic API",
        Description = "For reference",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact() { 
            Email = "kailash@cybrain.co.in",
            Name = "Kailash",
        }
    });
});

// Add Services
builder.Services.AddTransient<IFileUploadService, FileUploadService>();

// Add Repository
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // FOR FILE UPLOAD

app.UseAuthorization();

app.MapControllers();

app.Run();
