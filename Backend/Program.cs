using Backend;
using Backend.Data;
using Backend.Models;
using Backend.Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Neo4j.Driver;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Configure Neo4j settings from appsettings.json
builder.Services.Configure<Neo4jSettings>(
    builder.Configuration.GetSection("Neo4jSettings"));

//Register Neo4j client singleton
builder.Services.AddSingleton<IDriver>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<Neo4jSettings>>().Value;
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var driver = GraphDatabase.Driver(settings.Uri, AuthTokens.Basic(settings.UserName, settings.Password));
        logger.LogInformation("Successfully connected to Neo4j at {Uri}", settings.Uri); // Log successful connection
        return driver;
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while trying to connect to Neo4j at {Uri}", settings.Uri);
        throw new InvalidOperationException("Could not connect to Neo4j.", ex);
    }
});

// Configure MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register MongoDB client singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var client = new MongoClient(settings?.ConnectionString);
        logger.LogInformation("Successfully connected to MongoDB at {ConnectionString}",settings?.ConnectionString);
        return client;
    }
    catch (Exception ex)
    {
        logger.LogError(ex,"Error while trying to connect to MongoDB at {ConnectionString}",settings?.ConnectionString);
        throw new InvalidOperationException("Could not connect to MongoDB.", ex);
    }
});

// Configure MongoDB Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
        builder.Configuration["MongoDbSettings:ConnectionString"],
        builder.Configuration["MongoDbSettings:DatabaseName"]);

builder.Services.AddSingleton<AuthContext>();
builder.Services.AddSingleton<IndexService>();

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger
    app.UseSwaggerUI(); // Enable Swagger UI
}else{ 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();