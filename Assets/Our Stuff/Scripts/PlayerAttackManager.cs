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
    PhotonView PV;
    int SelectedGun;
    bool unlockpistol = false;
    bool unlocksniper = false;
    bool won = false;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine) 
        {
            if (Input.GetMouseButton(0))
            {
                PV.RPC("Shoot", RpcTarget.All);
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
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.LoadLevel(1);
            }
        }
    }

    [PunRPC]
    void Shoot()
    {
        Vector3 AimAt = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(TheCamera.transform.position, TheCamera.transform.forward, out hit))
        { AimAt = hit.point; }
        Hand.GetChild(SelectedGun).GetComponent<Gun>().Shoot(AimAt);
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
