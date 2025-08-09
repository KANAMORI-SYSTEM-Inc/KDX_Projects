# KDX Projects 開発環境整備チェックリスト

## 今日〜明日の実施タスク

### 1. プロジェクト構成の再構築
- [ ] KDX_Projects.sln に5プロジェクト体制を構築
  - [ ] Kdx.Designer (WPF) - 既存のKdxDesignerを改名
  - [ ] Kdx.API (Web API) - 既存のKdxMigrationAPIを改名・拡張
  - [ ] Kdx.Contracts (Class Library) - 共通DTO/Enum
  - [ ] Kdx.Core (Class Library) - ビジネスロジック層
  - [ ] Kdx.Infrastructure (Class Library) - データアクセス層

### 2. 共通設定ファイルの配置
- [ ] global.json作成 - .NETバージョン固定
  ```json
  {
    "sdk": {
      "version": "8.0.100",
      "rollForward": "latestPatch"
    }
  }
  ```
- [ ] Directory.Build.props更新 - プロジェクト間の共通設定

### 3. Kdx.Contracts プロジェクト
- [ ] 新規作成: `dotnet new classlib -n Kdx.Contracts`
- [ ] 既存プロジェクトから移動:
  - [ ] DTOクラス（データ転送オブジェクト）
  - [ ] Enum定義
  - [ ] 共通インターフェース
- [ ] WPFとAPIの両方から参照設定

### 4. Docker環境の整備
- [ ] docker-compose.yml を最小構成に変更
  ```yaml
  services:
    postgres:
      image: postgres:15
      environment:
        POSTGRES_DB: kdx_db
        POSTGRES_USER: kdx_user
        POSTGRES_PASSWORD: kdx_pass
      ports:
        - "5432:5432"
    
    pgadmin:
      image: dpage/pgadmin4
      environment:
        PGADMIN_DEFAULT_EMAIL: admin@kdx.com
        PGADMIN_DEFAULT_PASSWORD: admin
      ports:
        - "5050:80"
  ```
- [ ] `docker-compose up -d` で起動確認

### 5. API プロジェクトの設定
- [ ] User Secretsの有効化
  ```bash
  cd Kdx.API
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=kdx_db;Username=kdx_user;Password=kdx_pass"
  ```
- [ ] Entity Framework Core設定
  ```bash
  dotnet add package Microsoft.EntityFrameworkCore.Design
  dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
  ```
- [ ] 初期Migrationの作成
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

### 6. API クライアント自動生成（NSwag）
- [ ] Kdx.API に NSwag.AspNetCore を追加
  ```bash
  dotnet add package NSwag.AspNetCore
  ```
- [ ] Program.cs で Swagger/OpenAPI設定
- [ ] nswag.json 設定ファイル作成
- [ ] クライアント生成スクリプト作成
  ```bash
  nswag run nswag.json
  ```

### 7. WPF プロジェクトの改修
- [ ] HttpClient / Refit でAPI通信層を実装
- [ ] 既存のデータアクセス層をAPI呼び出しに置き換え
- [ ] ViewModelの調整（非同期処理対応）

## 実行順序

### Day 1（今日）
1. ✅ 基本構成ファイル作成（完了済み）
2. プロジェクト構成の再構築（1〜3）
3. Docker環境起動（4）
4. API基本設定（5の前半）

### Day 2（明日）
1. EF Core Migration（5の後半）
2. NSwag設定（6）
3. WPF改修開始（7）

## コマンドまとめ

```bash
# プロジェクト作成
dotnet new classlib -n Kdx.Contracts -o Kdx.Contracts
dotnet new classlib -n Kdx.Core -o Kdx.Core
dotnet new classlib -n Kdx.Infrastructure -o Kdx.Infrastructure

# ソリューションに追加
dotnet sln add Kdx.Contracts/Kdx.Contracts.csproj
dotnet sln add Kdx.Core/Kdx.Core.csproj
dotnet sln add Kdx.Infrastructure/Kdx.Infrastructure.csproj

# プロジェクト参照追加
cd Kdx.API
dotnet add reference ../Kdx.Contracts/Kdx.Contracts.csproj
dotnet add reference ../Kdx.Core/Kdx.Core.csproj
dotnet add reference ../Kdx.Infrastructure/Kdx.Infrastructure.csproj

cd ../Kdx.Designer
dotnet add reference ../Kdx.Contracts/Kdx.Contracts.csproj

# Docker起動
docker-compose up -d

# Migration
cd Kdx.API
dotnet ef migrations add InitialCreate -o Data/Migrations
dotnet ef database update
```

## 注意事項
- 既存コードの移行は段階的に実施
- テスト環境で動作確認後、本番環境へ反映
- Gitでのブランチ管理を徹底（feature/restructure-projects）