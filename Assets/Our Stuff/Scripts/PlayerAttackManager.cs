using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class PlayerAttackManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Camera TheCamera;
    [SerializeField] Transform Hand;
    int RememberKills;
    PhotonView PV;
    int SelectedGun;
    bool unlockpistol = false;
    bool unlocksniper = false;
    bool won = false;
    bool Shooting;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        KillsRemembered(PV.Owner);
    }

    // Update is called once per frame
    void Update()
    {
        if (Shooting)
        {
            Shoot();
        }
        if (PV.IsMine) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                PV.RPC("StartShoot", RpcTarget.All);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PV.RPC("StopShoot", RpcTarget.All);
            }
            //if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            //{
            //    if (Hand.childCount > SelectedGun+1)
            //    {
            //        SelectedGun++;
            //    }
            //    else
            //    {
            //        SelectedGun = 0;
            //    }
            //    Switching();
            //}
            //else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            //{
            //    if (SelectedGun != 0)
            //    {
            //        SelectedGun--; 
            //    }
            //    else
            //    {
            //        SelectedGun = Hand.childCount-1;
            //    }
            //    Switching();
            //}
            if (PV.Owner.CustomProperties.ContainsKey("Kills"))
            {
                if ((int)PV.Owner.CustomProperties["Kills"] >= 2 && !unlockpistol)
                {
                    unlockpistol = true;
                    SelectedGun++;
                    Switching();
                }
                if ((int)PV.Owner.CustomProperties["Kills"] >= 5 && !unlocksniper)
                {
                    unlocksniper = true;
                    SelectedGun++;
                    Switching();
                }
                if ((int)PV.Owner.CustomProperties["Kills"] >= 8 && !won)
                {
                    won = true;
                    PV.RPC("EndGame", RpcTarget.All);
                }
            }
        }
    }
    [PunRPC]
    void EndGame()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(1);
        }
    }

    [PunRPC]
    void StartShoot()
    {
        Shooting = true;
    }

    [PunRPC]
    void StopShoot()
    {
        Shooting = false;
    }

    void Shoot()
    {
        Vector3 AimAt = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(TheCamera.transform.position, TheCamera.transform.forward, out hit))
        { AimAt = hit.point; }
        Hand.GetChild(SelectedGun).GetComponent<Gun>().Shoot(AimAt);
    }

    public void KillsRemembered(Player p)
    {
        if (p.CustomProperties.ContainsKey("Kills"))
        {
            if (RememberKills <= (int)p.CustomProperties["Kills"])
            {
                RememberKills = (int)p.CustomProperties["Kills"];
            }
            else
            {
                Hashtable hash = new Hashtable();
                hash.Add("Kills", RememberKills);
                p.SetCustomProperties(hash);
            }
        }
    }

    void Switching()
    {
        Hashtable hash = new Hashtable();
        hash.Add("Gun", SelectedGun);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        SwitchWeapon(SelectedGun);
    }

    void SwitchWeapon(int WeaponSelected)
    {
        if (!PV.IsMine)
        {
            SelectedGun = WeaponSelected;
        }
        foreach(Transform h in Hand)
        {
            h.gameObject.SetActive(false);
        }
        Hand.GetChild(WeaponSelected).gameObject.SetActive(true);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner && changedProps.ContainsKey("Gun"))
        {
            SwitchWeapon((int)changedProps["Gun"]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Switching();
    }
}
