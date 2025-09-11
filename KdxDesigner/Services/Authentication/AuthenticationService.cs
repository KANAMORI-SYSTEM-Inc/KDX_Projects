using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using System;
using System.Threading.Tasks;
using SupabaseClient = Supabase.Client;

namespace KdxDesigner.Services.Authentication
{
    public interface IAuthenticationService
    {
        event EventHandler<Session?> AuthStateChanged;
        Session? CurrentSession { get; }
        bool IsAuthenticated { get; }
        Task<string?> SignInWithGitHubAsync();
        Task SignOutAsync();
        Task<Session?> GetSessionAsync();
        Task<Session?> ExchangeCodeForSessionAsync(string code);
        Task<bool> SignInWithEmailAsync(string email, string password);
        Task<bool> SignUpWithEmailAsync(string email, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly SupabaseClient _supabaseClient;
        private Session? _currentSession;

        public event EventHandler<Session?> AuthStateChanged = delegate { };
        
        public Session? CurrentSession => _currentSession;
        public bool IsAuthenticated => _currentSession != null;

        public AuthenticationService(SupabaseClient supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
            
            System.Diagnostics.Debug.WriteLine($"AuthenticationService initialized");
            
            // 認証状態の変更を監視
            _supabaseClient.Auth.AddStateChangedListener((sender, changed) =>
            {
                System.Diagnostics.Debug.WriteLine($"Auth state changed: {changed}");
                if (changed == Constants.AuthState.SignedIn || 
                    changed == Constants.AuthState.SignedOut ||
                    changed == Constants.AuthState.TokenRefreshed)
                {
                    _currentSession = _supabaseClient.Auth.CurrentSession;
                    AuthStateChanged?.Invoke(this, _currentSession);
                }
            });

            // 初期セッションを取得
            _currentSession = _supabaseClient.Auth.CurrentSession;
            System.Diagnostics.Debug.WriteLine($"Initial session: {(_currentSession != null ? "Exists" : "None")}");
        }

        public async Task<string?> SignInWithGitHubAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Starting GitHub OAuth flow...");
                
                // GitHub OAuthのURLを生成
                var providerAuthState = await _supabaseClient.Auth.SignIn(
                    Constants.Provider.Github,
                    new SignInOptions
                    {
                        // リダイレクトURLを設定（ローカルホスト）
                        RedirectTo = "http://localhost:3000/"
                    });
                
                // ProviderAuthStateからURIを取得
                var authUrl = providerAuthState?.Uri?.ToString();
                
                System.Diagnostics.Debug.WriteLine($"GitHub OAuth URL generated: {authUrl}");
                
                // ブラウザでGitHub認証ページを開く
                if (!string.IsNullOrEmpty(authUrl))
                {
                    System.Diagnostics.Debug.WriteLine($"Opening browser with URL: {authUrl}");
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = authUrl,
                        UseShellExecute = true
                    });
                    return authUrl;
                }
                
                System.Diagnostics.Debug.WriteLine("No OAuth URL was generated");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GitHub sign in failed: {ex}");
                System.Diagnostics.Debug.WriteLine($"Exception type: {ex.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new InvalidOperationException($"GitHubサインインに失敗しました: {ex.Message}", ex);
            }
        }

        public async Task<Session?> ExchangeCodeForSessionAsync(string code)
        {
            try
            {
                // 認証コードをセッションに交換
                // PKCEは使用しないのでcodeVerifierは空文字列
                var session = await _supabaseClient.Auth.ExchangeCodeForSession(code, string.Empty);
                _currentSession = session;
                System.Diagnostics.Debug.WriteLine($"Session exchanged successfully");
                return session;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Code exchange failed: {ex.Message}");
                throw new InvalidOperationException($"認証コードの交換に失敗しました: {ex.Message}", ex);
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                await _supabaseClient.Auth.SignOut();
                _currentSession = null;
                System.Diagnostics.Debug.WriteLine("Sign out successful");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sign out failed: {ex.Message}");
                throw new InvalidOperationException($"サインアウトに失敗しました: {ex.Message}", ex);
            }
        }

        public async Task<Session?> GetSessionAsync()
        {
            try
            {
                _currentSession = await _supabaseClient.Auth.RetrieveSessionAsync();
                return _currentSession;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to retrieve session: {ex.Message}");
                return null;
            }
        }
        
        public async Task<bool> SignInWithEmailAsync(string email, string password)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting email sign in for: {email}");
                
                var session = await _supabaseClient.Auth.SignIn(email, password);
                
                if (session != null)
                {
                    _currentSession = session;
                    System.Diagnostics.Debug.WriteLine("Email sign in successful");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email sign in failed: {ex.Message}");
                throw new InvalidOperationException($"メールサインインに失敗しました: {ex.Message}", ex);
            }
        }
        
        public async Task<bool> SignUpWithEmailAsync(string email, string password)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting email sign up for: {email}");
                
                var session = await _supabaseClient.Auth.SignUp(email, password);
                
                if (session != null)
                {
                    _currentSession = session;
                    System.Diagnostics.Debug.WriteLine("Email sign up successful");
                    return true;
                }
                
                System.Diagnostics.Debug.WriteLine("Sign up failed - no session returned");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email sign up failed: {ex.Message}");
                throw new InvalidOperationException($"メール新規登録に失敗しました: {ex.Message}", ex);
            }
        }
    }
}