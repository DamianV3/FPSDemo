using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRotateDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] float m_dragClamp = 10;
    RectTransform m_rect;
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
        m_rect = this.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_delta.x = eventData.position.x;
        m_delta.y = eventData.position.y;
        m_value.x = 0;
        m_value.y = 0;
        m_bIsDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!m_bIsDrag) return;
        Vector2 point = new Vector2(eventData.position.x, eventData.position.y);
        var offset = point - m_delta;
        float distance = Mathf.Clamp(offset.magnitude / m_dragClamp, 0, 1);
        offset = offset.normalized * distance;
        setDragPosition(offset.x, offset.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_bIsDrag) return;
        m_bIsDrag = false;
        setDragPosition(0, 0);
    }

    void setDragPosition(float x, float y)
    {
        m_value.x = x;
        m_value.y = y;
    }
}
