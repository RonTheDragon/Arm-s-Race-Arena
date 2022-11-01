using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Heath : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    
    [HideInInspector] public PhotonView PV;
    [HideInInspector] public Player AttackingPlayer;
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
        }

        if (TakeDamageCooldown > 0)
        {
            TakeDamageCooldown -= Time.deltaTime;
        }
        else
        {
            if (StoredDamage > 0 && AttackingPlayer != null)
            {
                PV.RPC("TakeDamage", RpcTarget.All, StoredDamage, AttackingPlayer.ActorNumber);
                StoredDamage = 0;
            }
        }
    }
    [PunRPC]
    void TakeDamage(float Damage, int PlayerId)
    {
        AttackingPlayer = PhotonNetwork.CurrentRoom.GetPlayer(PlayerId);

        Hp -= Damage;
        if (Hp <= 0 && !AlreadyDead)
        {
            AlreadyDead = true;
            if (PV.IsMine)
            {
                Death();
            }
        }
    }

    public void TakingDamage(float Damage, int id)
    {
        AttackingPlayer = PhotonNetwork.CurrentRoom.GetPlayer(id);
        if (TakeDamageCooldown > 0)
        {
            StoredDamage += Damage;
        }
        else
        {
            StoredDamage = Damage;
            TakeDamageCooldown = 0.01f;
        }
    }

    void Death()
    {
        if (AttackingPlayer != null)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Kills", (int)AttackingPlayer.CustomProperties["Kills"] + 1);
            AttackingPlayer.SetCustomProperties(hash);
        }

        GameManager.Instance.SpawnPlayer();
        PhotonNetwork.Destroy(gameObject);
    }
}
