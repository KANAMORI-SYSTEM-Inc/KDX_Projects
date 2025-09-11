# 開発モードサインイン トラブルシューティング

## 問題
開発モードでのサインインが失敗している

## 考えられる原因

### 1. Supabase Email認証が無効化されている
- Supabase Dashboard → Authentication → Providers → Email
- **Enabled** が OFF になっている可能性

### 2. Supabase新規ユーザー登録が無効化されている
- Supabase Dashboard → Authentication → Settings
- **Disable new user signups** が ON になっている可能性

### 3. SupabaseクライアントのURL/KEYが間違っている
- appsettings.json の設定確認が必要

## 確認手順

### Step 1: Supabase Email Provider確認
1. Supabase Dashboard: https://app.supabase.com/project/eebsrvkpcjsqmuvfqtve
2. Authentication → Providers → Email
3. **Enabled**: ✅ ON であることを確認

### Step 2: 新規ユーザー登録の確認
1. Authentication → Settings
2. **Disable new user signups**: ❌ OFF であることを確認

### Step 3: appsettings.json確認
以下の設定が正確であることを確認：

```json
{
  "SupabaseConfiguration": {
    "Url": "https://eebsrvkpcjsqmuvfqtve.supabase.co",
    "Key": "正しいAPIキー"
  }
}
```

### Step 4: デバッグログ確認
アプリケーション実行時のVisual Studio出力ウィンドウで以下を確認：
- "Attempting development mode sign in..."
- "Supabase client initialized: True"
- エラーメッセージの詳細

## 解決方法

### Option A: Supabase設定修正
上記の設定を確認・修正する

### Option B: 一時的なモック認証
完全にSupabaseを迂回した認証を実装

### Option C: 別のSupabaseプロジェクト
新しいSupabaseプロジェクトで試行

## 診断用ログの見方

### 成功時のログ:
```
Attempting development mode sign in...
Supabase client initialized: True
Dev email: dev@kdxdesigner.local
Attempting sign in with existing development user...
Sign in result: True
Session user: dev@kdxdesigner.local
Development sign in successful
```

### 失敗時のログ例:
```
Attempting development mode sign in...
Supabase client initialized: True
Dev email: dev@kdxdesigner.local
Attempting sign in with existing development user...
Sign in failed: [具体的なエラーメッセージ]
Creating development user...
Sign up failed: [具体的なエラーメッセージ]
Development mode sign in failed - no session created
```

## 次のステップ

1. まず上記の診断用コードを実行
2. Visual Studioの出力ウィンドウで詳細なログを確認
3. エラーメッセージに基づいて対応