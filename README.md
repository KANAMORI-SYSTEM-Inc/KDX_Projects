# KDX Projects

[![Build and Test](https://github.com/[organization]/KDX_Projects/actions/workflows/build.yml/badge.svg)](https://github.com/[organization]/KDX_Projects/actions/workflows/build.yml)
[![Docker Compose Test](https://github.com/[organization]/KDX_Projects/actions/workflows/docker.yml/badge.svg)](https://github.com/[organization]/KDX_Projects/actions/workflows/docker.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

統合開発環境用のソリューションです。複数のプロジェクトを効率的に管理し、モダンな開発手法に対応します。

## プロジェクト構成

- **KdxDesigner** - WPFベースのデザイナーアプリケーション
- **KdxMigrationAPI** - データ移行用Web API

## 開発環境セットアップ

### 必要条件

- .NET 8.0 SDK
- Visual Studio 2022 または VS Code
- Docker Desktop (ローカルデバッグ用)
- Git

### 初期セットアップ

1. リポジトリのクローン
```bash
git clone [repository-url]
cd KDX_Projects
```

2. NuGetパッケージの復元
```bash
dotnet restore
```

3. ビルド
```bash
dotnet build
```

## Docker環境の起動

### 基本サービスの起動
```bash
docker-compose up -d
```

### 全サービス（開発ツール含む）の起動
```bash
docker-compose --profile tools --profile cache --profile monitoring up -d
```

### サービスの停止
```bash
docker-compose down
```

### データベースを含めた完全な削除
```bash
docker-compose down -v
```

## 開発環境

### Visual Studio
1. `KDX_Projects.sln`を開く
2. 起動プロジェクトを選択
3. F5でデバッグ実行

### VS Code
1. フォルダを開く
2. ターミナルで実行:
```bash
# KdxDesigner
cd KdxDesigner
dotnet run

# KdxMigrationAPI
cd KdxMigrationAPI
dotnet run
```

### Claude AIアシスタント
`CLAUDE.md`ファイルにプロジェクト固有の情報を記載してください。

## データベース

### PostgreSQL接続情報（開発環境）
- Host: localhost
- Port: 5432
- Database: kdx_database
- Username: kdx_user
- Password: kdx_password

### pgAdmin（オプション）
- URL: http://localhost:5050
- Email: admin@kdx.com
- Password: admin

## API エンドポイント

### KdxMigrationAPI
- Base URL: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger

## ログ・監視

### Seq（オプション）
- URL: http://localhost:5341

## プロジェクト追加方法

新しいプロジェクトを追加する場合：

1. プロジェクトフォルダを作成
```bash
dotnet new [template] -n [ProjectName]
```

2. ソリューションに追加
```bash
dotnet sln add [ProjectName]/[ProjectName].csproj
```

3. 必要に応じてdocker-compose.ymlにサービスを追加

## ビルド設定

`Directory.Build.props`で共通設定を管理しています：
- .NET 8.0
- C# 最新バージョン
- Nullable有効
- コード分析有効

## トラブルシューティング

### Docker関連
- Docker Desktopが起動していることを確認
- ポートの競合を確認（5432, 5000, 5050など）

### ビルドエラー
- .NET SDKのバージョンを確認
- NuGetパッケージの復元を再実行

## ライセンス

各プロジェクトのLICENSEファイルを参照してください。