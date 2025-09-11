-- CompanyテーブルのRLSポリシーを設定
-- 全ユーザーに読み取り権限を付与

-- 既存のポリシーを削除（存在する場合）
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."Company";

-- 読み取り専用ポリシーを作成
CREATE POLICY "Enable read access for all users" 
ON "public"."Company"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- 他のテーブルにも同様のポリシーを適用する場合は以下を実行

-- Modelテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."Model";
CREATE POLICY "Enable read access for all users" 
ON "public"."Model"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- PLCテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."PLC";
CREATE POLICY "Enable read access for all users" 
ON "public"."PLC"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- Cycleテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."Cycle";
CREATE POLICY "Enable read access for all users" 
ON "public"."Cycle"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- Processテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."Process";
CREATE POLICY "Enable read access for all users" 
ON "public"."Process"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- ProcessDetailテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."ProcessDetail";
CREATE POLICY "Enable read access for all users" 
ON "public"."ProcessDetail"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- Operationテーブル
DROP POLICY IF EXISTS "Enable read access for all users" ON "public"."Operation";
CREATE POLICY "Enable read access for all users" 
ON "public"."Operation"
AS PERMISSIVE 
FOR SELECT
TO public
USING (true);

-- 注意: 本番環境では、より厳密なポリシーを設定することを推奨します
-- 例: 認証されたユーザーのみアクセス可能
-- CREATE POLICY "Enable read access for authenticated users only" 
-- ON "public"."Company"
-- AS PERMISSIVE 
-- FOR SELECT
-- TO authenticated
-- USING (true);