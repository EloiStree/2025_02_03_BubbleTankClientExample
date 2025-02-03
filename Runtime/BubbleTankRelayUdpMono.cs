using System.Net.Sockets;
using UnityEngine;

public class BubbleTankRelayUdpMono : MonoBehaviour
{


    public TargetIvp4 [] m_targetIvp4s;

    [System.Serializable]
    public class TargetIvp4{
        public string m_ip="127.0.0.1";
        public int m_port=2504;
    }
    
    public void PushIndexInteger(int index, int integer){

        UdpClient udpClient = new UdpClient();
        byte[] bytesToSend = new byte[8];
        bytesToSend[0] = (byte)(index);
        bytesToSend[1] = (byte)(index >> 8);
        bytesToSend[2] = (byte)(index >> 16);
        bytesToSend[3] = (byte)(index >> 24);
        bytesToSend[4] = (byte)(integer);
        bytesToSend[5] = (byte)(integer >> 8);
        bytesToSend[6] = (byte)(integer >> 16);
        bytesToSend[7] = (byte)(integer >> 24);

        foreach (var target in m_targetIvp4s)
        {
            udpClient.Send(bytesToSend, bytesToSend.Length, target.m_ip, target.m_port);
        }
        udpClient.Close();


    }
}
