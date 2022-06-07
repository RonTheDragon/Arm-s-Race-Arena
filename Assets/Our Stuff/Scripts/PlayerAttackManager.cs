using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAttackManager : MonoBehaviour
{
    [SerializeField] Camera TheCamera;
    [SerializeField] Transform Hand;
    PhotonView PV;
    int SelectedGun;

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
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                if (Hand.childCount > SelectedGun+1)
                {
                    SelectedGun++;
                }
                else
                {
                    SelectedGun = 0;
                }
                PV.RPC("SwitchWeapon", RpcTarget.All, SelectedGun);
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                if (SelectedGun != 0)
                {
                    SelectedGun--; 
                }
                else
                {
                    SelectedGun = Hand.childCount-1;
                }
                PV.RPC("SwitchWeapon", RpcTarget.All,SelectedGun);
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

    [PunRPC]
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
}
