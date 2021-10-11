using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform m_handle;
    [SerializeField, Range(0, 1f)] float m_maxOffset = 0.7f;

    RectTransform m_rect;
    float m_max;
    float m_distance;
    bool m_bIsDrag;
    Vector2 m_delta = new Vector2();
    Vector2 m_value = new Vector2();

    public Vector2 value { get { return m_value; } }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    private void init()
    {
        if (m_handle == null) return;
        m_rect = this.GetComponent<RectTransform>();
        m_max = m_rect.sizeDelta.x > m_rect.sizeDelta.y ? m_rect.sizeDelta.y : m_rect.sizeDelta.x;
        m_max *= 0.5f * m_maxOffset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_delta.x = eventData.position.x;
        m_delta.y = eventData.position.y;
        m_bIsDrag = true;
        m_distance = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_handle == null || !m_bIsDrag) return;
        Vector2 point = new Vector2(eventData.position.x, eventData.position.y);
        float dx = point.x - m_delta.x;
        float dy = point.y - m_delta.y;
        m_distance = Mathf.Sqrt(dx * dx + dy * dy);
        var currentPos = m_handle.anchoredPosition;
        currentPos.x += dx;
        currentPos.y += dy;
        float len = Vector2.Distance(currentPos, Vector2.zero);
        Vector2 spin = currentPos.normalized;
        if (len > m_max)
        {
            currentPos = currentPos.normalized * m_max;
        }
        else
        {
            spin *= (len / m_max);
        }
        m_handle.anchoredPosition = currentPos;
        setDragPosition(spin.x, spin.y);
        m_delta.x = point.x;
        m_delta.y = point.y;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_bIsDrag) return;
        m_bIsDrag = false;
        if (m_handle) m_handle.anchoredPosition = Vector2.zero;
        setDragPosition(0, 0);
    }

    void setDragPosition(float x, float y)
    {
        m_value.x = x;
        m_value.y = y;
    }
}
