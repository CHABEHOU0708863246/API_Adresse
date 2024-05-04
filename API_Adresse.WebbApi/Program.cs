using API_Adresse.Services.AdressService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SMART_Pressing.DataAccessLayer.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Configuration de MongoDbSettings à partir du fichier appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.Configure<MongoDbSettings>(
  builder.Configuration.GetSection(nameof(MongoDbSettings))
);

// Register MongoClient as a scoped service
builder.Services.AddScoped<MongoClient>(provider =>
{
    var settings = provider.GetService<IOptions<MongoDbSettings>>();
    return new MongoClient(settings.Value.ConnectionString);
});

// Register IMongoDatabase as a scoped service (assuming you want scoped lifetime)
builder.Services.AddScoped<IMongoDatabase>(provider =>
{
    var client = provider.GetService<MongoClient>();
    var settings = provider.GetService<IOptions<MongoDbSettings>>();
    return client.GetDatabase(settings.Value.DatabaseName);
});

// Keep MongoDbContext registration (assuming singleton lifetime)
builder.Services.AddSingleton<MongoDbContext>();

// Ajoutez IMemoryCache au conteneur de services
builder.Services.AddMemoryCache();


// Ajouter les services au conteneur.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HttpClient
builder.Services.AddHttpClient<IAddressService, AddressService>(client =>
{
    client.BaseAddress = new Uri("https://api-adresse.data.gouv.fr/");
    // Configurer d'autres options HttpClient si nécessaire
});

var app = builder.Build();

// Configurer le pipeline de requête HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
