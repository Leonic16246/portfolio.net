var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var url = builder.Configuration["Supabase:Url"];
var key = builder.Configuration["Supabase:Key"];

var supabase = new Supabase.Client(url, key);
await supabase.InitializeAsync();

builder.Services.AddSingleton(supabase);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
