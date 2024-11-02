using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIParam { }

public class UIActiveEffectParam : UIParam
{
    public DirectionType dir;
    public RectTransform targetRectTr;
    public Image fadeImage;

    public UIActiveEffectParam(DirectionType dir, RectTransform targetRectTr, Image fadeImage)
    {
        this.dir = dir;
        this.targetRectTr = targetRectTr;
        this.fadeImage = fadeImage;
    }
}

public class BaseCanvasUI : InitBase
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;

    [SerializeField] UIEffect[] uIEffects;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        canvas = this.GetOrAddComponent<Canvas>();
        canvasScaler = this.GetOrAddComponent<CanvasScaler>();

        this.gameObject.layer = LayerMask.NameToLayer("UI");

        SetCanvas();
        SetCanvasScaler();
        SetResolution();

        return true;
    }

    private void SetCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.overrideSorting = true;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 0;

        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
        canvas.vertexColorAlwaysGammaSpace = true;
    }

    protected void SetCanvasScaler()
    {
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = 100;
        canvasScaler.matchWidthOrHeight = 1.0f;
    }

    public void SetResolution()
    {
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }

        OnPreCull();
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    protected void SetSortingOrder(int order)
    {
        canvas.sortingOrder = order;
    }
}

