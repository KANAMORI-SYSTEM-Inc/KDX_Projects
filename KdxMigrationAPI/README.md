# KdxMigrationAPI

KdxDesignerのAccessデータベースからPostgreSQLへの段階的データ移行用API

## 環境要件

- .NET 8.0
- Docker Desktop (PostgreSQL用)
- Visual Studio 2022
- Microsoft Access Database Engine (ACE OLEDB Provider)

## セットアップ

### 1. Docker環境の起動

```bash
cd KdxMigrationAPI
docker-compose up -d
```

これにより以下が起動します：
- PostgreSQL (localhost:5432)
- pgAdmin (http://localhost:5050)
  - Email: admin@kdx.local
  - Password: admin123

### 2. Accessデータベースパスの設定

`appsettings.Development.json`の`AccessDatabase`接続文字列を実際のAccessファイルパスに更新してください。

### 3. データベースマイグレーション

初回起動時は以下のコマンドでマイグレーションを作成：

```bash
dotnet ef migrations add InitialCreate
```

### 4. APIの起動

Visual Studioから実行するか、コマンドラインで：

```bash
dotnet run
```

Swagger UIは https://localhost:[port]/swagger でアクセス可能

## API エンドポイント

### Migration Controller

- `GET /api/migration/test-connection` - Access DB接続テスト
- `POST /api/migration/migrate-io?clearExisting=false` - IOデータ移行実行
- `GET /api/migration/io-statistics` - IO統計情報取得

### IO Controller

- `GET /api/io` - 全IOデータ取得
- `GET /api/io/{address}/{plcId}` - 特定IO取得
- `POST /api/io` - IO作成
- `PUT /api/io/{address}/{plcId}` - IO更新
- `DELETE /api/io/{address}/{plcId}` - IO削除
- `GET /api/io/by-type/{ioType}` - タイプ別IO取得
- `GET /api/io/enabled` - 有効IOのみ取得

## データ移行手順

1. Docker環境を起動
2. APIを起動
3. Access DB接続をテスト: `/api/migration/test-connection`
4. IOデータを移行: `/api/migration/migrate-io`
5. 統計情報で確認: `/api/migration/io-statistics`

## 開発メモ

- 複合主キー（Address + PlcId）を使用
- Entity Framework Core + Npgsql使用
- Dapper使用でAccess DBからの読み込み
- 開発環境ではCORS全許可設定

## トラブルシューティング

### Access接続エラー
- Microsoft Access Database Engine がインストールされているか確認
- 32bit/64bitの違いに注意
- Accessファイルパスが正しいか確認

### PostgreSQL接続エラー
- Dockerコンテナが起動しているか確認
- ポート5432が他のアプリで使用されていないか確認