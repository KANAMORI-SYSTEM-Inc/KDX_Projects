# Supabase GitHub OAuth設定ガイド

## 概要
このドキュメントでは、KDX DesignerアプリケーションでGitHub OAuth認証を使用するためのSupabase側の設定方法を説明します。

## 前提条件
- Supabaseプロジェクトが作成済みであること
- GitHubアカウントを持っていること
- GitHub OAuth Appが作成済みであること

## 1. GitHub OAuth Appの作成

### GitHubでOAuth Appを作成:
1. GitHubにログイン
2. Settings → Developer settings → OAuth Apps に移動
3. 「New OAuth App」をクリック
4. 以下の情報を入力:
   - **Application name**: KDX Designer
   - **Homepage URL**: http://localhost:3000
   - **Authorization callback URL**: `https://[YOUR-PROJECT-REF].supabase.co/auth/v1/callback`
   
   ※ `[YOUR-PROJECT-REF]`は、Supabaseプロジェクトの参照ID（例: `xyzcompany`）

5. 「Register application」をクリック
6. **Client ID**と**Client Secret**をメモしておく

## 2. Supabase側の設定

### Supabaseダッシュボードでの設定:

1. [Supabase Dashboard](https://app.supabase.com)にログイン
2. プロジェクトを選択
3. 左側メニューから「Authentication」→「Providers」を選択
4. 「GitHub」を見つけて「Enable」をONにする
5. 以下の情報を入力:
   - **Client ID**: GitHubから取得したClient ID
   - **Client Secret**: GitHubから取得したClient Secret
   - **Redirect URL**: 自動生成される（コピーしておく）

### 重要なURL設定:

#### Site URL（認証後のリダイレクト先）:
1. Authentication → URL Configuration に移動
2. **Site URL**を設定: `http://localhost:3000`
   - これは認証成功後にユーザーがリダイレクトされるURL

#### Redirect URLs（許可するリダイレクトURL）:
1. **Redirect URLs**に以下を追加:
   ```
   http://localhost:3000
   http://localhost:3000/
   http://localhost:3000/auth/callback
   ```

#### その他の推奨設定:
- **Email Confirmations**: OFF（GitHub認証のみ使用する場合）
- **Enable Email Provider**: OFF（GitHub認証のみ使用する場合）

## 3. アプリケーション側の設定

### appsettings.json:
```json
{
  "Supabase": {
    "Url": "https://[YOUR-PROJECT-REF].supabase.co",
    "AnonKey": "your-anon-key-here"
  }
}
```

### 認証フロー:
1. アプリケーションが`http://localhost:3000/`でHTTPリスナーを起動
2. SupabaseがGitHub OAuth URLを生成（リダイレクト先: `http://localhost:3000/`）
3. ユーザーがGitHubで認証
4. GitHubがSupabaseにリダイレクト
5. Supabaseが`http://localhost:3000/?code=xxx`にリダイレクト
6. アプリケーションのHTTPリスナーが認証コードを受け取る
7. 認証コードをセッショントークンに交換

## 4. トラブルシューティング

### よくある問題と解決方法:

#### 「Redirect URL mismatch」エラー:
- Supabaseの「URL Configuration」で`http://localhost:3000`が許可されているか確認
- GitHubのOAuth App設定でCallback URLが正しいか確認

#### 認証後にアプリに戻らない:
- HTTPリスナーが正しくポート3000で起動しているか確認
- Windowsファイアウォールがポート3000をブロックしていないか確認

#### 「RLS enabled but no policies」エラー:
- 各テーブルのRLSポリシーを設定するか、開発中はRLSを無効化

### ローカル開発環境での注意点:
- `localhost`と`127.0.0.1`は異なるホストとして扱われる
- 必ず`http://localhost:3000`を使用（HTTPSではない）
- ポート3000が他のアプリケーションで使用されていないか確認

## 5. セキュリティ考慮事項

### 本番環境への移行時:
1. **Site URL**を本番環境のURLに変更
2. **Redirect URLs**に本番環境のURLを追加
3. GitHub OAuth AppのCallback URLを更新
4. HTTPSを使用（本番環境では必須）
5. 適切なRLSポリシーを設定

### APIキーの管理:
- `AnonKey`はクライアント側で使用可能（公開鍵）
- `Service Role Key`は絶対にクライアント側に含めない
- 環境変数または安全な設定ファイルでキーを管理

## 6. テスト手順

1. KDX Designerアプリケーションを起動
2. ログイン画面で「GitHubでサインイン」をクリック
3. ブラウザが開き、GitHubの認証画面が表示される
4. GitHubで認証を承認
5. 自動的にアプリケーションに戻り、メイン画面が表示される

## 参考リンク
- [Supabase Auth Documentation](https://supabase.com/docs/guides/auth)
- [GitHub OAuth Apps Documentation](https://docs.github.com/en/developers/apps/building-oauth-apps)
- [Supabase GitHub Provider](https://supabase.com/docs/guides/auth/social-login/auth-github)