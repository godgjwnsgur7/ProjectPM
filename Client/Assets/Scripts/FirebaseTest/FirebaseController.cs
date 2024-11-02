using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using Firebase;
using System;

/// <summary>
/// ���̾�̽� �׽�Ʈ ��ũ��Ʈ
/// </summary>
public class FirebaseController : MonoBehaviour
{
    private FirebaseAuth auth; // ��������
    private FirebaseUser user; // ��������

    string userToken = "asdf1234";

    FBUserData fbDataInfo = new FBUserData();

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                FirebaseInit();

                Debug.Log("���̾�̽� ���� ����");
            }
            else
            {
                Debug.LogError("���̾�̽� ���� ����");
            }
        });
    }

    private void FirebaseInit()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged -= AuthStateChanged;
        auth.StateChanged += AuthStateChanged;
    }

    /// <summary>
    /// ���� ���� ���� �� ȣ��Ǵ� �޼���
    /// </summary>
    private void AuthStateChanged(object sender, EventArgs e)
    {
        FirebaseAuth senderAuth = sender as FirebaseAuth;
        if(senderAuth != null)
        {
            user = senderAuth.CurrentUser;

            if(user != null)
            {
                Debug.Log($"���� ���� ID : {user.UserId}");
            }
        }
    }

    public void ReadTestData()
    {
        // DB�� ���� ��ġ ����
        DatabaseReference testDB = FirebaseDatabase.DefaultInstance.GetReference("TestData");

        testDB.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.LogWarning("�׽�Ʈ ������ �ε� ����");
                return;
            }
            if(task.IsCanceled)
            {
                Debug.LogWarning("�׽�Ʈ ������ �ε� ���");
                return;
            }

            DataSnapshot snapshot = task.Result;
            Debug.Log(snapshot.ChildrenCount);

            foreach(var message in snapshot.Children)
            {
                Debug.Log($"{message.Key} : {message.Child("username").Value.ToString()} , {message.Child("value").Value.ToString()}");
            }
        });
    }
    
    public class TestDataClass
    {
        public string nickname;
        public int num;

        public TestDataClass(string nickname, int num)
        {
            this.nickname = nickname;
            this.num = num;
        }
    }


    public void OnClickGuestLogin()
    {
        GuestLogin();
    }

    public void OnClikcGuestLogout()
    {
        auth.SignOut();
        Debug.Log("�α׾ƿ�");
    }

    private Task GuestLogin()
    {
        return auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("�Խ�Ʈ �α��� ���");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning("�Խ�Ʈ �α��� ����");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("�Խ�Ʈ �α��� ����");
        });
    }

    #region �̸��� �α���
    // �̸��� �α����� �׽�Ʈ�� �ʹ� ������ �� ���� ����;;;
    public void JoinEmailMembership(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("�̸��� ȸ������ ���");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogWarning("�̸��� ȸ������ ����");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("�̸��� ȸ������ �Ϸ�");
        });
    }

    private void EmailLogin(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("�̸��� �α��� ���");
                return;
            }
            if (task.IsFaulted)
            {

                Debug.LogWarning("�̸��� �α��� ����");
                return;
            }

            FirebaseUser newUser = task.Result.User;
            Debug.Log("�̸��� �α��� �Ϸ�");
        });
    }
    #endregion
}
