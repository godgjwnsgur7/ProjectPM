using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    #region Managers
    private DataManager _data = new DataManager();
    private GameManager _game = new GameManager();

    // NetworkManager MonoBehaviour�� new ��� �߰� ����
    // �����Ǹ� �ּ� ������
    private NetworkManager _network = new NetworkManager();
    private PlatformManager _platform = new PlatformManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance?._data; } }
    public static GameManager Game { get { return Instance?._game; } }
    public static NetworkManager Network { get { return Instance?._network; } }
    public static PlatformManager Platform { get { return Instance?._platform; } }
    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static SceneManagerEx Scene { get { return Instance?._scene; } }
    public static UIManager UI { get { return Instance?._ui; } }
    #endregion
    
    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // �ʱ�ȭ
            s_instance = go.GetComponent<Managers>();
        }
    }
    
    /// <summary>
    /// �� �̵� �� ȣ���
    /// </summary>
    public static void Clear()
    {
        UI.Clear();
    }
}
