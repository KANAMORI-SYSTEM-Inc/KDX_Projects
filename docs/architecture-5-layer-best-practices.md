# 5層アーキテクチャ ベストプラクティス

## アーキテクチャ概要

このプロジェクトでは、クリーンアーキテクチャの原則に基づいた5層アーキテクチャを採用しています。

### 層の構成

```
┌─────────────────────────────────────────┐
│     Presentation Layer (UI/API)         │
│  - WPF Views/ViewModels                 │
│  - Web API Controllers                  │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│     Application Layer                    │
│  - Use Cases                            │
│  - Application Services                 │
│  - DTOs                                 │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│     Domain Layer                        │
│  - Entities                             │
│  - Domain Services                      │
│  - Factories                            │
│  - Interfaces                           │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│     Infrastructure Layer                │
│  - Repositories                         │
│  - External Services                    │
│  - Cache                                │
│  - Configuration                        │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│     Data Layer                          │
│  - Database (Access/PostgreSQL)         │
│  - External APIs                        │
└─────────────────────────────────────────┘
```

## 各層の責務

### 1. Domain層 (Kdx.Core/Domain)

**責務**: ビジネスロジックとビジネスルールの実装

```csharp
// エンティティ
public class MnemonicTimerDevice { }

// ドメインサービスのインターフェース
public interface IDeviceOffsetProvider
{
    int DeviceStartT { get; }
    int TimerStartZR { get; }
}

// ファクトリー
public static class MnemonicTimerDeviceFactory
{
    public static MnemonicTimerDevice Create(
        Timer timer, ProcessDetail detail, 
        int sequenceBase, IDeviceOffsetProvider offsets, int plcId)
    {
        // ビジネスロジック
        var prefix = (timer.TimerCategoryId is 6 or 7) ? "T" : "ST";
        // ...
    }
}
```

### 2. Application層 (Kdx.Core/Application)

**責務**: ユースケースの実装、トランザクション管理

```csharp
public sealed class SaveProcessDetailTimerDevicesUseCase
{
    private readonly IMnemonicTimerDeviceRepository _repo;
    private readonly ITimerDeviceCache _cache;
    private readonly ISequenceGenerator _seq;
    private readonly IDeviceOffsetProvider _offsets;

    public async Task<int> ExecuteAsync(
        IReadOnlyList<Timer> timers,
        IReadOnlyList<ProcessDetail> details,
        int plcId,
        CancellationToken ct = default)
    {
        // ユースケースロジック
        var devices = new List<MnemonicTimerDevice>();
        
        foreach (var detail in details)
        {
            var timer = FindRelatedTimer(timers, detail);
            if (timer is null) continue;
            
            var device = MnemonicTimerDeviceFactory.Create(
                timer, detail, _seq.Next(), _offsets, plcId);
                
            devices.Add(device);
            _cache.AddOrUpdate(device, plcId, timer.CycleId ?? 1);
        }
        
        if (!_useCacheOnly && devices.Count > 0)
        {
            await _repo.AddRangeAsync(devices, ct);
        }
        
        return devices.Count;
    }
}
```

### 3. Infrastructure層 (Kdx.Infrastructure)

**責務**: 外部リソースへのアクセス実装

```csharp
// リポジトリ実装
public sealed class MnemonicTimerDeviceRepository : IMnemonicTimerDeviceRepository
{
    private readonly IAccessRepository _accessRepository;
    
    public async Task AddRangeAsync(
        IEnumerable<MnemonicTimerDevice> devices, 
        CancellationToken ct)
    {
        using var connection = new OleDbConnection(_connectionString);
        await connection.OpenAsync(ct);
        
        using var transaction = connection.BeginTransaction();
        try
        {
            // バルクインサート処理
            foreach (var device in devices)
            {
                await connection.ExecuteAsync(sql, device, transaction);
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}

// キャッシュ実装
public sealed class TimerDeviceCache : ITimerDeviceCache
{
    private readonly IMnemonicDeviceMemoryStore _memoryStore;
    
    public void AddOrUpdate(MnemonicTimerDevice device, int plcId, int cycleId)
        => _memoryStore.AddOrUpdateTimerDevice(device, plcId, cycleId);
}
```

### 4. Presentation層 (KdxDesigner/ViewModels, KdxMigrationAPI/Controllers)

**責務**: ユーザーインターフェース、入力検証

```csharp
// WPF ViewModel
public partial class MainViewModel
{
    private SaveProcessDetailTimerDevicesUseCase? _saveTimerDevicesUseCase;
    
    public async Task SaveTimerDevicesAsync()
    {
        var timer = _repository!.GetTimers();
        var details = _repository.GetProcessDetails();
        
        await _saveTimerDevicesUseCase.ExecuteAsync(
            timer, details, SelectedPlc!.Id);
    }
}

// Web API Controller
[ApiController]
[Route("api/[controller]")]
public class MnemonicTimerDeviceController : ControllerBase
{
    private readonly SaveProcessDetailTimerDevicesUseCase _useCase;
    
    [HttpPost("save-process-detail-timers")]
    public async Task<IActionResult> SaveProcessDetailTimers(
        [FromBody] SaveTimerDevicesRequest request)
    {
        var result = await _useCase.ExecuteAsync(
            request.Timers, request.Details, request.PlcId);
            
        return Ok(new { DeviceCount = result });
    }
}
```

## 依存性注入 (DI) の設定

### WPFアプリケーション側

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKdxCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Options
        services.Configure<DeviceOffsetOptions>(configuration.GetSection("DeviceOffsets"));
        
        // Domain Services
        services.AddSingleton<IDeviceOffsetProvider, DeviceOffsetProvider>();
        services.AddSingleton<ISequenceGenerator, SequenceGenerator>();
        
        // Infrastructure
        services.AddSingleton<ITimerDeviceCache, TimerDeviceCache>();
        services.AddScoped<IMnemonicTimerDeviceRepository, MnemonicTimerDeviceRepository>();
        
        // Application Use Cases
        services.AddScoped<SaveProcessDetailTimerDevicesUseCase>();
        
        return services;
    }
}
```

### Web API側

```csharp
// Program.cs
builder.Services.Configure<DeviceOffsetOptions>(options =>
{
    options.DeviceStartT = builder.Configuration.GetValue<int>("DeviceOffsets:DeviceStartT", 0);
    options.TimerStartZR = builder.Configuration.GetValue<int>("DeviceOffsets:TimerStartZR", 0);
});

builder.Services.AddSingleton<IDeviceOffsetProvider, DeviceOffsetProvider>();
builder.Services.AddScoped<SaveProcessDetailTimerDevicesUseCase>();
```

## ベストプラクティス

### 1. 依存性の方向

- 上位層は下位層に依存する（Presentation → Application → Domain）
- 下位層は上位層を知らない
- インターフェースを使用して依存性を逆転させる

### 2. DTOの使用

- 層間のデータ転送にはDTOを使用
- エンティティを直接公開しない
- Contracts プロジェクトにDTOを配置

### 3. トランザクション管理

- Application層でトランザクション境界を定義
- Repository内でもトランザクションを使用可能
- 分散トランザクションは避ける

### 4. エラーハンドリング

```csharp
public class TimerDeviceService : ITimerDeviceService
{
    public async Task<SaveTimerDevicesResult> SaveProcessDetailTimersAsync(...)
    {
        try
        {
            // ビジネスルールの検証
            var validationErrors = ValidateInput(timers, details);
            if (validationErrors.Any())
            {
                return new SaveTimerDevicesResult
                {
                    Success = false,
                    Errors = validationErrors
                };
            }
            
            // 処理実行
            var deviceCount = await _saveUseCase.ExecuteAsync(...);
            
            return new SaveTimerDevicesResult
            {
                Success = true,
                DeviceCount = deviceCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving timer devices");
            return new SaveTimerDevicesResult
            {
                Success = false,
                Errors = new[] { ex.Message }
            };
        }
    }
}
```

### 5. テスタビリティ

- インターフェースを使用してモック可能にする
- 純粋関数を活用する（ファクトリーメソッドなど）
- 単体テストを層ごとに作成

### 6. 設定管理

```json
// appsettings.json
{
  "DeviceOffsets": {
    "DeviceStartT": 0,
    "TimerStartZR": 0
  },
  "ConnectionStrings": {
    "AccessConnection": "Provider=Microsoft.ACE.OLEDB.12.0;..."
  }
}
```

## マイグレーションパス

既存のコードから段階的に移行する方法：

1. **Phase 1**: 新機能を5層アーキテクチャで実装
2. **Phase 2**: 既存コードをラップして徐々に移行
3. **Phase 3**: レガシーコードを完全に置き換え

```csharp
// 移行期の実装例
public async Task SaveTimerDevicesAsync()
{
    if (_saveTimerDevicesUseCase == null)
    {
        // フォールバック: 既存の同期メソッドを使用
        SaveTimerDevicesLegacy();
        return;
    }
    
    // 新しいユースケースを使用
    await _saveTimerDevicesUseCase.ExecuteAsync(...);
}
```

## まとめ

この5層アーキテクチャにより：

- **保守性**: 各層の責務が明確で変更が容易
- **テスタビリティ**: モックを使用した単体テストが可能
- **拡張性**: 新機能の追加が既存コードに影響しない
- **再利用性**: ビジネスロジックをUIから分離

これらのベストプラクティスに従うことで、長期的に保守可能で拡張性の高いシステムを構築できます。