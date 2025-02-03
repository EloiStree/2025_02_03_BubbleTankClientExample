using UnityEngine;

public class BubbleTankTrackPlayerInfoMono : MonoBehaviour
{

    public TrackUser [] m_trackUsers;

    [System.Serializable]
    public class TrackUser{

        public int m_playerId;
        public STRUCT_IntegerPlayerInGameInfo m_playerInfo;
    }

    public void PushInPlayerInfo(STRUCT_IntegerPlayerInGameInfo info)
    {
        foreach (var trackUser in m_trackUsers)
        {
            if (trackUser.m_playerId==info.m_playerIndex)
            {
                trackUser.m_playerInfo=info;
            }
        }
    }
}