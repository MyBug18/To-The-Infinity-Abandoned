using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPC_ColorPicker : MonoBehaviour
{
    private Camera m_cam;
    private Camera cam
    {
        get
        {
            if (m_cam == null)
                m_cam = FindObjectOfType<Camera>();

            return m_cam;
        }
    }
    public enum ColorComponent { H, SV }

    private RectTransform SVRectTrans, HRectTrans;
    private RectTransform SVIndicator, HIndicator;

    public Color selectedColor = Color.black;

    public PPC_ColorUnityEvent OnColorChange;

    private void Start()
    {
        SVRectTrans = transform.Find("SV").GetComponent<RectTransform>();
        HRectTrans = transform.Find("H").GetComponent<RectTransform>();
        SVIndicator = SVRectTrans.transform.GetChild(0).GetComponent<RectTransform>();
        HIndicator = HRectTrans.transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void CalculateColor(RectTransform _rectTrans, ColorComponent _colorComponent)
    {
        Vector2 localpoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTrans, Input.mousePosition, cam, out localpoint);

        switch (_colorComponent)
        {
            case ColorComponent.H:
                SetHIndicatorPosition(localpoint.y, false);
                H2RGB(Set01RelativePosition(localpoint, _rectTrans).y, true);
                break;
            case ColorComponent.SV:
                SetSVIndicatorPosition(localpoint, false);
                SV2RGB(Set01RelativePosition(localpoint, _rectTrans));
                break;
        }
    }

    private void SetHIndicatorPosition(float _localPositionY, bool _01relative)
    {
        if (HIndicator == null)
            return;

        HIndicator.localPosition = new Vector3(
            HIndicator.localPosition.x,
            _01relative ?
                Mathf.Lerp(HRectTrans.rect.yMin, HRectTrans.rect.yMax, _localPositionY) :
                Mathf.Clamp(_localPositionY, HRectTrans.rect.yMin, HRectTrans.rect.yMax),
            HIndicator.localPosition.z);
    }

    private void SetSVIndicatorPosition(Vector2 _localPosition, bool _01relative)
    {
        if (SVIndicator == null)
            return;

        if (_01relative)
        {
            SVIndicator.localPosition = new Vector3(
                Mathf.Lerp(SVRectTrans.rect.xMin, SVRectTrans.rect.xMax, _localPosition.x) - SVIndicator.rect.width * 0.5f,
                Mathf.Lerp(SVRectTrans.rect.yMin, SVRectTrans.rect.yMax, _localPosition.y) - SVIndicator.rect.height * 0.5f,
                SVIndicator.localPosition.z
                );
        }
        else
        {
            SVIndicator.localPosition = new Vector3(
                Mathf.Clamp(_localPosition.x, SVRectTrans.rect.xMin, SVRectTrans.rect.xMax) - SVIndicator.rect.width * 0.5f,
                Mathf.Clamp(_localPosition.y, SVRectTrans.rect.yMin, SVRectTrans.rect.yMax) - SVIndicator.rect.height * 0.5f,
                SVIndicator.localPosition.z
                );
        }
    }

    private Vector2 Set01RelativePosition(Vector2 _localPosition, RectTransform _rectTrans)
    {
        return new Vector2(
            Mathf.Clamp01((_localPosition.x - _rectTrans.rect.xMin) / _rectTrans.rect.width),
            Mathf.Clamp01((_localPosition.y - _rectTrans.rect.yMin) / _rectTrans.rect.height)
            );
    }

    private void SV2RGB(Vector2 _01position)
    {
        if (SVIndicator == null)
            return;

        Color col = Color.Lerp(
            Color.black,
            Color.Lerp(
                Color.white,
                SVRectTrans.GetComponent<Image>().color,
                _01position.x),
            _01position.y);

        SVIndicator.GetComponent<Image>().color = col;

        selectedColor = col;
        OnColorChange.Invoke(col);
    }
    private void H2RGB(float _localPosY, bool _calculateNewColor)
    {
        if (HIndicator == null || SVIndicator == null)
            return;

        Color col = Color.Lerp(
                        Color.Lerp(
                            new Color(1, 0, 0, 1),
                            Color.Lerp(
                                new Color(1, 1, 0, 1),
                                Color.Lerp(
                                    new Color(0, 1, 0, 1),
                                    Color.Lerp(
                                        new Color(0, 1, 1, 1),
                                        Color.Lerp(
                                            new Color(0, 0, 1, 1),
                                            new Color(1, 0, 1, 1),
                                            Mathf.Clamp01(_localPosY * 6 - 4)
                                        ),
                                        Mathf.Clamp01(_localPosY * 6 - 3)
                                    ),
                                    Mathf.Clamp01(_localPosY * 6 - 2)
                                ),
                                Mathf.Clamp01(_localPosY * 6 - 1)
                            ),
                            Mathf.Clamp01(_localPosY * 6)
                        ),
                        new Color(1, 0, 0, 1),
                    Mathf.Clamp01(_localPosY * 6 - 5));

        SVRectTrans.GetComponent<Image>().color = col;
        HIndicator.GetComponent<Image>().color = col;

        if (_calculateNewColor)
            SV2RGB(Set01RelativePosition(SVIndicator.localPosition, SVRectTrans));
    }

    public void RGB2HSV(Color _color)
    {
        float max = Mathf.Max(_color.r, Mathf.Max(_color.g, _color.b));
        float min = Mathf.Min(_color.r, Mathf.Min(_color.g, _color.b));

        float hue;
        float saturation;
        float luminance = max;

        if (max == min)
        {
            hue = 0;
            saturation = 0;
        }
        else
        {
            if (_color.r == max)
                hue = (_color.g - _color.b) / (max - min);
            else if (_color.g == max)
                hue = 2 + (_color.b - _color.r) / (max - min);
            else
                hue = 4 + (_color.r - _color.g) / (max - min);

            if (hue < 0)
                hue += 6;
            hue /= 6f;

            saturation = (max - min) / max;
        }

        SetHIndicatorPosition(hue, true);
        H2RGB(hue, false);
        
        SetSVIndicatorPosition(new Vector2(saturation, luminance), true);
        SV2RGB(new Vector2(saturation, luminance));
    }
}
