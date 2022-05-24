using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerButton : MonoBehaviour
{
    public string ServerName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinMe()
    {
        if (PhotonScript.instance.nick != string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = PhotonScript.instance.nick;
            PhotonNetwork.JoinRoom(ServerName);
        }
    }
}
