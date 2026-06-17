# IotNefes

IoT cihazlardan (ESP32) gelen sensör verilerini toplayan ve son kullanıcılara sunan ASP.NET Core 8.0 Web API projesi.

## Mimari

Proje **Clean Architecture** prensiplerine uygun olarak 4 ana katmandan oluşur:

```
┌─────────────────────────────────────────┐
│         Presentation (WebApi)           │  ← API Controller'lar, Request/Response modelleri
├─────────────────────────────────────────┤
│         Application                     │  ← Servisler, İş mantığı, DTO'lar, Interface'ler
├─────────────────────────────────────────┤
│         Infastructure                   │  ← MongoDB, Repository, DI, Ortak yapılar
├─────────────────────────────────────────┤
│         Domain                          │  ← Entity'ler (saf POCO, bağımlılık yok)
└─────────────────────────────────────────┘
```

### Klasör Yapısı

```
IotNefes/
├── Domain/IotNefes.Domain/              # Entity'ler (BaseEntity, Example, AirTemperature)
├── Application/
│   ├── IotNefes.Abstractions/           # Interface'ler (IGenericService, IExampleService) + DTO'lar
│   └── IotNefes.Application/            # Servis implementasyonları + Mapperly mapper'lar
├── Infastructure/
│   ├── IotNefes.Common/                 # Ortak yapılar (ServiceResponse, PagedResult, PagedRequest)
│   ├── IotNefes.Infastructure/          # Generic interface'ler (IGenericRepository, IEntityMapper, IApiMapper, Auto-DI)
│   └── IotNefes.Persistance/            # MongoDB implementasyonu (GenericRepository, MongoDbContext)
└── Presentation/IotNefes.WebApi/        # API katmanı (Controller, Request/Response, Mapperly API mapper'lar)
```

## Kullanılan Teknolojiler

| Teknoloji | Versiyon | Amaç |
|-----------|----------|------|
| ASP.NET Core | 8.0 | Web API framework |
| MongoDB.Driver | 2.28.0 | Veritabanı |
| Riok.Mapperly | 4.3.1 | Compile-time object mapping (source generator) |
| Swashbuckle | 6.6.2 | Swagger/OpenAPI |

## Design Pattern'ler

### Generic Repository Pattern

Tüm entity'ler için tek bir repository. Yeni entity eklediğinizde ayrı repository dosyası oluşturmanız gerekmez.

- **Interface:** `IGenericRepository<T>` (`Infastructure/Abstraction/`)
- **Implementasyon:** `GenericRepository<T>` (`Persistance/Repository/`)
- **Metotlar:** `GetAllAsync`, `GetPagedAsync`, `GetByIdAsync`, `FindAsync`, `AddAsync`, `AddRangeAsync`, `UpdateAsync`, `DeleteAsync`, `CountAsync`, `AnyAsync`

### Generic Service Pattern

CRUD operasyonlarını tekrar etmemek için abstract base service. Virtual metotlar override edilebilir.

- **Interface:** `IGenericService<TDto>` (`Abstractions/`)
- **Implementasyon:** `GenericService<TDto, TEntity>` (`Application/`)
- **Metotlar:** `GetAllAsync`, `GetPagedAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`

Override örneği (`ExampleService`):
- `CreateAsync` → Aynı isimde kayıt var mı kontrolü
- `DeleteAsync` → Aktif kayıtlarda soft delete

### Generic Controller Pattern

CRUD endpoint'lerini tekrar etmemek için abstract base controller. Virtual endpoint'ler override edilebilir.

- **Base:** `GenericController<TDto, TRequest, TResponse>` (`WebApi/Controllers/`)
- **Endpoint'ler:** `GET /`, `GET /paged`, `GET /{id}`, `POST /`, `PUT /{id}`, `DELETE /{id}`

Override örneği (`ExampleController`):
- `GET /active` → Sadece aktif kayıtları döner

### Compile-Time Mapping (Mapperly)

AutoMapper yerine Mapperly kullanılır. Runtime'da reflection yok, compile-time'da mapping kodu üretilir.

İki tür mapper vardır:

1. **IEntityMapper<TDto, TEntity>** → Service katmanında (Entity ↔ DTO)
2. **IApiMapper<TDto, TRequest, TResponse>** → Controller katmanında (Request → DTO → Response)

### ServiceResponse Pattern

Tüm servisler `ServiceResponse<T>` döner:
```csharp
ServiceResponse<T>.Success(data)
ServiceResponse<T>.Fail("mesaj", "HATA_KODU")
```

### Auto-DI (Marker Interface'ler)

Servisler marker interface ile işaretlenir, DI kaydı otomatik yapılır:

```csharp
public class ExampleService : ..., IScopedService { }     // Scoped
public class CacheService : ..., ISingletonService { }     // Singleton
public class EmailService : ..., ITransientService { }     // Transient
```

`AddServicesByConvention()` assembly'yi tarar ve tüm servisleri otomatik kaydeder.

## Performans Özellikleri

### Response Compression
- **Gzip** (birincil, `CompressionLevel.Fastest`) + **Brotli** (fallback, `Fastest`)
- Düşük CPU kullanımıyla ~%60-70 sıkıştırma

### Rate Limiting
- Fixed Window: dakikada 100 istek (konfigüre edilebilir)
- Aşım durumunda `429 Too Many Requests` + `retryAfterSeconds`

### Output Caching
- GET endpoint'leri 5 dakika cache'lenir (konfigüre edilebilir)
- Write işlemlerinde (POST/PUT/DELETE) cache otomatik invalidate edilir
- Farklı `page` ve `pageSize` kombinasyonları ayrı cache'lenir (VaryByQuery)

### ETag / Conditional Requests
- GET response'larına SHA256 hash bazlı `ETag` header eklenir
- Client `If-None-Match` gönderirse ve veri değişmemişse `304 Not Modified` (boş body)

### Pagination
- `PagedRequest`: `page` ve `pageSize` (10, 50 veya 100'e snap)
- `PagedResult<T>`: `Items`, `Page`, `PageSize`, `TotalCount`, `TotalPages`, `HasPrevious`, `HasNext`
- MongoDB `Skip`/`Limit` + `CountDocumentsAsync` ile verimli sayfalama

## Konfigürasyon

Tüm ayarlar `appsettings.json`'dan yönetilir:

```json
{
  "RateLimiting": {
    "PermitLimit": 100,
    "WindowInSeconds": 60,
    "QueueLimit": 0
  },
  "Caching": {
    "DefaultDurationSeconds": 300
  },
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "IotNefesDb"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  }
}
```

## Yeni Entity Ekleme Rehberi

Yeni bir sensör tipi (örn. `Humidity`) eklemek için sırasıyla:

### 1. Domain Entity
```
Domain/IotNefes.Domain/Humidity/Humidity.cs
```
```csharp
public class Humidity : BaseEntity
{
    public string DeviceId { get; set; } = string.Empty;
    public double Value { get; set; }
}
```

### 2. DTO + Service Interface
```
Application/IotNefes.Abstractions/Humidity/Dto/HumidityDto.cs
Application/IotNefes.Abstractions/Humidity/IHumidityService.cs
```
```csharp
public interface IHumidityService : IGenericService<HumidityDto> { }
```

### 3. Service + Entity Mapper
```
Application/IotNefes.Application/Humidity/HumidityService.cs
Application/IotNefes.Application/Humidity/Mapping/HumidityEntityMapper.cs
```
```csharp
public class HumidityService : GenericService<HumidityDto, HumidityEntity>, IHumidityService, IScopedService { }

[Mapper]
public partial class HumidityEntityMapper : IEntityMapper<HumidityDto, HumidityEntity> { ... }
```

### 4. Controller + Request/Response + API Mapper
```
Presentation/IotNefes.WebApi/src/Controllers/HumidityController.cs
Presentation/IotNefes.WebApi/src/Models/Requests/Humidity/HumidityRequest.cs
Presentation/IotNefes.WebApi/src/Models/Responses/Humidity/HumidityResponse.cs
Presentation/IotNefes.WebApi/src/Mapping/HumidityApiMapper.cs
```

### 5. DI Kaydı (Program.cs)
```csharp
builder.Services.AddSingleton<IEntityMapper<HumidityDto, HumidityEntity>, HumidityEntityMapper>();
builder.Services.AddSingleton<IApiMapper<HumidityDto, HumidityRequest, HumidityResponse>, HumidityApiMapper>();
```

Servis kaydı (`IHumidityService`) otomatik yapılır (`IScopedService` marker interface sayesinde).

## Çalıştırma

```bash
# MongoDB'nin çalıştığından emin olun
dotnet build
dotnet run --project Presentation/IotNefes.WebApi

# Swagger: https://localhost:{port}/swagger
```

## MongoDB Mapping

Domain entity'lerinde MongoDB attribute'ü yoktur. Mapping convention-based olarak `MongoDbContext`'te yapılır:
- **CamelCaseElementNameConvention** → property isimleri camelCase olarak saklanır
- **IgnoreExtraElementsConvention** → DB'deki fazla alanlar hata vermez
- **StringObjectIdGenerator** → `Id` alanı string olarak yönetilir