using DotNetEnv;
using Goksel_Chat_BotAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IGeminiService, GeminiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("GuvenliSitePolitikasi",
        policyBuilder =>
        {
            policyBuilder.WithOrigins(
                    "https://www.gokselkaradag.com.tr", 
                    "https://gokselkaradag.com.tr",     
                    "http://127.0.0.1:5500",            
                    "http://localhost:5500"             
                )
                .AllowAnyMethod()   
                .AllowAnyHeader();  
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("GuvenliSitePolitikasi");

app.UseAuthorization();
app.MapControllers();

app.Run();