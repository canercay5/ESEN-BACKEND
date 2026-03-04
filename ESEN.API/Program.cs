using ESEN.Application.Interfaces;
using ESEN.Application.Services;
using ESEN.Domain.Interfaces;
using ESEN.Infrastructure.Data;
using ESEN.Infrastructure.Repositories;
using ESEN.Infrastructure.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

// --- 1. CONTROLLER VE SWAGGER AYARLARI ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- 2. MONGODB BAÐLANTI AYARLARI (Singleton) ---
// appsettings.json'dan MongoDbSettings kýsmýný okuyoruz.
// MongoDB istemcisi (MongoClient) thread-safe olduðu için Singleton olarak kaydedilmesi best practice'tir.
builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDbSettings:ConnectionString"];
    var databaseName = configuration["MongoDbSettings:DatabaseName"];

    return new MongoDbContext(connectionString, databaseName);
});

// --- 3. REPOSITORY (Depo) AYARLARI (Scoped) ---
// Generic bir yapýmýz olduðu için typeof() kullanýyoruz.
// Her HTTP isteðinde (Request) yeni bir nesne oluþturulmasý için Scoped seçiyoruz.
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

// --- 4. DIÞ SERVÝSLER (Application & Infrastructure) ---
// Push Notification mock servisi
builder.Services.AddScoped<IPushNotificationService, PushNotificationService>();

// Ana iþ mantýðýmýz olan Use Case servisi
builder.Services.AddScoped<OutbreakAnalysisService>();

// --- 5. HTTP CLIENT AYARLARI ---
// AiPredictionService içinde HttpClient kullanýyoruz. IHttpClientFactory deseniyle 
// HttpClient nesnelerinin yönetimini .NET'in kendisine býrakýyoruz. Bu bellek sýzýntýlarýný önler.
builder.Services.AddHttpClient<IAiPredictionService, AiPredictionService>();

var app = builder.Build();

// --- HTTP REQUEST PIPELINE (Ara Katmanlar) ---
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();