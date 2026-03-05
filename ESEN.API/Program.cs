using ESEN.Application.Interfaces;
using ESEN.Application.Services;
using ESEN.Domain.Interfaces;
using ESEN.Infrastructure.Data;
using ESEN.Infrastructure.Repositories;
using ESEN.Infrastructure.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var firebaseKeyPath = Path.Combine(builder.Environment.ContentRootPath, "firebase-key.json");
if (File.Exists(firebaseKeyPath))
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseKeyPath)
    });
}
else
{
    Console.WriteLine("D�KKAT: firebase-key.json dosyas� bulunamad�! Bildirimler �al��mayacak.");
}

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["MongoDbSettings:ConnectionString"];
    var databaseName = configuration["MongoDbSettings:DatabaseName"];

    return new MongoDbContext(connectionString, databaseName);
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<IPushNotificationService, PushNotificationService>();
builder.Services.AddScoped<OutbreakAnalysisService>();
builder.Services.AddHttpClient<IAiPredictionService, AiPredictionService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();