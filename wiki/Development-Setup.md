# 開発環境セットアップガイド

## 前提条件

### 必須ソフトウェア

| ソフトウェア | バージョン | 用途 |
|------------|-----------|------|
| .NET SDK | 8.0.408以上 | アプリケーション開発 |
| Visual Studio 2022 | 17.8以上 | IDE（推奨） |
| Visual Studio Code | 最新版 | 代替IDE |
| Docker Desktop | 最新版 | コンテナ環境 |
| Git | 2.30以上 | バージョン管理 |
| PostgreSQL クライアント | 15以上 | DB管理（オプション） |

### 推奨ツール

- **GitHub Desktop** - Git GUI クライアント
- **Postman** - API テスト
- **pgAdmin 4** - PostgreSQL管理（Dockerに含まれる）
- **NSwag Studio** - API クライアント生成

## セットアップ手順

### 1. リポジトリのクローン

```bash
# HTTPSでクローン
git clone https://github.com/YourOrg/KDX_Projects.git

# またはSSHでクローン
git clone git@github.com:YourOrg/KDX_Projects.git

# ディレクトリに移動
cd KDX_Projects
```

### 2. .NET SDKバージョンの確認

```bash
# インストールされているSDKの確認
dotnet --list-sdks

# 必要なバージョンがない場合はインストール
# https://dotnet.microsoft.com/download/dotnet/8.0
```

プロジェクトは`global.json`でSDKバージョンを固定しています：
```json
{
  "sdk": {
    "version": "8.0.408",
    "rollForward": "latestFeature",
    "allowPrerelease": false
  }
}
```

### 3. Docker環境の起動

```bash
# Docker Desktopが起動していることを確認

# コンテナの起動
docker-compose up -d

# 起動確認
docker ps

# 期待される出力:
# CONTAINER ID   IMAGE                   PORTS                    NAMES
# xxxxxxxxxxxx   postgres:15-alpine      0.0.0.0:5432->5432/tcp   kdx_postgres
# xxxxxxxxxxxx   dpage/pgadmin4:latest   0.0.0.0:5050->80/tcp     kdx_pgadmin
```

### 4. データベース接続の確認

#### pgAdmin経由での確認

1. ブラウザで http://localhost:5050 にアクセス
2. ログイン情報:
   - Email: `admin@kdx.local`
   - Password: `admin`
3. サーバー追加:
   - Host: `postgres`
   - Port: `5432`
   - Database: `kdx_database`
   - Username: `kdx_user`
   - Password: `kdx_password`

#### コマンドラインでの確認

```bash
# PostgreSQLコンテナに接続
docker exec -it kdx_postgres psql -U kdx_user -d kdx_database

# 接続確認
\dt

# 終了
\q
```

### 5. NuGetパッケージの復元

```bash
# ソリューションレベルで復元
dotnet restore

# 個別プロジェクトの復元（必要な場合）
dotnet restore KdxDesigner/KdxDesigner.csproj
dotnet restore Kdx.API/Kdx.API.csproj
```

### 6. User Secretsの設定

#### Kdx.API用のUser Secrets

```bash
# Kdx.APIディレクトリに移動
cd Kdx.API

# User Secrets IDの初期化（既に設定済みの場合はスキップ）
dotnet user-secrets init

# 接続文字列の設定
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=kdx_database;Username=kdx_user;Password=kdx_password"

# 設定の確認
dotnet user-secrets list
```

### 7. ビルドと実行

#### ソリューション全体のビルド

```bash
# ルートディレクトリで実行
dotnet build

# 成功時の出力例:
# Build succeeded.
#     0 Warning(s)
#     0 Error(s)
```

#### APIサーバーの起動

```bash
# 新しいターミナルウィンドウで
cd Kdx.API
dotnet run

# 起動確認
# info: Microsoft.Hosting.Lifetime[0]
#       Now listening on: https://localhost:7001
# info: Microsoft.Hosting.Lifetime[0]
#       Now listening on: http://localhost:5000
```

Swagger UIの確認: https://localhost:7001/swagger

#### WPFアプリケーションの起動

```bash
# 新しいターミナルウィンドウで
cd KdxDesigner
dotnet run

# または Visual Studio から F5 でデバッグ実行
```

## Visual Studio設定

### 1. ソリューションを開く

1. Visual Studio 2022を起動
2. 「プロジェクトまたはソリューションを開く」
3. `KDX_Projects.sln`を選択

### 2. スタートアッププロジェクトの設定

1. ソリューションエクスプローラーでソリューションを右クリック
2. 「スタートアッププロジェクトの設定」を選択
3. 「マルチスタートアッププロジェクト」を選択
4. 以下を設定:
   - `Kdx.API`: **開始**
   - `KdxDesigner`: **開始**
   - その他: **なし**

### 3. デバッグ設定

`launchSettings.json`の例（Kdx.API）:
```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## Visual Studio Code設定

### 1. 推奨拡張機能

`.vscode/extensions.json`:
```json
{
  "recommendations": [
    "ms-dotnettools.csharp",
    "ms-dotnettools.csdevkit",
    "ms-azuretools.vscode-docker",
    "ms-vscode.powershell",
    "streetsidesoftware.code-spell-checker",
    "eamodio.gitlens"
  ]
}
```

### 2. タスク設定

`.vscode/tasks.json`:
```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/KDX_Projects.sln"
      ],
      "problemMatcher": "$msCompile"
    },
    {
      "label": "run-api",
      "command": "dotnet",
      "type": "process",
      "args": [
        "run",
        "--project",
        "${workspaceFolder}/Kdx.API/Kdx.API.csproj"
      ]
    },
    {
      "label": "run-designer",
      "command": "dotnet",
      "type": "process",
      "args": [
        "run",
        "--project",
        "${workspaceFolder}/KdxDesigner/KdxDesigner.csproj"
      ]
    }
  ]
}
```

### 3. デバッグ設定

`.vscode/launch.json`:
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Launch API",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/Kdx.API/bin/Debug/net8.0/Kdx.API.dll",
      "args": [],
      "cwd": "${workspaceFolder}/Kdx.API",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
        "uriFormat": "%s/swagger"
      }
    },
    {
      "name": "Launch Designer",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/KdxDesigner/bin/Debug/net8.0-windows/KdxDesigner.dll",
      "args": [],
      "cwd": "${workspaceFolder}/KdxDesigner",
      "stopAtEntry": false
    }
  ],
  "compounds": [
    {
      "name": "Launch All",
      "configurations": ["Launch API", "Launch Designer"]
    }
  ]
}
```

## トラブルシューティング

### よくある問題と解決方法

#### 1. Docker関連

**問題:** Docker コンテナが起動しない
```bash
# 解決方法
docker-compose down -v  # 既存のコンテナとボリュームを削除
docker-compose up -d    # 再起動
```

**問題:** PostgreSQL接続エラー
```bash
# ポート競合の確認
netstat -an | findstr :5432

# 別のプロセスが使用している場合は停止するか、
# docker-compose.ymlでポートを変更
```

#### 2. .NET SDK関連

**問題:** SDKバージョンが見つからない
```bash
# 解決方法1: 指定バージョンをインストール
winget install Microsoft.DotNet.SDK.8

# 解決方法2: global.jsonを編集して利用可能なバージョンを指定
```

#### 3. ビルドエラー

**問題:** NuGetパッケージの復元失敗
```bash
# NuGetキャッシュクリア
dotnet nuget locals all --clear

# 再復元
dotnet restore --force
```

**問題:** 名前空間エラー
```bash
# プロジェクト参照の確認
dotnet list reference

# 必要な参照を追加
dotnet add KdxDesigner/KdxDesigner.csproj reference Kdx.Contracts/Kdx.Contracts.csproj
```

#### 4. 実行時エラー

**問題:** API接続エラー
- Kdx.APIが起動していることを確認
- URLとポート番号を確認（https://localhost:7001）
- ファイアウォール設定を確認

**問題:** Access データベース接続エラー
- OleDbプロバイダーのインストール確認
- Accessファイルのパスと権限を確認

## 開発のベストプラクティス

### 1. ブランチ戦略

```bash
# 機能開発
git checkout -b feature/新機能名

# バグ修正
git checkout -b bugfix/バグ説明

# ホットフィックス
git checkout -b hotfix/緊急修正
```

### 2. コミットメッセージ

```bash
# 良い例
git commit -m "feat: プロセス管理APIを追加"
git commit -m "fix: メモリリークを修正"
git commit -m "docs: READMEを更新"

# 悪い例
git commit -m "更新"
git commit -m "バグ修正"
```

### 3. コードレビュー前のチェック

```bash
# ビルド確認
dotnet build

# テスト実行（テストプロジェクトがある場合）
dotnet test

# コード整形
dotnet format

# 依存関係の確認
dotnet list package --outdated
```

---
*次へ: [データベース移行戦略](Database-Migration.md)*