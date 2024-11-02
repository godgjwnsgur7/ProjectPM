using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class NetworkManager : MonoBehaviour
{
    public event Action<List<Lobby>> lobbyListChangedEvent; // �κ� ��� ���� �� ȣ��
    public event Action<Lobby> joinLobbyEvent; // �� ���� ����, �� ���� �� ȣ��
    public event Action leaveLobbyEvent; // ���� ���� �� ȣ��
    public event Action gameStartEvent; // ���� ���� �� ȣ��
    private Lobby joinedLobby; // ���� ������ �� ����

    // ����Ƽ ���� ���� �α���
    public async void Init()
    {
        // ������ ����
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(Managers.Platform.GetUserID()); // ������ ����

        await UnityServices.InitializeAsync(initializationOptions);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
