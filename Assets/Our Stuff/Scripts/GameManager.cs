using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public static GameManager Instance;
    public Camera TheCamera;
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
        SpawnPlayer();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        int n = Random.Range(0, Checkpoints.childCount);
        PhotonNetwork.Instantiate("Player", Checkpoints.GetChild(n).position, Quaternion.identity);
    }
}
