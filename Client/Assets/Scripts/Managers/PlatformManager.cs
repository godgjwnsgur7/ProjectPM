using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using JetBrains.Annotations;
using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class PlatformManager
{
    private IPlatformAuth Auth
    {
        get
        {
            auth ??= new PlatformGuestAuth(); // �ϴ� ������ �Խ�Ʈ�α���
            return auth;
        }
    }
    private IPlatformAuth auth = null;

    FirebaseDB DB = new FirebaseDB();

    public string MyUserID { get; private set; } = null;

    public void Initialize()
    {
        Clear();

        FirebaseApp.CheckAndFixDependenciesAsync()
               .ContinueWithOnMainThread(task =>
               {
                   if (task.Result == DependencyStatus.Available)
                   {
                       Auth.TryConnectAuth();
                       
                       Debug.Log("���̾�̽� ���� ����");
                   }
                   else
                   {
                       Debug.LogError("���̾�̽� ���� ����");
                   }
               });
    }
    
    public void Clear()
    {
        auth = new PlatformGuestAuth();
    }

    public void RegisterFBUserInfoCallback(IFBUserInfoPostProcess mono)
    {
        DB.RegisterIFBUserInfoPostProcess(mono);
    }

    public void UnregisterFBUserInfoCallback(IFBUserInfoPostProcess mono)
    {
        DB.UnregisterIFBUserInfoPostProcess(mono);
    }

    public void RegisterFBUserItemCallback(IFBUserItemPostProcess mono)
    {
        DB.RegisterIFBUserItemPostProcess(mono);
    }

    public void UnregisterFBUserItemCallback(IFBUserItemPostProcess mono)
    {
        DB.UnregisterIFBUserItemPostProcess(mono);
    }

    public bool UpdateDB(FirebaseDataCategory type, FBDataBase data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if(data == null || GetUserID() == string.Empty)
        {
            OnCanceled?.Invoke();
            return false;
        }

        DB.UpdateDB(data);
        return true;
    }

    public void GetRakingBoardDatas(Action<List<RankingBoardData>> onRankingBoardData)
    {
        DB.GetRankingBoardDatas(onRankingBoardData);
    }

    private System.Random random = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF); //���� �õ尪

    public void PushRankingBoardData(RankingBoardData data)
    {
        // �׽�Ʈ�� ����
        int lenght = 34;
        string strPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";  //���� ���� Ǯ
        char[] chRandom = new char[lenght];

        for (int i = 0; i < lenght; i++)
        {
            chRandom[i] = strPool[random.Next(strPool.Length)];
        }
        string strRet = new String(chRandom);   // char to string

        DB.PushMyRankingBoardData(data, strRet);

        // ������ �̷��Ը� ����ؾ� �� - �Ű����� ���� DB���� UserID �����ͼ� ����
        // DB.PushMyRankingBoardData(data, GetUserID());
    }

    public string GetUserID()
    {
        if (!Auth.IsAuthValid)
        {
            Debug.LogError("���� �α����� ���� ���� �����Դϴ�.");
            return string.Empty;
        }

        return Auth.UserId;
    }

    public void Login(Action _OnSignInSuccess = null, Action _OnSignInFailed = null, Action _OnSignCanceled = null, Action<bool> _OnCheckFirstUser = null)
    {
        Auth.SignIn(
            OnSignInSuccess: () =>
            {
                // �α��� ���� ( ���� ID ���� ) -> ���� ID�� DB�� ����
                DB.InitDB();
                _OnSignInSuccess?.Invoke();
            },
            OnSignInFailed: () =>
            {
                _OnSignInFailed?.Invoke();
            },
            OnSignCanceled: () =>
            {
                _OnSignCanceled?.Invoke();
            }
        );
    }

    public void Logout()
    {
        if (Auth.IsLogin)
            Auth.SignOut();
    }
}