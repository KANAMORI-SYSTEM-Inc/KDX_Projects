# KDX Projects - Claude AI Assistant Context

## プロジェクト概要
KDX Projectsは、複数のプロジェクトを統合管理する開発環境です。

## アーキテクチャ
- **KdxDesigner**: WPFデスクトップアプリケーション（.NET 8.0）
- **KdxMigrationAPI**: Web API（ASP.NET Core 8.0）
- **データベース**: PostgreSQL（Docker環境）/ Access（レガシー）

## 主要技術スタック
- .NET 8.0
- WPF (Windows Presentation Foundation)
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker

## プロジェクト構造
```
KDX_Projects/
├── KdxDesigner/         # WPFアプリケーション
│   ├── Models/          # データモデル
│   ├── ViewModels/      # MVVM ViewModels
│   ├── Views/           # XAML Views
│   ├── Services/        # ビジネスロジック
│   └── Utils/           # ユーティリティ
├── KdxMigrationAPI/     # Web API
│   ├── Controllers/     # APIコントローラー
│   ├── Services/        # ビジネスロジック
│   └── Data/            # データアクセス層
└── docker-compose.yml   # Docker構成

```

## 開発規約

### コーディング規約
- C# 最新バージョンの機能を使用
- Nullable Reference Types有効
- 非同期処理はasync/awaitパターンを使用

### 命名規則
- クラス名: PascalCase
- メソッド名: PascalCase
- プロパティ名: PascalCase
- プライベートフィールド: _camelCase
- ローカル変数: camelCase

### Git規約
- ブランチ戦略: Git Flow
- コミットメッセージ: 日本語可
- プルリクエスト必須

## データベース設計

### 主要テーブル
- Companies - 会社情報
- Machines - 機械情報
- Operations - 操作情報
- Processes - プロセス情報
- Devices - デバイス情報
- IO - 入出力情報
- Memories - メモリ情報
- Timers - タイマー情報

## 開発環境
- Windows 11
- Visual Studio 2022 / VS Code
- Docker Desktop
- Git

## テスト実行
```bash
dotnet test
```

## ビルド＆実行
```bash
# ソリューション全体のビルド
dotnet build

# KdxDesigner実行
cd KdxDesigner
dotnet run

# KdxMigrationAPI実行
cd KdxMigrationAPI
dotnet run
```

## Docker環境
```bash
# サービス起動
docker-compose up -d

# サービス停止
docker-compose down

# ログ確認
docker-compose logs -f
```

## 注意事項
- Accessデータベースファイルのパスは環境に依存
- Windows環境でのみKdxDesignerが動作
- PostgreSQLへの移行作業進行中

## 今後の計画
- マイクロサービス化
- Kubernetes対応
- CI/CDパイプライン構築
- 単体テスト・統合テストの充実