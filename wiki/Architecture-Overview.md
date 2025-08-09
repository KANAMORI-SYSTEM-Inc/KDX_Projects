# アーキテクチャ概要

## 設計思想

KDX Projectsは、**クリーンアーキテクチャ**の原則に基づいて設計されています。この設計により、以下の利点を実現しています：

- **関心の分離** - 各層が明確な責任を持つ
- **依存関係の逆転** - ビジネスロジックが外部技術に依存しない
- **テスタビリティ** - 各層を独立してテスト可能
- **保守性** - 変更の影響範囲を最小化

## アーキテクチャ図

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│                      (KdxDesigner)                          │
│                    WPF Application                          │
└────────────────────────┬────────────────────────────────────┘
                         │ HTTP/REST
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                        API Layer                            │
│                       (Kdx.API)                            │
│                   ASP.NET Core Web API                      │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                    Contracts Layer                          │
│                   (Kdx.Contracts)                          │
│                  DTOs, Interfaces                          │
└─────────────────────────────────────────────────────────────┘
                         ▲
                         │
┌────────────────────────┴────────────────────────────────────┐
│                      Core Layer                             │
│                     (Kdx.Core)                             │
│              Business Logic, Domain Models                  │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                 Infrastructure Layer                        │
│                (Kdx.Infrastructure)                        │
│         Data Access, External Services                      │
└─────────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                     Data Storage                            │
│              PostgreSQL (Docker Container)                  │
│                  Legacy Access DB                           │
└─────────────────────────────────────────────────────────────┘
```

## 層の責任

### 1. Presentation Layer (KdxDesigner)

**責任:**
- ユーザーインターフェースの提供
- ユーザー入力の処理
- データの表示とフォーマット

**技術スタック:**
- WPF (Windows Presentation Foundation)
- MVVM Pattern with CommunityToolkit.Mvvm
- NSwag生成APIクライアント

**主要コンポーネント:**
```
KdxDesigner/
├── Views/           # XAMLビュー
├── ViewModels/      # ビューモデル
├── Services/        # UIサービス（現在はレガシー）
└── ApiClients/      # NSwag生成クライアント
```

### 2. API Layer (Kdx.API)

**責任:**
- RESTful APIエンドポイントの提供
- HTTPリクエスト/レスポンスの処理
- 認証・認可（将来実装）
- リクエストバリデーション

**技術スタック:**
- ASP.NET Core 8.0
- Swashbuckle (OpenAPI/Swagger)
- Entity Framework Core

**主要エンドポイント:**
```csharp
// プロセス管理
GET    /api/process
POST   /api/process
PUT    /api/process/{id}
DELETE /api/process/{id}

// デバイス管理
GET    /api/device
POST   /api/device/sync

// メモリ管理
GET    /api/memory
POST   /api/memory/batch
```

### 3. Contracts Layer (Kdx.Contracts)

**責任:**
- プロジェクト間で共有されるデータ構造の定義
- APIコントラクトの明確化
- 型安全性の保証

**主要コンポーネント:**
```
Kdx.Contracts/
├── DTOs/            # Data Transfer Objects
│   ├── Process.cs
│   ├── ProcessDetail.cs
│   ├── Device.cs
│   ├── Memory.cs
│   └── ...（27個のテーブルモデル）
├── Enums/           # 共有列挙型
└── Interfaces/      # 共有インターフェース
```

**DTOの例:**
```csharp
namespace Kdx.Contracts.DTOs
{
    public class Process
    {
        public int Id { get; set; }
        public int PlcId { get; set; }
        public string ProcessName { get; set; }
        public int? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
```

### 4. Core Layer (Kdx.Core)

**責任:**
- ビジネスロジックの実装
- ドメインモデルの定義
- ビジネスルールの適用

**設計原則:**
- 外部依存を持たない
- Pure C# コード
- ユニットテスト可能

**主要コンポーネント:**
```
Kdx.Core/
├── Domain/          # ドメインモデル
├── Services/        # ビジネスサービス
├── Validators/      # ビジネスルール検証
└── Specifications/  # ビジネス仕様
```

### 5. Infrastructure Layer (Kdx.Infrastructure)

**責任:**
- データベースアクセス
- 外部サービス連携
- 永続化の実装

**技術スタック:**
- Entity Framework Core
- PostgreSQL Provider
- Dapper (レガシー互換用)

**主要コンポーネント:**
```
Kdx.Infrastructure/
├── Data/
│   ├── KdxDbContext.cs      # EF Core DbContext
│   ├── Configurations/       # エンティティ設定
│   └── Migrations/          # データベース移行
├── Repositories/            # リポジトリ実装
└── Services/               # 外部サービス実装
```

## 依存関係ルール

```
KdxDesigner → Kdx.Contracts
     ↓
Kdx.API → Kdx.Contracts, Kdx.Core, Kdx.Infrastructure
     
Kdx.Core → Kdx.Contracts

Kdx.Infrastructure → Kdx.Contracts, Kdx.Core
```

**重要な原則:**
1. 依存関係は常に内側に向かう
2. Core層は外部技術に依存しない
3. Contracts層はすべての層から参照可能

## データフロー

### 読み取り操作の例

```
1. User → KdxDesigner (UI操作)
2. KdxDesigner → API Client (NSwag生成)
3. API Client → HTTP Request → Kdx.API
4. Kdx.API → Service (Kdx.Core)
5. Service → Repository (Kdx.Infrastructure)
6. Repository → Database
7. Database → Repository (データ取得)
8. Repository → Service (ドメインモデル)
9. Service → Kdx.API (DTO変換)
10. Kdx.API → HTTP Response → API Client
11. API Client → KdxDesigner (ViewModelに反映)
12. KdxDesigner → User (画面表示)
```

## 設計上の決定事項

### 1. DTOとドメインモデルの分離

- **DTO (Contracts層)**: API通信用の軽量オブジェクト
- **ドメインモデル (Core層)**: ビジネスロジックを含む豊富なモデル

### 2. リポジトリパターンの採用

```csharp
public interface IProcessRepository
{
    Task<Process> GetByIdAsync(int id);
    Task<IEnumerable<Process>> GetAllAsync();
    Task<int> CreateAsync(Process process);
    Task UpdateAsync(Process process);
    Task DeleteAsync(int id);
}
```

### 3. 非同期処理の標準化

すべてのI/O操作は非同期で実装：
```csharp
public async Task<IActionResult> GetProcesses()
{
    var processes = await _processService.GetAllAsync();
    return Ok(processes);
}
```

## セキュリティ考慮事項

1. **接続文字列の管理**
   - User Secrets (開発環境)
   - 環境変数 (本番環境)

2. **APIセキュリティ** (今後実装)
   - JWT認証
   - CORS設定
   - Rate Limiting

3. **データ検証**
   - 入力検証
   - SQLインジェクション対策
   - XSS対策

## パフォーマンス最適化

1. **データベースアクセス**
   - 遅延読み込みの適切な使用
   - クエリ最適化
   - 接続プーリング

2. **キャッシュ戦略**
   - メモリキャッシュ
   - 分散キャッシュ（将来）

3. **非同期処理**
   - async/awaitパターン
   - 並列処理の活用

## 拡張性

### 水平スケーリング対応
- ステートレスなAPI設計
- データベース接続の抽象化
- 外部サービスの疎結合

### マイクロサービス化への準備
- 明確な境界づけられたコンテキスト
- イベント駆動アーキテクチャへの移行パス
- サービス間通信の抽象化

---
*次へ: [プロジェクト構成](Project-Structure.md)*