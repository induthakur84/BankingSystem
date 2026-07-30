using Account.Data.Automapper;
using Account.Data.Context;
using Account.Data.Services;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using ProjectCommonCode;
using ProjectCommonCode.RegisterDependency;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AccountDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserMapping));




builder.Services.RegisterServices(typeof(UserService).Assembly.FullName);


//


//builder.Services.AddScoped<IUserInterface, UserService>();

builder.Services.AddControllers();



// here we add and configure api versioing service

builder.Services.AddApiVersioning(options =>
{

    //Default version to use when client does specify one
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),//api/v1/user, //api/v1/user

        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version")
    );
})
.AddApiExplorer(options =>
{

    //Format of api versioing (e.g 'v1', 'v3'
    options.GroupNameFormat = "'v'VVV";

    options.SubstituteApiVersionInUrl = true;


});
//here we can register the swagger configuration options helper we 
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
