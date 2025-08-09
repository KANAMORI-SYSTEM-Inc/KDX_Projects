# Docker構成

## 概要

KDX ProjectsではDockerを使用して、開発環境の統一化とデプロイメントの簡素化を実現しています。PostgreSQLデータベースとpgAdmin管理ツールをコンテナ化することで、環境構築の手間を最小限に抑えています。

## Docker Compose構成

### docker-compose.yml

```yaml
version: '3.8'

services:
  # PostgreSQLデータベース
  postgres:
    image: postgres:15-alpine
    container_name: kdx_postgres
    restart: unless-stopped
    environment:
      POSTGRES_USER: kdx_user
      POSTGRES_PASSWORD: kdx_password
      POSTGRES_DB: kdx_database
      TZ: Asia/Tokyo
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./init:/docker-entrypoint-initdb.d
    networks:
      - kdx_network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U kdx_user -d kdx_database"]
      interval: 10s
      timeout: 5s
      retries: 5

  # pgAdmin (データベース管理ツール)
  pgadmin:
    image: dpage/pgadmin4:latest
    container_name: kdx_pgadmin
    restart: unless-stopped
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@kdx.local
      PGADMIN_DEFAULT_PASSWORD: admin
      PGADMIN_CONFIG_SERVER_MODE: 'False'
      PGADMIN_CONFIG_MASTER_PASSWORD_REQUIRED: 'False'
    ports:
      - "5050:80"
    volumes:
      - pgadmin_data:/var/lib/pgadmin
      - ./pgadmin/servers.json:/pgadmin4/servers.json
    networks:
      - kdx_network
    depends_on:
      postgres:
        condition: service_healthy

# ボリューム定義
volumes:
  postgres_data:
    name: kdx_postgres_data
  pgadmin_data:
    name: kdx_pgadmin_data

# ネットワーク定義
networks:
  kdx_network:
    name: kdx_network
    driver: bridge
```

## 初期化スクリプト

### init/01-create-schema.sql

```sql
-- スキーマ作成
CREATE SCHEMA IF NOT EXISTS kdx;

-- デフォルトスキーマ設定
SET search_path TO kdx, public;

-- 拡張機能の有効化
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- 基本テーブルの作成
CREATE TABLE IF NOT EXISTS kdx.processes (
    id SERIAL PRIMARY KEY,
    plc_id INTEGER NOT NULL,
    process_name VARCHAR(255) NOT NULL,
    category_id INTEGER,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS kdx.process_details (
    id SERIAL PRIMARY KEY,
    process_id INTEGER NOT NULL REFERENCES kdx.processes(id) ON DELETE CASCADE,
    detail_name VARCHAR(255),
    block_number INTEGER,
    start_sensor VARCHAR(100),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- インデックス作成
CREATE INDEX idx_processes_plc_id ON kdx.processes(plc_id);
CREATE INDEX idx_process_details_process_id ON kdx.process_details(process_id);

-- 更新日時自動更新トリガー
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

CREATE TRIGGER update_processes_updated_at BEFORE UPDATE
    ON kdx.processes FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_process_details_updated_at BEFORE UPDATE
    ON kdx.process_details FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
```

### init/02-seed-data.sql

```sql
-- 開発用のシードデータ
INSERT INTO kdx.processes (plc_id, process_name, category_id) VALUES
    (1, 'メインプロセス', 1),
    (1, 'サブプロセスA', 2),
    (1, 'サブプロセスB', 2),
    (2, 'テストプロセス', 3);

INSERT INTO kdx.process_details (process_id, detail_name, block_number, start_sensor) VALUES
    (1, '初期化', 1, 'S001'),
    (1, '処理実行', 2, 'S002'),
    (1, '終了処理', 3, 'S003');
```

## pgAdmin設定

### pgadmin/servers.json

```json
{
  "Servers": {
    "1": {
      "Name": "KDX PostgreSQL",
      "Group": "Development",
      "Host": "postgres",
      "Port": 5432,
      "MaintenanceDB": "kdx_database",
      "Username": "kdx_user",
      "SSLMode": "prefer",
      "PassFile": "/tmp/pgpassfile"
    }
  }
}
```

## 環境変数管理

### .env ファイル（本番環境用）

```env
# データベース設定
POSTGRES_USER=kdx_prod_user
POSTGRES_PASSWORD=StrongPassword123!
POSTGRES_DB=kdx_production
POSTGRES_HOST=postgres
POSTGRES_PORT=5432

# pgAdmin設定
PGADMIN_DEFAULT_EMAIL=admin@company.com
PGADMIN_DEFAULT_PASSWORD=AdminPassword456!

# アプリケーション設定
ASPNETCORE_ENVIRONMENT=Production
API_BASE_URL=https://api.kdx.company.com
```

### docker-compose.override.yml（開発環境用）

```yaml
version: '3.8'

services:
  postgres:
    environment:
      POSTGRES_USER: kdx_dev_user
      POSTGRES_PASSWORD: dev_password
      POSTGRES_DB: kdx_development
    ports:
      - "15432:5432"  # 開発環境用の別ポート
    volumes:
      - ./dev-data:/var/lib/postgresql/data

  pgadmin:
    environment:
      PGADMIN_DEFAULT_EMAIL: dev@local
      PGADMIN_DEFAULT_PASSWORD: dev
    ports:
      - "15050:80"  # 開発環境用の別ポート
```

## Docker操作コマンド

### 基本操作

```bash
# コンテナの起動
docker-compose up -d

# コンテナの停止
docker-compose down

# コンテナの再起動
docker-compose restart

# ログの確認
docker-compose logs -f postgres
docker-compose logs -f pgadmin

# コンテナの状態確認
docker-compose ps
```

### データベース操作

```bash
# PostgreSQLコンテナに接続
docker exec -it kdx_postgres psql -U kdx_user -d kdx_database

# SQLファイルの実行
docker exec -i kdx_postgres psql -U kdx_user -d kdx_database < backup.sql

# データベースのバックアップ
docker exec kdx_postgres pg_dump -U kdx_user kdx_database > backup_$(date +%Y%m%d).sql

# データベースのリストア
docker exec -i kdx_postgres psql -U kdx_user -d kdx_database < backup_20250809.sql
```

### 開発環境のリセット

```bash
# すべてのコンテナとボリュームを削除（注意：データが削除されます）
docker-compose down -v

# イメージも含めて完全にクリーンアップ
docker-compose down -v --rmi all

# 再構築
docker-compose build --no-cache
docker-compose up -d
```

## 本番環境用Dockerfile

### Kdx.API/Dockerfile

```dockerfile
# ビルドステージ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルのコピーとリストア
COPY ["Kdx.API/Kdx.API.csproj", "Kdx.API/"]
COPY ["Kdx.Core/Kdx.Core.csproj", "Kdx.Core/"]
COPY ["Kdx.Infrastructure/Kdx.Infrastructure.csproj", "Kdx.Infrastructure/"]
COPY ["Kdx.Contracts/Kdx.Contracts.csproj", "Kdx.Contracts/"]
RUN dotnet restore "Kdx.API/Kdx.API.csproj"

# ソースコードのコピーとビルド
COPY . .
WORKDIR "/src/Kdx.API"
RUN dotnet build "Kdx.API.csproj" -c Release -o /app/build

# パブリッシュステージ
FROM build AS publish
RUN dotnet publish "Kdx.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 実行ステージ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# 非rootユーザーの作成
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

# ヘルスチェック
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

ENTRYPOINT ["dotnet", "Kdx.API.dll"]
```

### docker-compose.production.yml

```yaml
version: '3.8'

services:
  api:
    build:
      context: .
      dockerfile: Kdx.API/Dockerfile
    container_name: kdx_api
    restart: always
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      ConnectionStrings__DefaultConnection: Host=postgres;Database=kdx_database;Username=kdx_user;Password=${POSTGRES_PASSWORD}
    ports:
      - "80:80"
      - "443:443"
    networks:
      - kdx_network
    depends_on:
      postgres:
        condition: service_healthy
    volumes:
      - ./certs:/https:ro  # SSL証明書

  postgres:
    image: postgres:15-alpine
    container_name: kdx_postgres
    restart: always
    environment:
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
      POSTGRES_DB: ${POSTGRES_DB}
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./backups:/backups
    networks:
      - kdx_network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${POSTGRES_USER} -d ${POSTGRES_DB}"]
      interval: 10s
      timeout: 5s
      retries: 5

  # 本番環境ではpgAdminを含めない（セキュリティ上の理由）

volumes:
  postgres_data:
    external: true

networks:
  kdx_network:
    external: true
```

## モニタリングとログ

### ログ集約設定

```yaml
# docker-compose.monitoring.yml
version: '3.8'

services:
  # Prometheusメトリクス収集
  prometheus:
    image: prom/prometheus:latest
    container_name: kdx_prometheus
    volumes:
      - ./prometheus/prometheus.yml:/etc/prometheus/prometheus.yml
      - prometheus_data:/prometheus
    ports:
      - "9090:9090"
    networks:
      - kdx_network

  # Grafanaダッシュボード
  grafana:
    image: grafana/grafana:latest
    container_name: kdx_grafana
    environment:
      GF_SECURITY_ADMIN_PASSWORD: admin
    volumes:
      - grafana_data:/var/lib/grafana
      - ./grafana/dashboards:/etc/grafana/provisioning/dashboards
    ports:
      - "3000:3000"
    networks:
      - kdx_network

  # ログ収集
  loki:
    image: grafana/loki:latest
    container_name: kdx_loki
    ports:
      - "3100:3100"
    volumes:
      - ./loki/loki-config.yaml:/etc/loki/local-config.yaml
    networks:
      - kdx_network

volumes:
  prometheus_data:
  grafana_data:
```

## バックアップ戦略

### 自動バックアップスクリプト

```bash
#!/bin/bash
# backup.sh

# 設定
BACKUP_DIR="/backups"
CONTAINER_NAME="kdx_postgres"
DB_NAME="kdx_database"
DB_USER="kdx_user"
RETENTION_DAYS=30

# タイムスタンプ
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="${BACKUP_DIR}/backup_${TIMESTAMP}.sql.gz"

# バックアップ実行
echo "Starting backup at ${TIMESTAMP}"
docker exec ${CONTAINER_NAME} pg_dump -U ${DB_USER} ${DB_NAME} | gzip > ${BACKUP_FILE}

# 古いバックアップの削除
find ${BACKUP_DIR} -name "backup_*.sql.gz" -mtime +${RETENTION_DAYS} -delete

echo "Backup completed: ${BACKUP_FILE}"
```

### Cronジョブ設定

```bash
# 毎日午前2時にバックアップ実行
0 2 * * * /path/to/backup.sh >> /var/log/kdx_backup.log 2>&1
```

## トラブルシューティング

### よくある問題と解決方法

#### 1. PostgreSQLコンテナが起動しない

```bash
# ログを確認
docker-compose logs postgres

# ボリュームの権限問題の場合
docker-compose down -v
sudo chown -R 999:999 ./postgres_data
docker-compose up -d
```

#### 2. pgAdminに接続できない

```bash
# コンテナ間通信の確認
docker exec kdx_pgadmin ping postgres

# ネットワークの確認
docker network ls
docker network inspect kdx_network
```

#### 3. ディスク容量不足

```bash
# 不要なイメージとコンテナの削除
docker system prune -a

# ボリュームのクリーンアップ
docker volume prune
```

## セキュリティ考慮事項

1. **本番環境でのpgAdmin無効化**
   - 本番環境ではpgAdminコンテナを含めない
   - 必要な場合はVPN経由でのみアクセス可能にする

2. **環境変数の管理**
   - `.env`ファイルはGitにコミットしない
   - Docker Secretsまたは外部の秘密管理ツールを使用

3. **ネットワーク分離**
   - データベースは内部ネットワークのみに公開
   - APIゲートウェイ経由でのみアクセス

4. **定期的な更新**
   - ベースイメージの定期的な更新
   - セキュリティパッチの適用

---
