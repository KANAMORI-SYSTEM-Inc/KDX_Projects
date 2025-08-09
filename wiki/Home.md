# KDX Projects - アーキテクチャドキュメント

## 概要

KDX Projectsは、金森システム株式会社の自動ラダープログラム生成システムのモダン化プロジェクトです。従来のモノリシックなWPFアプリケーションを、クリーンアーキテクチャに基づいた5層構造に再構築しました。

## 🎯 プロジェクトの目的

1. **レガシーシステムのモダナイゼーション**
   - Microsoft AccessデータベースからSQL Server/PostgreSQLへの移行準備
   - API中心のアーキテクチャへの移行
   - コンテナ化による開発環境の統一

2. **開発効率の向上**
   - Gitによるバージョン管理
   - Dockerによる開発環境の標準化
   - CI/CDパイプラインの構築準備

## 📚 ドキュメント構成

- [アーキテクチャ概要](Architecture-Overview.md) - システム全体の設計思想と構造
- [プロジェクト構成](Project-Structure.md) - 各プロジェクトの役割と責任
- [開発環境セットアップ](Development-Setup.md) - 開発環境の構築手順
- [データベース移行戦略](Database-Migration.md) - AccessからSQL Serverへの移行計画
- [API統合](API-Integration.md) - WPFアプリケーションとAPIの連携方法
- [Docker環境](Docker-Configuration.md) - コンテナ化とローカル開発環境

## 🚀 クイックスタート

```bash
# リポジトリのクローン
git clone https://github.com/YourOrg/KDX_Projects.git
cd KDX_Projects

# Docker環境の起動
docker-compose up -d

# ソリューションのビルド
dotnet build

# APIサーバーの起動
cd Kdx.API
dotnet run

# WPFアプリケーションの起動（別ターミナル）
cd ../KdxDesigner
dotnet run
```

## 📋 主な技術スタック

- **.NET 8.0** - 統一されたフレームワーク
- **WPF** - デスクトップアプリケーション（MVVM Pattern）
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - プライマリデータベース
- **Docker** - コンテナ化
- **NSwag** - API クライアント自動生成

## 🏗️ アーキテクチャの特徴

### クリーンアーキテクチャの採用
- 関心の分離による保守性の向上
- テスタビリティの確保
- 技術的な変更の影響を最小化

### 5層構造
1. **Presentation Layer** (KdxDesigner) - UI/UX
2. **API Layer** (Kdx.API) - RESTful API
3. **Contracts Layer** (Kdx.Contracts) - 共有DTOs
4. **Core Layer** (Kdx.Core) - ビジネスロジック
5. **Infrastructure Layer** (Kdx.Infrastructure) - データアクセス

## 📈 今後の展開

- [ ] Microsoft Accessからの完全移行
- [ ] CI/CDパイプラインの構築
- [ ] 自動テストの充実
- [ ] マイクロサービス化の検討
- [ ] クラウド環境への展開

## 📞 サポート

問題や質問がある場合は、GitHubのIssuesまたは社内のSlackチャンネルでお問い合わせください。

---
*最終更新: 2025年8月*