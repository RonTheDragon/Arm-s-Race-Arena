using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;

public class PhotonScript : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    bool isConnecting;
    bool refreshing = true;

    string RoomName;
    string nick;
    string serverlist;

    public TMP_Text ServerList;

    public List<RoomInfo> TheRoomList;
    public GameObject Canvas;
    

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Canvas.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        nick = string.Empty;
        Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Connect()
    {
        isConnecting = PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
    }
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("Connected To Photon");
            //PhotonNetwork.JoinRandomOrCreateRoom();
            isConnecting = false;
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Canvas.SetActive(true);
    }

    public void EditName(string N)
    {
        RoomName = N;
    }

    public void EditNick(string N)
    {
        nick = N;
    }

    public void MYJoin()
    {
        if (nick != string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = nick;
            PhotonNetwork.JoinRoom(RoomName);
        }
    }

    public void MYCreate()
    {
        if (RoomName != string.Empty && nick != string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = nick;
            PhotonNetwork.CreateRoom(RoomName);
        }

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        serverlist = string.Empty;
        foreach (RoomInfo a in roomList)
        {
            serverlist += $"{a.Name}"+ Environment.NewLine;
        }
        if (refreshing)
        {
            ServerList.text = serverlist;
            refreshing = false;
        }
    }

    

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        MYRefresh();
    }

    public void MYRefresh()
    {
        if (PhotonNetwork.InLobby == true)
        {
            refreshing = true;
            PhotonNetwork.LeaveLobby();
        }
    }
    public override void OnLeftLobby()
    {
        if (refreshing)
        {
            PhotonNetwork.JoinLobby();
        }
    }


}
