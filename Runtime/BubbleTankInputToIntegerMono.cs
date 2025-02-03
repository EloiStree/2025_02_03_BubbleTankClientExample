using System;
using UnityEngine;
using UnityEngine.Events;

public class BubbleTankInputToIntegerMono : MonoBehaviour
{
    public UnityEvent<int,int> m_onPlayerIntegerPush;
    public int m_lastPushed;

    public int m_playerIndex;
    [Range(-1,1)]
    public float m_horizontalAxis;
    [Range(-1,1)]
    public float m_verticalAxis;

    public int m_gamepadState;

    public void Update()
    {
        PushNewGamepadValue();
    }
    

    public void SetPlayerIndex(int playerIndex){
        m_playerIndex=playerIndex;
    }

    public void SetHorizontalAxis(float horizontalAxis){
        m_horizontalAxis=horizontalAxis;
    }


    public void SetVerticalAxis(float verticalAxis){
        m_verticalAxis=verticalAxis;
    }

    private void PushInteger(int value)
    {
        m_lastPushed=value;
        m_onPlayerIntegerPush.Invoke(m_playerIndex,value);
    }

    [ContextMenu("Push Gamepad Value")]
    private void PushNewGamepadValue()
    {
        int value =1800000000;
        ParsePercent11To99(m_horizontalAxis,out int valueHoriziontal);
        ParsePercent11To99(m_verticalAxis,out int valueVertical);
        value+= valueVertical;
        value+= valueHoriziontal*100;
        value+= valueVertical*10000;
        value+= valueHoriziontal*1000000;
        if (m_gamepadState!= value)
        {
            m_gamepadState=value;
            PushInteger(value);
        }
    }

    private void ParsePercent11To99(float percent11, out int value99)
    {
        if (percent11==0)
        {
            value99=0;
            return;
        }
        percent11+=1f;
        percent11/=2f;
        percent11*=98f;
        value99=1+(int)(Mathf.Clamp(percent11,0f,98f));

    }



    #region  FIRE AND PING
    public int m_fireKeyCode =1032;
    public int m_pingKeyCode=1080;
    
    public void Fire(){
        StartFiring();
        StopFiring();
    }
    public void StartFiring(){ 

        PushInteger(m_fireKeyCode);
    }
    public void StopFiring(){ 

        PushInteger(m_fireKeyCode+1000);
    }

 
    public void Ping(){
        StartPing();
        StopPing();
    }
    public void StartPing(){ 

        PushInteger(m_pingKeyCode);
    }
    public void StopPing(){ 

        PushInteger(m_pingKeyCode+1000);
    }
#endregion


}
