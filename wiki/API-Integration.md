# API統合アプローチ

## 概要

KDX ProjectsのAPI統合は、従来の直接データベースアクセスから、RESTful APIを介した疎結合アーキテクチャへの移行を実現します。NSwagによる自動クライアント生成により、型安全性を保ちながら効率的な開発を可能にします。

## アーキテクチャ概要

```
┌─────────────────────────────────────────────────┐
│             KdxDesigner (WPF)                    │
│  ┌──────────────────────────────────────────┐   │
│  │          ViewModels                      │   │
│  │  ┌────────────────────────────────────┐ │   │
│  │  │     NSwag Generated Client         │ │   │
│  │  │      (IKdxApiClient)              │ │   │
│  │  └────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────┬───────────────────────────┘
                      │ HTTP/HTTPS
                      ▼
┌─────────────────────────────────────────────────┐
│              Kdx.API (ASP.NET Core)             │
│  ┌──────────────────────────────────────────┐   │
│  │           Controllers                     │   │
│  │  ┌────────────────────────────────────┐ │   │
│  │  │        Swagger/OpenAPI             │ │   │
│  │  │         Documentation              │ │   │
│  │  └────────────────────────────────────┘ │   │
│  └──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
```

## NSwag設定

### 1. API側の設定 (Kdx.API)

**Program.cs:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Swagger/OpenAPI設定
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "KDX API",
        Version = "v1",
        Description = "KDX Project Management API",
        Contact = new OpenApiContact
        {
            Name = "Kanamori System Inc.",
            Email = "support@kanamori-system.co.jp"
        }
    });
    
    // XMLコメントの有効化
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    // 認証設定（将来実装）
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KDX API V1");
        c.RoutePrefix = "swagger";
    });
}
```

### 2. クライアント側の設定 (KdxDesigner)

**nswag.json:**
```json
{
  "runtime": "Net80",
  "defaultVariables": null,
  "documentGenerator": {
    "fromDocument": {
      "url": "https://localhost:7001/swagger/v1/swagger.json",
      "output": null
    }
  },
  "codeGenerators": {
    "openApiToCSharpClient": {
      "clientBaseClass": null,
      "configurationClass": null,
      "generateClientClasses": true,
      "generateClientInterfaces": true,
      "clientBaseInterface": "IKdxApiClient",
      "injectHttpClient": true,
      "disposeHttpClient": false,
      "protectedMethods": [],
      "generateExceptionClasses": true,
      "exceptionClass": "KdxApiException",
      "wrapDtoExceptions": true,
      "useHttpClientCreationMethod": false,
      "httpClientType": "System.Net.Http.HttpClient",
      "useHttpRequestMessageCreationMethod": false,
      "useBaseUrl": true,
      "generateBaseUrlProperty": true,
      "generateSyncMethods": false,
      "generatePrepareRequestAndProcessResponseAsAsyncMethods": false,
      "exposeJsonSerializerSettings": false,
      "clientClassAccessModifier": "public",
      "typeAccessModifier": "public",
      "generateContractsOutput": false,
      "contractsNamespace": "Kdx.Contracts.DTOs",
      "contractsOutputFilePath": null,
      "parameterDateTimeFormat": "s",
      "parameterDateFormat": "yyyy-MM-dd",
      "generateUpdateJsonSerializerSettingsMethod": true,
      "useRequestAndResponseSerializationSettings": false,
      "serializeTypeInformation": false,
      "queryNullValue": "",
      "className": "KdxApiClient",
      "operationGenerationMode": "MultipleClientsFromOperationId",
      "additionalNamespaceUsages": [
        "Kdx.Contracts.DTOs"
      ],
      "additionalContractNamespaceUsages": [],
      "generateOptionalParameters": false,
      "generateJsonMethods": false,
      "enforceFlagEnums": false,
      "parameterArrayType": "System.Collections.Generic.IEnumerable",
      "parameterDictionaryType": "System.Collections.Generic.IDictionary",
      "responseArrayType": "System.Collections.Generic.IList",
      "responseDictionaryType": "System.Collections.Generic.IDictionary",
      "wrapResponses": false,
      "wrapResponseMethods": [],
      "generateResponseClasses": true,
      "responseClass": "SwaggerResponse",
      "namespace": "KdxDesigner.ApiClients",
      "requiredPropertiesMustBeDefined": true,
      "dateType": "System.DateTime",
      "jsonConverters": null,
      "anyType": "object",
      "dateTimeType": "System.DateTime",
      "timeType": "System.TimeSpan",
      "timeSpanType": "System.TimeSpan",
      "arrayType": "System.Collections.Generic.IList",
      "arrayInstanceType": "System.Collections.Generic.List",
      "dictionaryType": "System.Collections.Generic.IDictionary",
      "dictionaryInstanceType": "System.Collections.Generic.Dictionary",
      "arrayBaseType": "System.Collections.ObjectModel.Collection",
      "dictionaryBaseType": "System.Collections.Generic.Dictionary",
      "classStyle": "Poco",
      "jsonLibrary": "SystemTextJson",
      "generateDefaultValues": true,
      "generateDataAnnotations": true,
      "excludedTypeNames": [],
      "excludedParameterNames": [],
      "handleReferences": false,
      "generateImmutableArrayProperties": false,
      "generateImmutableDictionaryProperties": false,
      "jsonSerializerSettingsTransformationMethod": null,
      "inlineNamedArrays": false,
      "inlineNamedDictionaries": false,
      "inlineNamedTuples": true,
      "inlineNamedAny": false,
      "generateDtoTypes": false,
      "generateOptionalPropertiesAsNullable": false,
      "generateNullableReferenceTypes": true,
      "templateDirectory": null,
      "typeNameGeneratorType": null,
      "propertyNameGeneratorType": null,
      "enumNameGeneratorType": null,
      "serviceHost": null,
      "serviceSchemes": null,
      "output": "ApiClients/KdxApiClient.cs",
      "newLineBehavior": "Auto"
    }
  }
}
```

**クライアント生成コマンド:**
```bash
# NSwag CLIのインストール（初回のみ）
dotnet tool install -g NSwag.ConsoleCore

# クライアント生成
nswag run nswag.json

# またはMSBuildタスクとして自動実行
# KdxDesigner.csprojに追加:
<Target Name="NSwag" AfterTargets="Build">
  <Exec Command="nswag run nswag.json" />
</Target>
```

## API実装パターン

### 1. コントローラー実装

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProcessController : ControllerBase
{
    private readonly IProcessService _processService;
    private readonly ILogger<ProcessController> _logger;
    
    public ProcessController(
        IProcessService processService,
        ILogger<ProcessController> logger)
    {
        _processService = processService;
        _logger = logger;
    }
    
    /// <summary>
    /// すべてのプロセスを取得
    /// </summary>
    /// <param name="plcId">PLC ID（オプション）</param>
    /// <returns>プロセスのリスト</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProcessDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetProcesses(
        [FromQuery] int? plcId = null)
    {
        try
        {
            var processes = await _processService.GetProcessesAsync(plcId);
            return Ok(processes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting processes");
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// 新しいプロセスを作成
    /// </summary>
    /// <param name="request">プロセス作成リクエスト</param>
    /// <returns>作成されたプロセス</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProcessDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProcess(
        [FromBody] CreateProcessRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var process = await _processService.CreateProcessAsync(request);
        return CreatedAtAction(
            nameof(GetProcess), 
            new { id = process.Id }, 
            process);
    }
}
```

### 2. リクエスト/レスポンスモデル

```csharp
// リクエストモデル
public class CreateProcessRequest
{
    [Required]
    public int PlcId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string ProcessName { get; set; }
    
    public int? CategoryId { get; set; }
    
    public List<ProcessDetailRequest> Details { get; set; } = new();
}

// レスポンスモデル
public class ProcessResponse
{
    public int Id { get; set; }
    public int PlcId { get; set; }
    public string ProcessName { get; set; }
    public int? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ProcessDetailResponse> Details { get; set; }
}
```

## WPFクライアント実装

### 1. 依存性注入の設定

```csharp
// App.xaml.cs
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();
        
        var mainWindow = Services.GetRequiredService<MainView>();
        mainWindow.Show();
        
        base.OnStartup(e);
    }
    
    private void ConfigureServices(IServiceCollection services)
    {
        // HttpClient設定
        services.AddHttpClient<IKdxApiClient, KdxApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7001");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            // 開発環境でのSSL証明書エラー回避
            ServerCertificateCustomValidationCallback = 
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
        
        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<ProcessViewModel>();
        
        // Views
        services.AddTransient<MainView>();
    }
}
```

### 2. ViewModelでのAPI呼び出し

```csharp
public partial class ProcessViewModel : ObservableObject
{
    private readonly IKdxApiClient _apiClient;
    private readonly ILogger<ProcessViewModel> _logger;
    
    [ObservableProperty]
    private ObservableCollection<ProcessDto> _processes;
    
    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private string _errorMessage;
    
    public ProcessViewModel(
        IKdxApiClient apiClient,
        ILogger<ProcessViewModel> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
        Processes = new ObservableCollection<ProcessDto>();
    }
    
    [RelayCommand]
    private async Task LoadProcessesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            
            var processes = await _apiClient.GetProcessesAsync();
            
            Processes.Clear();
            foreach (var process in processes)
            {
                Processes.Add(process);
            }
        }
        catch (KdxApiException ex)
        {
            _logger.LogError(ex, "API error loading processes");
            ErrorMessage = $"APIエラー: {ex.Message}";
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error loading processes");
            ErrorMessage = "ネットワークエラーが発生しました";
        }
        finally
        {
            IsLoading = false;
        }
    }
    
    [RelayCommand]
    private async Task CreateProcessAsync()
    {
        try
        {
            var request = new CreateProcessRequest
            {
                PlcId = SelectedPlcId,
                ProcessName = NewProcessName,
                CategoryId = SelectedCategoryId
            };
            
            var created = await _apiClient.CreateProcessAsync(request);
            Processes.Add(created);
            
            // UIをリセット
            NewProcessName = string.Empty;
        }
        catch (KdxApiException ex) when (ex.StatusCode == 400)
        {
            ErrorMessage = "入力データが不正です";
        }
    }
}
```

## エラーハンドリング

### 1. API側のグローバルエラーハンドリング

```csharp
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    
    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (NotFoundException ex)
        {
            await HandleNotFoundException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericException(context, ex);
        }
    }
    
    private async Task HandleValidationException(
        HttpContext context, 
        ValidationException ex)
    {
        context.Response.StatusCode = 400;
        
        var response = new
        {
            error = "Validation Error",
            details = ex.Errors
        };
        
        await context.Response.WriteAsJsonAsync(response);
    }
}
```

### 2. クライアント側のエラーハンドリング

```csharp
public class ApiErrorHandler
{
    public static string GetUserFriendlyMessage(KdxApiException ex)
    {
        return ex.StatusCode switch
        {
            400 => "入力データが不正です",
            401 => "認証が必要です",
            403 => "アクセス権限がありません",
            404 => "データが見つかりません",
            409 => "データの競合が発生しました",
            500 => "サーバーエラーが発生しました",
            _ => "予期しないエラーが発生しました"
        };
    }
}
```

## 認証と認可（将来実装）

### JWT認証の実装計画

```csharp
// API側
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

// クライアント側
public class AuthenticationService
{
    private string _accessToken;
    
    public async Task<bool> LoginAsync(string username, string password)
    {
        var response = await _apiClient.LoginAsync(
            new LoginRequest { Username = username, Password = password });
        
        _accessToken = response.AccessToken;
        
        // HttpClientにトークンを設定
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _accessToken);
        
        return true;
    }
}
```

## パフォーマンス最適化

### 1. レスポンスキャッシング

```csharp
[HttpGet]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
public async Task<IActionResult> GetProcessCategories()
{
    // 頻繁に変更されないデータのキャッシング
}
```

### 2. ページネーション

```csharp
public class PagedRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "Id";
    public bool Descending { get; set; } = false;
}

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}
```

### 3. バッチ処理

```csharp
[HttpPost("batch")]
public async Task<IActionResult> BatchUpdate(
    [FromBody] List<UpdateProcessRequest> requests)
{
    var results = await _processService.BatchUpdateAsync(requests);
    return Ok(results);
}
```

## モニタリングとログ

### 1. API呼び出しのログ

```csharp
public class ApiLoggingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        _logger.LogInformation(
            "API Request: {Method} {Path}", 
            context.Request.Method, 
            context.Request.Path);
        
        await _next(context);
        
        stopwatch.Stop();
        
        _logger.LogInformation(
            "API Response: {StatusCode} in {ElapsedMs}ms",
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
```

### 2. クライアント側のテレメトリ

```csharp
public class TelemetryService
{
    public async Task TrackApiCall(string endpoint, long duration, bool success)
    {
        // Application Insights等へのテレメトリ送信
    }
}
```

---
*次へ: [Docker構成](Docker-Configuration.md)*