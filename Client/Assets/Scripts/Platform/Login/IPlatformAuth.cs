using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

using Google;
using System.Linq;

public enum UserLoginType
{
    None,
    Guest,
    Google,
}

public interface IPlatformAuth
{
    public bool IsAuthValid
    {
        get;
    }

    public bool IsLogin
    {
        get;
    }

    public string UserId
    {
        get;
    }

    public UserLoginType CurrentLoginType
    {
        get;
    }

    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null);
    public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null);
    public void SignOut();

}

public class PlatformGuestAuth : IPlatformAuth
{
    public bool IsAuthValid => isAuthValid;
    private bool isAuthValid = false;

    public bool IsLogin => isLogin;
    private bool isLogin = false;

    public string UserId
    {
        get
        {
            string host = System.Net.Dns.GetHostName();
            var entry = System.Net.Dns.GetHostEntry(host);
            var ipAddr = entry.AddressList;
            var address = ipAddr.FirstOrDefault();

            return address.ToString().Replace(".", "").Replace(":", "") + host.Replace(".", "").Replace(":", "");
        }
    }

    public UserLoginType CurrentLoginType => UserLoginType.Guest;

    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
    {
        isAuthValid = true;
        OnConnectAuthSuccess?.Invoke();
        return true;
    }

    public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        isLogin = true;
        OnSignInSuccess?.Invoke();
    }

    public void SignOut()
    {
        isLogin = false;
    }
}

/// <summary>
/// ���̾�̽� �� ���� ������ �ϴ� ���� ���Ǵ� ����
/// </summary>

public class PlatformGoogleAuth : IPlatformAuth
{
    private FirebaseApp app = null;
    private FirebaseAuth auth = null;


    public bool IsAuthValid
    {
        get
        {
            return app != null && auth != null;
        }
    }

    public bool IsLogin
    {
        get
        {
            return IsAuthValid && user != null && UserId != string.Empty;
        }
    }

    public UserLoginType CurrentLoginType => UserLoginType.Google;

    private FirebaseUser user = null;
    public string UserId
    {
        get;
        private set;
    } = string.Empty;

    private GoogleSignIn googleModule = null;
    private readonly string AndroidClientID = "834296008969-ha5c3bfbqjqfh21jo08nggjho53s9tt0.apps.googleusercontent.com";
    private readonly string WebClientID = "834296008969-3jm5utarunoinuhm5tc8f2ulus81v91j.apps.googleusercontent.com";
    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
    {
        if (IsAuthValid) // �̹� ���̾�̽� ������ ���� �����
            return false;

        InitAuth();
        return true;
    }

    private void InitAuth()
    {
        app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
    }

    private void SetFirebaseCurrentUser(FirebaseUser currentUser)
    {
        if (currentUser == null)
            return;

        user = currentUser;
        UserId = currentUser.UserId;
    }

    private void UnsetFirebaseCurrentUser()
    {
        user = null;
        UserId = string.Empty;
    }

    public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        if (IsLogin) // �̹� �α����� �Ϸ��� ���
        {
            Debug.LogError($"�̹� �α��� �����Դϴ�. {UserId}");
            return;
        }

        GoogleAuthenticate(OnGetToken: (Credential c) => { SignInByCredential(c, OnSignInSuccess, OnSignInFailed, OnSignCanceled); },
                      OnSignInFailed);
    }

    public void SignOut()
    {
        if (!IsLogin)
        {
            Debug.LogError("�̹� �α׾ƿ� �����Դϴ�.");
            return;
        }

        googleModule?.SignOut();
        auth?.SignOut();

        Debug.LogError($"{UserId} ������ �α׾ƿ��Ͽ����ϴ�.");
        UnsetFirebaseCurrentUser();
    }

    public void RegistStateChanged(EventHandler handler)
    {
        if (auth == null)
        {
            Debug.LogError("���̾�̽� ������ �Ϸ���� �ʾҴµ� StateChanged �ݹ��� ����մϴ�.");
            return;
        }

        auth.StateChanged -= handler;
        auth.StateChanged += handler;
    }

    public void UnregistStateChanged(EventHandler handler)
    {
        if (auth == null)
        {
            Debug.LogError("���̾�̽� ������ �Ϸ���� �ʾҴµ� StateChanged �ݹ��� ����մϴ�.");
            return;
        }

        auth.StateChanged -= handler;
    }

    private void GoogleAuthenticate(Action<Credential> OnGetToken, Action OnFailed = null)
    {
        if (googleModule == null)
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration { WebClientId = WebClientID, RequestEmail = true, RequestIdToken = true };
            GoogleSignIn.Configuration.UseGameSignIn = false;
            GoogleSignIn.Configuration.RequestIdToken = true;
            googleModule = GoogleSignIn.DefaultInstance;
        }

        googleModule.SignIn()
            .ContinueWithOnMainThread((task) =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log($"�̸��� �α��� ���� : {task.Result.UserId}");
                    OnFailed?.Invoke();
                }
                else if (task.IsCompleted)
                {
                    Debug.Log($"�̸��� �α��� ���� : {task.Result.UserId}");

                    var credential = GetUserCredential(task.Result.IdToken);
                    OnGetToken?.Invoke(credential);
                }
            });
    }

    private Credential GetUserCredential(string token)
    {
        string idToken = token;
        string accessToken = GetGoogleAccessToken();
        return GoogleAuthProvider.GetCredential(idToken, accessToken);
    }

    private string GetGoogleAccessToken()
    {
        return null; // ���� �ǹ̰� �ִ� �� ��, �ٵ� null�� ��... ���� ���� �� ����
    }

    private void SignInByCredential(Credential credential, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                OnSignInFailed?.Invoke();
                Debug.LogError($"�̸��� �α��� ���� : {task.Result.Email}");
            }
            else if (task.IsCanceled)
            {
                OnSignCanceled?.Invoke();
                Debug.LogWarning($"�̸��� �α��� ��� : {task.Result.Email}");
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("�̸��� �α��� ���� : {0} ({1})", newUser.DisplayName, newUser.UserId);

                SetFirebaseCurrentUser(newUser);

                OnSignInSuccess?.Invoke();
            }
        });
    }
}