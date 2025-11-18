using Backend;
using Backend.Controllers;
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
    if (string.IsNullOrEmpty(settings.Uri))
    {
        throw new InvalidOperationException("Neo4j connection string is not configured.");
    }
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
    if (string.IsNullOrEmpty(settings?.ConnectionString))
    {
        throw new InvalidOperationException("MongoDB connection string is not configured.");
    }
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var client = new MongoClient(settings.ConnectionString);
        logger.LogInformation("Successfully connected to MongoDB at {ConnectionString}",settings.ConnectionString);
        return client;
    }
    catch (Exception ex)
    {
        logger.LogError(ex,"Error while trying to connect to MongoDB at {ConnectionString}",settings.ConnectionString);
        throw new InvalidOperationException("Could not connect to MongoDB.", ex);
    }
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var client = sp.GetRequiredService<IMongoClient>();
        logger.LogInformation("Successfully added singleton IMongoDatabase");
        return client.GetDatabase(settings?.DatabaseName);
    }
    catch (Exception ex)
    {
        logger.LogError(ex,"Error while trying to add singleton IMongoDatabase");
        throw new InvalidOperationException("Could not add singleton IMongoDatabase", ex);
    }
});

builder.Services.AddScoped<IMongoCollection<ApplicationUser>>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();
    try
    {
        var database = sp.GetRequiredService<IMongoDatabase>();
        var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
        logger.LogInformation("Successfully connected to MongoDB at {Database}", settings.DatabaseName);
        return database.GetCollection<ApplicationUser>(settings.UserCollectionName);
    }
    catch (Exception ex)
    {
        logger.LogError(ex,"Could not add scoped IMongoCollection<User>");
        throw new InvalidOperationException("Could not add scoped IMongoCollection<User>",ex);
    }
});

// Configure MongoDB Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(
        builder.Configuration["MongoDbSettings:ConnectionString"],
        builder.Configuration["MongoDbSettings:DatabaseName"]);

builder.Services.AddSingleton<AuthContext>();
builder.Services.AddSingleton<NoteContext>();
builder.Services.AddSingleton<IndexService>();

builder.Services.AddScoped<NotesController>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<Neo4jService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var uri = configuration["Neo4j:Uri"];
    var username = configuration["Neo4j:UserName"];
    var password = configuration["Neo4j:Password"];
    var logger = provider.GetRequiredService<ILogger<Neo4jService>>();
    return new Neo4jService(uri, username, password, logger);
});

// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started successfully.");

app.Lifetime.ApplicationStopping.Register(() =>
{
    var driver = app.Services.GetRequiredService<IDriver>();
    driver.DisposeAsync().GetAwaiter().GetResult();}
    );

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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();