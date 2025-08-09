# Docker環境セットアップ手順

## 前提条件
- Docker Desktop for Windowsがインストールされていること
- WSL2が有効になっていること（推奨）

## 1. Docker起動確認
```powershell
# PowerShellで実行
docker --version
docker compose version
```

## 2. PostgreSQLとpgAdminの起動

```powershell
# プロジェクトルートディレクトリで実行
cd C:\Users\shimada.KANAMORI-SYSTEM\source\repos\KDX_Projects

# コンテナ起動
docker compose up -d

# 起動確認
docker compose ps

# ログ確認
docker compose logs postgres
```

## 3. データベース接続確認

### pgAdmin経由での確認
1. ブラウザで http://localhost:5050 にアクセス
2. ログイン情報:
   - Email: admin@kdx.com
   - Password: admin
3. サーバー追加:
   - Host: postgres (コンテナ名)
   - Port: 5432
   - Database: kdx_database
   - Username: kdx_user
   - Password: kdx_password

### psqlコマンドでの確認
```powershell
docker compose exec postgres psql -U kdx_user -d kdx_database
```

## 4. Entity Framework Migration実行

```powershell
# API プロジェクトディレクトリへ移動
cd KdxMigrationAPI

# Migrationツールインストール（初回のみ）
dotnet tool install --global dotnet-ef

# Migration作成
dotnet ef migrations add InitialCreate -o Data/Migrations

# データベース更新
dotnet ef database update
```

## 5. トラブルシューティング

### Dockerが起動しない場合
1. Docker Desktopが起動していることを確認
2. Windows機能で以下が有効になっていることを確認:
   - Hyper-V
   - Windows Subsystem for Linux
   - 仮想マシン プラットフォーム

### PostgreSQL接続エラーの場合
1. ファイアウォール設定を確認
2. ポート5432が他のアプリケーションで使用されていないか確認
   ```powershell
   netstat -an | findstr :5432
   ```

### コンテナの再起動
```powershell
# 停止
docker compose down

# データも含めて削除（注意）
docker compose down -v

# 再起動
docker compose up -d
```

## 6. 開発時の運用

### 起動（毎回）
```powershell
docker compose up -d
```

### 停止（作業終了時）
```powershell
docker compose stop
```

### 完全削除（クリーンアップ）
```powershell
docker compose down -v
```

## 接続情報まとめ

### PostgreSQL
- Host: localhost
- Port: 5432
- Database: kdx_database
- Username: kdx_user
- Password: kdx_password
- Connection String: `Host=localhost;Database=kdx_database;Username=kdx_user;Password=kdx_password`

### pgAdmin
- URL: http://localhost:5050
- Email: admin@kdx.com
- Password: admin