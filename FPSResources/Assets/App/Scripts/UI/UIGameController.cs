using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class UIGameController : Singleton<UIGameController>
{
    [SerializeField] UIJoystick joystick;
    [SerializeField] UIButton ButtonShot;
    [SerializeField] UIRotateDrag rotateDrag;
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] Text lostTesx;
    [SerializeField] Image screenFader;


    bool isInit = false;

    bool m_shotPressed = false;
    Vector2 m_move;
    Vector2 m_rotate;
    
    public bool IsShot { get { return m_shotPressed; } }
    public Vector2 Movement { get { return m_move; } }
    public Vector2 Rotate { get { return m_rotate; } }

    private void Start()
    {
        init();    
    }

    void Update()
    {
        if(joystick)
        {
            m_move = joystick.value;
        }
        if (rotateDrag) 
        { 
            m_rotate = rotateDrag.value;
        }
    }

    private void init()
    {
        if (isInit) return;
        if(ButtonShot != null)
        {
            ButtonShot.onPressed = onButtonPressed;
            ButtonShot.onReleased = onButtonReleased;
        }
        isInit = true;
        SetLose(false);
    }

    private void onButtonPressed()
    {
        m_shotPressed = true;
    }

    private void onButtonReleased()
    {
        m_shotPressed = false;
    }

    public void SetScore(int value)
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + value.ToString();
        }
    }

    public void SetTime(float value)
    {
        if (timeText)
        {
            timeText.text = value.ToString("0.##");
        }
    }

    public void SetFade(bool active, float delay)
    {
        StartCoroutine(doFade(active, delay));
    }

    IEnumerator doFade(bool active, float delay)
    {
        if (screenFader == null) yield break;
        screenFader.gameObject.SetActive(true);
        screenFader.CrossFadeAlpha(active ? 1f : 0f, delay, true);
        yield return new WaitForSeconds(delay);
        if(!active)
            screenFader.gameObject.SetActive(active);
    }

    public void SetLose(bool active)
    {
        if(lostTesx != null)
        {
            lostTesx.gameObject.SetActive(active);
        }
    }
}
