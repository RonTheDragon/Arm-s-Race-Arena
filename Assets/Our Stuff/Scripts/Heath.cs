using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Heath : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    [HideInInspector] public PhotonView PV;
    [HideInInspector] public PhotonView Attacker;
    PlayerManager PM;
    [HideInInspector] public bool AlreadyDead;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        Hp = MaxHp;
        PM = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0 && !AlreadyDead)
        {
            AlreadyDead = true;
            if (PV.IsMine)
            {
                Death();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
        [PunRPC]
        void TakeDamage(float Damage,int DamagerId)
        {
            Hp -= Damage;
            Attacker = PhotonNetwork.GetPhotonView(DamagerId);
        }

        void Death()
        {
        if (Attacker != null)
            Attacker.GetComponent<PlayerManager>().AddKill();
            GameManager.Instance.SpawnPlayer();
            PhotonNetwork.Destroy(gameObject);
        }
}
