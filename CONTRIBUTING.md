# Contributing to KDX Projects

このプロジェクトへの貢献を検討いただき、ありがとうございます。

## 開発プロセス

### ブランチ戦略
- `main` - 本番環境用ブランチ
- `develop` - 開発用統合ブランチ
- `feature/*` - 新機能開発用
- `bugfix/*` - バグ修正用
- `hotfix/*` - 緊急修正用

### プルリクエストの手順
1. Issueを作成または既存のIssueを選択
2. featureブランチを作成
   ```bash
   git checkout -b feature/issue-番号-機能名
   ```
3. 変更をコミット（下記のコミット規約に従う）
4. プルリクエストを作成
5. レビュー＆マージ

## コミットメッセージ規約

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Type
- `feat`: 新機能
- `fix`: バグ修正
- `docs`: ドキュメントのみの変更
- `style`: コードの意味に影響しない変更（フォーマット等）
- `refactor`: リファクタリング
- `perf`: パフォーマンス改善
- `test`: テストの追加・修正
- `chore`: ビルドプロセスやツールの変更

### 例
```
feat(api): ユーザー認証エンドポイントを追加

JWT認証を使用したユーザー認証機能を実装。
- /api/auth/login エンドポイント追加
- /api/auth/refresh エンドポイント追加

Closes #123
```

## コーディング規約

### C# / .NET
- [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)に従う
- EditorConfigの設定を遵守
- コード分析の警告を解決

### 命名規則
- **クラス**: PascalCase
- **メソッド**: PascalCase
- **プロパティ**: PascalCase
- **フィールド（private）**: _camelCase
- **変数**: camelCase
- **定数**: UPPER_CASE

## テスト

### 単体テスト
- 新機能には必ずテストを追加
- カバレッジ70%以上を維持
- テストメソッド名: `MethodName_StateUnderTest_ExpectedBehavior`

### 実行方法
```bash
dotnet test
```

## ドキュメント

### 必須ドキュメント
- 新機能のREADME更新
- APIエンドポイントのXMLドキュメントコメント
- 複雑なロジックへのコメント

### ドキュメント記載場所
- API仕様: `/docs/api/`
- アーキテクチャ: `/docs/architecture/`
- 設定方法: `/docs/setup/`

## レビュープロセス

### レビューポイント
1. コードが要件を満たしているか
2. テストが適切に書かれているか
3. パフォーマンスへの影響
4. セキュリティの考慮
5. ドキュメントの更新

### マージ条件
- [ ] すべてのCIチェックが通過
- [ ] 1名以上のレビュー承認
- [ ] コンフリクトの解決
- [ ] ドキュメントの更新

## セットアップ

### 必要環境
- .NET 8.0 SDK
- Docker Desktop
- Visual Studio 2022 または VS Code

### 初期設定
```bash
# リポジトリのクローン
git clone https://github.com/[organization]/KDX_Projects.git
cd KDX_Projects

# 依存関係の復元
dotnet restore

# ビルド
dotnet build

# Docker環境の起動
docker-compose up -d
```

## 質問・サポート

- Issueで質問を投稿
- Discussionsで議論
- メール: dev@kanamori-system.com

## ライセンス

このプロジェクトに貢献することで、あなたのコントリビューションがMITライセンスの下でライセンスされることに同意したものとみなされます。