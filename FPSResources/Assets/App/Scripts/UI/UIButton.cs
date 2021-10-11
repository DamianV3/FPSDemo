using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public System.Action onPressed;
    public System.Action onReleased;

    bool m_bIsClicked = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        m_bIsClicked = true;
        if (onPressed != null) onPressed.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!m_bIsClicked) return;
        if (onReleased != null) onReleased.Invoke();
        m_bIsClicked = false;
    }

}
