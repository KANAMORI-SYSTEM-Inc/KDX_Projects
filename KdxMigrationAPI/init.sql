-- PostgreSQL初期化スクリプト
-- このファイルはDockerコンテナ初回起動時に自動実行されます

-- スキーマ作成（必要に応じて）
-- CREATE SCHEMA IF NOT EXISTS kdx;

-- 拡張機能の有効化（必要に応じて）
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- 初期テーブル作成はEntity Framework Coreのマイグレーションで行うため、
-- ここでは基本的な設定のみ行います

-- デフォルトのタイムゾーン設定
SET timezone = 'Asia/Tokyo';