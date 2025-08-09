# GitHub リポジトリ セットアップ手順

## 1. GitHubでリポジトリを作成

1. GitHubにログイン
2. 右上の「+」→「New repository」をクリック
3. 以下の設定で作成：
   - Repository name: `KDX_Projects`
   - Description: `統合開発環境用ソリューション - WPFアプリケーションとWeb APIの統合プロジェクト`
   - Public/Private: 組織のポリシーに従って選択
   - **初期化オプションはすべてOFF**（README、.gitignore、ライセンスは追加しない）

## 2. ローカルリポジトリの設定

```bash
# 現在のディレクトリ: C:\Users\shimada.KANAMORI-SYSTEM\source\repos\KDX_Projects

# ユーザー情報の設定（まだの場合）
git config user.name "Your Name"
git config user.email "your.email@kanamori-system.com"

# リモートリポジトリの追加
git remote add origin https://github.com/[organization]/KDX_Projects.git
# または SSH を使用する場合
git remote add origin git@github.com:[organization]/KDX_Projects.git

# 現在の状態を確認
git status
```

## 3. 初回コミットとプッシュ

```bash
# すべてのファイルをステージング
git add .

# 初回コミット
git commit -m "Initial commit: 5層アーキテクチャ構成でプロジェクトを初期化

- Kdx.Contracts: 共通DTO/Enum
- Kdx.Core: ビジネスロジック層
- Kdx.Infrastructure: データアクセス層
- KdxDesigner: WPFアプリケーション
- KdxMigrationAPI: Web API

Docker環境（PostgreSQL + pgAdmin）およびGitHub Actions CI/CD設定を含む"

# mainブランチに名前を変更（必要な場合）
git branch -M main

# GitHubへプッシュ
git push -u origin main
```

## 4. ブランチの作成

```bash
# developブランチを作成
git checkout -b develop
git push -u origin develop

# mainに戻る
git checkout main
```

## 5. GitHubでの設定

### ブランチ保護の設定
1. Settings → Branches
2. 「Add rule」をクリック
3. Branch name pattern: `main`
4. 以下を有効化：
   - ✅ Require a pull request before merging
   - ✅ Require approvals (1)
   - ✅ Dismiss stale pull request approvals when new commits are pushed
   - ✅ Require status checks to pass before merging
     - ✅ Require branches to be up to date before merging
     - Status checks: `build`
   - ✅ Require conversation resolution before merging
   - ✅ Include administrators

### Secrets の設定（必要に応じて）
1. Settings → Secrets and variables → Actions
2. 必要なシークレットを追加：
   - `NUGET_API_KEY`（NuGetパッケージ公開用）
   - `DOCKER_HUB_TOKEN`（Docker Hub用）

### GitHub Pages（ドキュメント公開用）
1. Settings → Pages
2. Source: Deploy from a branch
3. Branch: main / docs（または gh-pages）

## 6. チームメンバーの招待

1. Settings → Manage access
2. 「Invite a collaborator」
3. チームメンバーのGitHubユーザー名を入力

## 7. プロジェクト管理の設定

### Issues の有効化
Settings → General → Features → ✅ Issues

### Projects の作成
1. Projects タブ → New project
2. テンプレート: Basic kanban
3. カラム: To do, In progress, Done

### Milestones の作成
1. Issues → Milestones → New milestone
2. 例: v1.0.0, v1.1.0

## 8. 追加の推奨設定

### Branch naming convention
- `feature/issue-番号-機能名`
- `bugfix/issue-番号-バグ説明`
- `hotfix/issue-番号-緊急修正`

### Commit message template
`.gitmessage` ファイルを作成：
```
# <type>(<scope>): <subject>
# 
# <body>
# 
# <footer>
```

設定：
```bash
git config commit.template .gitmessage
```

## 9. CI/CD の確認

1. プッシュ後、Actions タブを確認
2. ワークフローが正常に実行されることを確認
3. バッジが緑色になることを確認

## 10. README.md の更新

README.md の以下の部分を実際のURLに更新：
- `[organization]` を実際の組織名/ユーザー名に置換
- バッジのURLを更新

## トラブルシューティング

### Permission denied (publickey)
SSH キーの設定が必要です：
```bash
ssh-keygen -t ed25519 -C "your.email@kanamori-system.com"
# 公開鍵を GitHub に追加
cat ~/.ssh/id_ed25519.pub
```

### プッシュが拒否される
```bash
# リモートの変更を取得
git pull origin main --rebase
# 再度プッシュ
git push origin main
```

## 完了チェックリスト

- [ ] GitHubリポジトリ作成
- [ ] ローカルGit設定
- [ ] 初回コミット＆プッシュ
- [ ] developブランチ作成
- [ ] ブランチ保護設定
- [ ] CI/CD動作確認
- [ ] チームメンバー招待
- [ ] Issues/Projects設定
- [ ] README.md更新