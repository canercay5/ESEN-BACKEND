# 1. Aşama: .NET 8 SDK ile projeyi derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sadece proje dosyalarını kopyalayıp bağımlılıkları yüklüyoruz (Önbellek optimizasyonu)
COPY ["ESEN.API/ESEN.API.csproj", "ESEN.API/"]
COPY ["ESEN.Application/ESEN.Application.csproj", "ESEN.Application/"]
COPY ["ESEN.Domain/ESEN.Domain.csproj", "ESEN.Domain/"]
COPY ["ESEN.Infrastructure/ESEN.Infrastructure.csproj", "ESEN.Infrastructure/"]
RUN dotnet restore "ESEN.API/ESEN.API.csproj"

# Şimdi tüm kodları kopyalayıp Release modunda yayınlıyoruz
COPY . .
WORKDIR "/src/ESEN.API"
RUN dotnet publish "ESEN.API.csproj" -c Release -o /app/publish

# 2. Aşama: Sadece çalıştırma ortamını (Runtime) alarak sunucuyu hafifletme
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render'ın dışarıya açacağı port ayarı
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "ESEN.API.dll"]