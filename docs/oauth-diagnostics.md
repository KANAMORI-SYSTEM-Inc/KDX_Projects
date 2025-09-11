# GitHub OAuth設定診断ガイド

## 現在のエラー状況

**エラー**: `server_error: unexpected_failure`  
**詳細**: `Error getting user profile from external provider`

これは、SupabaseがGitHubからユーザープロファイル情報を取得できないことを示しています。

## 診断チェックリスト

### 1. Supabase Dashboard確認

**URL**: https://app.supabase.com/project/eebsrvkpcjsqmuvfqtve

#### Authentication → Providers → GitHub
- [ ] **Enabled**: ON になっている
- [ ] **Client ID**: 正しい値が入力されている
- [ ] **Client Secret**: 正しい値が入力されている（余分なスペースなし）

#### Authentication → URL Configuration
- [ ] **Site URL**: `http://localhost:3000`
- [ ] **Redirect URLs**:
  - `http://localhost:3000`
  - `http://localhost:3000/`
  - `http://localhost:3000/auth/callback`

### 2. GitHub OAuth App確認

**URL**: https://github.com/settings/developers

#### OAuth App設定
- [ ] **Application name**: 任意の名前
- [ ] **Homepage URL**: `http://localhost:3000`
- [ ] **Authorization callback URL**: `https://eebsrvkpcjsqmuvfqtve.supabase.co/auth/v1/callback`

### 3. GitHubアカウント設定確認

**URL**: https://github.com/settings/emails

#### メール公開設定
- [ ] **Keep my email addresses private**: チェックを外す
- [ ] **Public email**: メールアドレスを選択
- [ ] 少なくとも1つのメールアドレスが認証済み

### 4. GitHubアカウント権限確認

**URL**: https://github.com/settings/profile

#### プロファイル設定
- [ ] **Public profile**: 表示可能になっている
- [ ] **Name**: 設定されている
- [ ] **Public email**: 設定されている（または「Don't show my email address」以外）

## 一般的な解決方法

### 方法1: Client Secretの再生成

1. GitHub Developer Settings → OAuth Apps → 該当アプリ
2. **Generate a new client secret**をクリック
3. 新しいSecretをコピー（余分な文字なし）
4. Supabase Dashboard → GitHub Provider → Client Secretに貼り付け
5. **Save**をクリック

### 方法2: メール公開設定の変更

1. GitHub Settings → Emails
2. **Keep my email addresses private**のチェックを外す
3. **Public email**でメールアドレスを選択
4. 変更を保存

### 方法3: OAuth Appの再作成

1. GitHub Developer Settings で新しいOAuth Appを作成
2. 設定:
   ```
   Application name: KDX Designer Dev
   Homepage URL: http://localhost:3000
   Authorization callback URL: https://eebsrvkpcjsqmuvfqtve.supabase.co/auth/v1/callback
   ```
3. 新しいClient ID/Secretを使用

## 開発モード（一時的解決策）

OAuth設定が解決されるまで、アプリケーションのログイン画面で**「開発モードでサインイン」**ボタンを使用できます。

この方法により：
- OAuth設定問題に関係なくアプリケーションにアクセス可能
- 開発作業を継続できる
- バックグラウンドでOAuth設定を修正できる

## 詳細診断手順

### Supabaseログの確認

1. Supabase Dashboard → Logs → Auth
2. 認証試行時のログエントリを確認
3. 具体的なエラーメッセージを探す

### GitHubのAuthorized OAuth Appsの確認

1. GitHub Settings → Applications → Authorized OAuth Apps
2. KDX Designerアプリが表示されるか確認
3. 権限スコープを確認（user:emailが含まれているか）

### ネットワーク診断

1. ブラウザの開発者ツールを開く
2. Network タブで認証フローを監視
3. 失敗したリクエストの詳細を確認

## よくある問題と解決策

### 問題1: "Client Secret invalid"
**解決策**: Client Secretを再生成して貼り直し

### 問題2: "Email not public"
**解決策**: GitHubでメール公開設定を変更

### 問題3: "Redirect URI mismatch"
**解決策**: GitHub OAuth AppのCallback URLを確認

### 問題4: "Rate limiting"
**解決策**: 少し待ってから再試行

## 最終確認項目

認証が成功するには、以下がすべて正しく設定されている必要があります：

1. ✅ GitHubのメールアドレスが公開されている
2. ✅ GitHub OAuth AppのCallback URLが正確
3. ✅ SupabaseのClient ID/Secretが正確
4. ✅ SupabaseのRedirect URLsが設定されている
5. ✅ GitHubアカウントに必要な権限がある

すべて確認後も問題が続く場合は、開発モードを使用してアプリケーション開発を継続し、後でOAuth設定を見直してください。