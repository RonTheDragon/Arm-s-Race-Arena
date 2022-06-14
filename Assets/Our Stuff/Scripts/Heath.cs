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

    float TakeDamageCooldown;
    float StoredDamage;

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

        if (TakeDamageCooldown > 0)
        {
            TakeDamageCooldown -= Time.deltaTime;
        }
        else
        {
            if (StoredDamage > 0)
            {
                PV.RPC("TakeDamage", RpcTarget.All, StoredDamage, Attacker.ViewID);
                StoredDamage = 0;
            }
        }
    }
        [PunRPC]
        void TakeDamage(float Damage,int DamagerId)
        {
            Hp -= Damage;
            Attacker = PhotonNetwork.GetPhotonView(DamagerId);
        }

   public void TakingDamage(float Damage, int id)
    {
        if (TakeDamageCooldown > 0)
        {
            StoredDamage += Damage;
        }
        else
        {
            Attacker = PhotonNetwork.GetPhotonView(id);
            StoredDamage = Damage;
            TakeDamageCooldown = 1;
        }
    }

        void Death()
        {
        if (Attacker != null)
            Attacker.GetComponent<PlayerManager>().AddKill();
            GameManager.Instance.SpawnPlayer();
            PhotonNetwork.Destroy(gameObject);
        }
}
