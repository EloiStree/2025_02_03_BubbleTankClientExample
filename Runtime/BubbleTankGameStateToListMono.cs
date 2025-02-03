using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BubbleTankGameStateToListMono : MonoBehaviour
{

    public UnityEvent m_onPlayerStateInGameParsed;
    public UnityEvent<STRUCT_IntegerPlayerInGameInfo> m_onPlayerInfoReceived;
    public List<STRUCT_IntegerPlayerInGameInfo> m_playerStateInGames = new List<STRUCT_IntegerPlayerInGameInfo>();


    public void PushInBytesFromWeb(byte [] bytesReceived){

        string text = System.Text.Encoding.UTF8.GetString(bytesReceived);
        PushInAsUtf8TextFromWeb(text);
    }
    public void PushInAsUtf8TextFromWeb(string textReceived){
 
        
        List<STRUCT_IntegerPlayerInGameInfo> playerStateInGames = new List<STRUCT_IntegerPlayerInGameInfo>();
        //`PLAYER_ID:PLAYER_LOBBY_ID:TEAM_ID:POSITION_X:POSITION_Y:POSITIONZ:ROTATION_X:ROTATION_Y:ROTATION_Z:SCALE:FLAT_XZ_ANGLE`

        string [] lines = textReceived.Split('\n');
        foreach (var line in lines)
        {
            string [] values = line.Trim().Split(':');
            if (values.Length==11)
            {
                if( ! int.TryParse(values[0],out int playerIndex))
                {
                    continue;
                }

                STRUCT_IntegerPlayerInGameInfo playerStateInGame = new STRUCT_IntegerPlayerInGameInfo();
                playerStateInGame.m_playerIndex = int.Parse(values[0]);
                playerStateInGame.m_playerNetworkIndex = int.Parse(values[1]);
                playerStateInGame.m_playerTeamIndex = int.Parse(values[2]);
                float xfromMM = int.Parse(values[3])/1000f;
                float yfromMM = int.Parse(values[4])/1000f;
                float zfromMM = int.Parse(values[5])/1000f;
                float xEuleurFromMM= int.Parse(values[6])/1000f;
                float yEuleurFromMM= int.Parse(values[7])/1000f;
                float zEuleurFromMM= int.Parse(values[8])/1000f;
                float scaleFromMM= int.Parse(values[9])/1000f;
                float flatAngleXZFromMM= int.Parse(values[10])/1000f;
                playerStateInGame.m_position = new Vector3(xfromMM,yfromMM,zfromMM);
                playerStateInGame.m_rotationEuler = new Vector3(xEuleurFromMM,yEuleurFromMM,zEuleurFromMM);
                playerStateInGame.m_playerScale = (int)scaleFromMM;
                playerStateInGame.m_flatAngleXZ = (int)flatAngleXZFromMM;
                long timeInSecondsSince1970UTC = (System.DateTime.UtcNow.Ticks - new System.DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond;
                playerStateInGame.m_timestampReceived = timeInSecondsSince1970UTC;
                playerStateInGames.Add(playerStateInGame);
                m_onPlayerInfoReceived.Invoke(playerStateInGame);
            }
        }

        m_playerStateInGames = playerStateInGames;
        m_onPlayerStateInGameParsed.Invoke();
    }
}

    [System.Serializable]
    public struct STRUCT_IntegerPlayerInGameInfo{
        public int m_playerIndex;
        public int m_playerNetworkIndex;
                
        public int m_playerTeamIndex;
        public Vector3 m_position;
        public Vector3 m_rotationEuler;
        public float m_playerScale;
        public float m_flatAngleXZ;
        public long m_timestampReceived;
    }
