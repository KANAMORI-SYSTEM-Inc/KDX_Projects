-- Cylinderテーブルの null PlcId を修正
-- 1. まず現在の状況を確認
SELECT COUNT(*) as null_count FROM "Cylinder" WHERE "PlcId" IS NULL;

-- 2. NULLのPlcIdを0に更新
UPDATE "Cylinder" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

-- 3. 今後の挿入時にデフォルト値0を設定
ALTER TABLE "Cylinder" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- 4. NOT NULL制約を追加（必要に応じて）
-- ALTER TABLE "Cylinder" 
-- ALTER COLUMN "PlcId" SET NOT NULL;

-- CylinderIOテーブルも同様に修正
UPDATE "CylinderIO" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "CylinderIO" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- OperationIOテーブルも同様に修正
UPDATE "OperationIO" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "OperationIO" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- 他のテーブルでPlcIdがある場合も同様の処理
-- Cycle
UPDATE "Cycle" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "Cycle" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- Memory
UPDATE "Memory" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "Memory" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- MnemonicDevice
UPDATE "MnemonicDevice" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "MnemonicDevice" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- MnemonicSpeedDevice
UPDATE "MnemonicSpeedDevice" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "MnemonicSpeedDevice" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- MnemonicTimerDevice
UPDATE "MnemonicTimerDevice" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "MnemonicTimerDevice" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- ProsTime
UPDATE "ProsTime" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "ProsTime" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- Servo
UPDATE "Servo" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "Servo" 
ALTER COLUMN "PlcId" SET DEFAULT 0;

-- Length
UPDATE "Length" 
SET "PlcId" = 0 
WHERE "PlcId" IS NULL;

ALTER TABLE "Length" 
ALTER COLUMN "PlcId" SET DEFAULT 0;