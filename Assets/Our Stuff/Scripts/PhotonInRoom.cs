using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PhotonInRoom : MonoBehaviourPunCallbacks
{
    public TMP_Text ServerName;
    public TMP_Text HowManyPlayers;
    public TMP_Text PlayerNames;
    public GameObject StartButton;
    // Start is called before the first frame update
    void Start()
    {
        ServerName.text = PhotonNetwork.CurrentRoom.Name;
        setPlayerCount();
        CheckIfOwner();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        setPlayerCount();
        CheckIfOwner();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        setPlayerCount();
        CheckIfOwner();
    }

    void setPlayerCount()
    {
        HowManyPlayers.text = $"Currently {PhotonNetwork.CurrentRoom.PlayerCount} in Room";

        string AllPlayerNames = string.Empty;

        foreach(Player p in PhotonNetwork.PlayerList)
        {
            AllPlayerNames += p.NickName + Environment.NewLine;
        }
               
        PlayerNames.text = AllPlayerNames;
    }
    public void MYLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void MYStartGame()
    {
        PhotonNetwork.LoadLevel(2);
    }
    void CheckIfOwner()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }
}
