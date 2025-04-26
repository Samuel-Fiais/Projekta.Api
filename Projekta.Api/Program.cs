using Application.Infra;
using Carter;
using Projekta.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAllOrigins",
        configurePolicy: policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddProblemDetails();
builder.Services.AddCarter();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDatabase();
builder.Services.AddSwagger(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseSession();

app.UseSwagger();
app.UseSwaggerUI();

app.MapCarter();
app.MapControllers();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<SessionMiddleware>();

await app.RunAsync();
