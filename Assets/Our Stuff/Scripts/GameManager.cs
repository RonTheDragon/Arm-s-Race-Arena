using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;
    public List<Material> colors;
    [HideInInspector] public Transform Checkpoints;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    private void Awake()
    {
        Instance = this;
        Checkpoints = transform.GetChild(0);
        SpawnPlayer(null);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer(Material material)
    {
        int n = Random.Range(0, Checkpoints.childCount);
        GameObject Player = PhotonNetwork.Instantiate("Player", Checkpoints.GetChild(n).position, Quaternion.identity);
        PlayerManager PM = Player.GetComponent<PlayerManager>();
        if (material == null)
        {
            if (colors.Count> PhotonNetwork.CountOfPlayers)
            PM.Color = colors[PhotonNetwork.CountOfPlayers];
        }
        else
        {
            PM.Color = material;
        }
    }
}
