using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject TheCamera;
    public Material Color;
    [SerializeField] Renderer mesh;
    PhotonView view;
    bool firsttime = true;
    void Start()
    {
        TheCamera = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(firsttime)
        {
            view = GetComponent<PhotonView>();
            firsttime = false;
            TheCamera.GetComponent<Zoom>().enabled = false;
            mesh.materials[0].color = Color.color;
            if (!view.IsMine)
            {
                Canvas.SetActive(false);
                GetComponent<FirstPersonMovement>().enabled = false;
                GetComponent<Jump>().enabled = false;
                GetComponent<Crouch>().enabled = false;
                TheCamera.GetComponent<Camera>().enabled = false;
                TheCamera.GetComponent<FirstPersonLook>().enabled = false;
            }
            else
            {
                TheCamera.GetComponent<AudioListener>().enabled = true;
            }
        }

    }
}
