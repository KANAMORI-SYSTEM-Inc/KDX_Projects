-- KDX Database初期化スクリプト
-- PostgreSQL用

-- 拡張機能の有効化
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- スキーマの作成
CREATE SCHEMA IF NOT EXISTS kdx;

-- デフォルトスキーマの設定
SET search_path TO kdx, public;

-- 会社マスタ
CREATE TABLE IF NOT EXISTS companies (
    id SERIAL PRIMARY KEY,
    company_code VARCHAR(20) UNIQUE NOT NULL,
    company_name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 機械マスタ
CREATE TABLE IF NOT EXISTS machines (
    id SERIAL PRIMARY KEY,
    company_id INTEGER REFERENCES companies(id),
    machine_code VARCHAR(50) NOT NULL,
    machine_name VARCHAR(100) NOT NULL,
    machine_type VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(company_id, machine_code)
);

-- PLCマスタ
CREATE TABLE IF NOT EXISTS plcs (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    plc_name VARCHAR(100) NOT NULL,
    plc_type VARCHAR(50),
    ip_address VARCHAR(15),
    port INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- デバイスマスタ
CREATE TABLE IF NOT EXISTS devices (
    id SERIAL PRIMARY KEY,
    plc_id INTEGER REFERENCES plcs(id),
    device_name VARCHAR(100) NOT NULL,
    device_type VARCHAR(50),
    address VARCHAR(50),
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- IO定義テーブル
CREATE TABLE IF NOT EXISTS io (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    io_type VARCHAR(10) CHECK(io_type IN ('INPUT', 'OUTPUT')),
    io_number INTEGER NOT NULL,
    io_name VARCHAR(100),
    device_address VARCHAR(50),
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, io_type, io_number)
);

-- メモリ定義テーブル
CREATE TABLE IF NOT EXISTS memories (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    memory_type VARCHAR(10),
    memory_number INTEGER NOT NULL,
    memory_name VARCHAR(100),
    device_address VARCHAR(50),
    data_type VARCHAR(20),
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, memory_type, memory_number)
);

-- タイマー定義テーブル
CREATE TABLE IF NOT EXISTS timers (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    timer_number INTEGER NOT NULL,
    timer_name VARCHAR(100),
    preset_value INTEGER,
    time_unit VARCHAR(10),
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, timer_number)
);

-- プロセス定義テーブル
CREATE TABLE IF NOT EXISTS processes (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    process_code VARCHAR(50) NOT NULL,
    process_name VARCHAR(100),
    process_type VARCHAR(50),
    sequence_number INTEGER,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, process_code)
);

-- プロセス詳細テーブル
CREATE TABLE IF NOT EXISTS process_details (
    id SERIAL PRIMARY KEY,
    process_id INTEGER REFERENCES processes(id),
    step_number INTEGER NOT NULL,
    action_type VARCHAR(50),
    target_device VARCHAR(100),
    value VARCHAR(100),
    condition TEXT,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(process_id, step_number)
);

-- オペレーション定義テーブル
CREATE TABLE IF NOT EXISTS operations (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    operation_code VARCHAR(50) NOT NULL,
    operation_name VARCHAR(100),
    operation_type VARCHAR(50),
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, operation_code)
);

-- オペレーションIO関連テーブル
CREATE TABLE IF NOT EXISTS operation_io (
    id SERIAL PRIMARY KEY,
    operation_id INTEGER REFERENCES operations(id),
    io_id INTEGER REFERENCES io(id),
    io_role VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(operation_id, io_id)
);

-- シリンダー定義テーブル
CREATE TABLE IF NOT EXISTS cylinders (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    cylinder_code VARCHAR(50) NOT NULL,
    cylinder_name VARCHAR(100),
    cylinder_type VARCHAR(50),
    stroke_length DECIMAL(10,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, cylinder_code)
);

-- シリンダーIO関連テーブル
CREATE TABLE IF NOT EXISTS cylinder_io (
    id SERIAL PRIMARY KEY,
    cylinder_id INTEGER REFERENCES cylinders(id),
    io_id INTEGER REFERENCES io(id),
    io_function VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(cylinder_id, io_id)
);

-- エラー定義テーブル
CREATE TABLE IF NOT EXISTS errors (
    id SERIAL PRIMARY KEY,
    machine_id INTEGER REFERENCES machines(id),
    error_code VARCHAR(50) NOT NULL,
    error_message TEXT,
    error_level VARCHAR(20),
    recovery_action TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(machine_id, error_code)
);

-- 監査ログテーブル
CREATE TABLE IF NOT EXISTS audit_logs (
    id SERIAL PRIMARY KEY,
    table_name VARCHAR(50),
    record_id INTEGER,
    action VARCHAR(20),
    user_name VARCHAR(100),
    change_data JSONB,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 更新日時自動更新用トリガー関数
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- 各テーブルに更新日時トリガーを設定
CREATE TRIGGER update_companies_updated_at BEFORE UPDATE ON companies
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_machines_updated_at BEFORE UPDATE ON machines
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_plcs_updated_at BEFORE UPDATE ON plcs
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_devices_updated_at BEFORE UPDATE ON devices
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_io_updated_at BEFORE UPDATE ON io
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_memories_updated_at BEFORE UPDATE ON memories
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_timers_updated_at BEFORE UPDATE ON timers
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_processes_updated_at BEFORE UPDATE ON processes
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_process_details_updated_at BEFORE UPDATE ON process_details
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_operations_updated_at BEFORE UPDATE ON operations
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_cylinders_updated_at BEFORE UPDATE ON cylinders
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_errors_updated_at BEFORE UPDATE ON errors
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- インデックスの作成
CREATE INDEX idx_machines_company_id ON machines(company_id);
CREATE INDEX idx_plcs_machine_id ON plcs(machine_id);
CREATE INDEX idx_devices_plc_id ON devices(plc_id);
CREATE INDEX idx_io_machine_id ON io(machine_id);
CREATE INDEX idx_memories_machine_id ON memories(machine_id);
CREATE INDEX idx_timers_machine_id ON timers(machine_id);
CREATE INDEX idx_processes_machine_id ON processes(machine_id);
CREATE INDEX idx_process_details_process_id ON process_details(process_id);
CREATE INDEX idx_operations_machine_id ON operations(machine_id);
CREATE INDEX idx_operation_io_operation_id ON operation_io(operation_id);
CREATE INDEX idx_cylinders_machine_id ON cylinders(machine_id);
CREATE INDEX idx_cylinder_io_cylinder_id ON cylinder_io(cylinder_id);
CREATE INDEX idx_errors_machine_id ON errors(machine_id);
CREATE INDEX idx_audit_logs_table_record ON audit_logs(table_name, record_id);
CREATE INDEX idx_audit_logs_created_at ON audit_logs(created_at);

-- サンプルデータの挿入
INSERT INTO companies (company_code, company_name) VALUES
    ('KANAMORI', 'カナモリシステム株式会社')
ON CONFLICT DO NOTHING;

-- 権限の設定
GRANT ALL PRIVILEGES ON SCHEMA kdx TO kdx_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA kdx TO kdx_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA kdx TO kdx_user;