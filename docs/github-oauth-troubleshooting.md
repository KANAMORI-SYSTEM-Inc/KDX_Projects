# GitHub OAuth認証エラーの解決方法

## エラー: "Error getting user profile from external provider"

このエラーは、SupabaseがGitHubからユーザープロファイル情報を取得できない場合に発生します。

## 解決手順

### 1. GitHub OAuth App設定の確認

#### GitHub側の設定を確認:
1. [GitHub Developer Settings](https://github.com/settings/developers)にアクセス
2. OAuth Appsから対象のアプリを選択
3. 以下を確認:
   - **Application name**: 正しく設定されているか
   - **Homepage URL**: `http://localhost:3000`
   - **Authorization callback URL**: `https://[YOUR-PROJECT-REF].supabase.co/auth/v1/callback`

### 2. Supabase側の設定確認

#### A. GitHub Provider設定:
1. [Supabase Dashboard](https://app.supabase.com)にログイン
2. Authentication → Providers → GitHub
3. 以下を確認:
   - **Enabled**: ON
   - **Client ID**: GitHubから取得した値と一致
   - **Client Secret**: GitHubから取得した値と一致

#### B. URL Configuration:
1. Authentication → URL Configuration
2. 以下の設定を確認:
   ```
   Site URL: http://localhost:3000
   
   Redirect URLs:
   - http://localhost:3000
   - http://localhost:3000/
   - http://localhost:3000/auth/callback
   ```

### 3. よくある原因と解決方法

#### 原因1: Client SecretのCopy&Pasteエラー
- **症状**: Client Secretに余分なスペースや改行が含まれている
- **解決**: Client Secretを再生成して貼り直す

#### 原因2: GitHub OAuth Appのスコープ設定
- **症状**: 必要なユーザー情報にアクセスできない
- **解決**: GitHub OAuth Appの設定でuser:emailスコープが有効になっているか確認

#### 原因3: Supabaseプロジェクトの地域設定
- **症状**: 地域によってGitHub APIへのアクセスが制限される
- **解決**: 別の地域でSupabaseプロジェクトを作成

#### 原因4: GitHubアカウントのメールアドレス設定
- **症状**: GitHubアカウントでPublicメールが設定されていない
- **解決**: 
  1. GitHub → Settings → Emails
  2. メールアドレスを追加して認証
  3. Public email設定を確認

### 4. デバッグ手順

#### A. Supabaseログの確認:
1. Supabase Dashboard → Logs → Auth
2. エラーログを確認

#### B. GitHub OAuth Appの再作成:
1. 新しいOAuth Appを作成
2. 設定を慎重に入力:
   ```
   Application name: KDX Designer Dev
   Homepage URL: http://localhost:3000
   Authorization callback URL: https://[YOUR-PROJECT-REF].supabase.co/auth/v1/callback
   ```
3. 新しいClient ID/Secretを使用

#### C. Supabaseでの直接テスト:
1. Supabase Dashboard → SQL Editor
2. 以下のクエリを実行してauth設定を確認:
   ```sql
   SELECT * FROM auth.identities;
   SELECT * FROM auth.users;
   ```

### 5. 代替案: Magic Link認証の使用

GitHub OAuthが機能しない場合の代替案として、Email Magic Link認証を使用：

1. Supabase Dashboard → Authentication → Providers
2. Email Provider を有効化
3. コードを以下のように変更:
   ```csharp
   // Email認証の実装
   await _supabaseClient.Auth.SignIn(email);
   ```

### 6. 完全な設定リセット

問題が解決しない場合:

1. **GitHub側**:
   - OAuth Appを削除して再作成
   - 新しいClient ID/Secretを生成

2. **Supabase側**:
   - GitHub Providerを無効化
   - 設定をクリア
   - 新しいClient ID/Secretで再設定

3. **ローカル環境**:
   - ブラウザのCookieとキャッシュをクリア
   - アプリケーションの再起動

## テスト用の簡易認証実装

開発中の一時的な解決策として、モック認証を使用：

```csharp
// AuthenticationService.cs に追加
public async Task<bool> SignInWithMockAsync()
{
    // 開発用のモックセッション
    _currentSession = new Session
    {
        User = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "dev@test.local"
        },
        AccessToken = "mock-token",
        ExpiresIn = 3600
    };
    
    AuthStateChanged?.Invoke(this, _currentSession);
    return true;
}
```

## 関連リンク
- [Supabase Auth Documentation](https://supabase.com/docs/guides/auth)
- [GitHub OAuth Apps Documentation](https://docs.github.com/en/developers/apps/building-oauth-apps)
- [Supabase GitHub Issues](https://github.com/supabase/supabase/issues)