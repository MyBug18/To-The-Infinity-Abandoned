using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PPC_ColorPicker_SV : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PPC_ColorPicker colorPicker;

    private RectTransform m_rectTrans;
    private RectTransform rectTrans
    {
        get
        {
            if (m_rectTrans == null)
                m_rectTrans = GetComponent<RectTransform>();
            return m_rectTrans;
        }
    }

    private RectTransform m_indicatorRectTrans;
    private RectTransform indicatorRectTrans
    {
        get
        {
            if (m_indicatorRectTrans == null)
                m_indicatorRectTrans = transform.GetChild(0).GetComponent<RectTransform>();
            return m_indicatorRectTrans;
        }
    }

    private bool mouseDown;

    private void Start()
    {
        colorPicker = GetComponentInParent<PPC_ColorPicker>();
    }

    private void Update()
    {
        if (mouseDown)
            colorPicker.CalculateColor(rectTrans, PPC_ColorPicker.ColorComponent.SV);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        mouseDown = false;
    }
}
