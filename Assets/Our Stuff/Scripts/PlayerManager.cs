using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviourPun
{
    [SerializeField] GameObject MyCanvas;
    [SerializeField] TMP_Text MyKills;
    [SerializeField] Image MyHpBar;
    [SerializeField] GameObject TheirCanvas;
    [SerializeField] TMP_Text PlayerName;
    [SerializeField] TMP_Text ShowThemKills;
    [SerializeField] Image HpBar;
    [SerializeField] GameObject TheCamera;
    [SerializeField] Renderer mesh;
    PhotonView view;
    bool firsttime = true;
    Heath hp;
    public int Kills;
    void Start()
    {
        TheCamera = transform.GetChild(0).gameObject;
        hp = GetComponent<Heath>();
    }

    // Update is called once per frame
    void Update()
    {
        if(firsttime)
        {
            view = GetComponent<PhotonView>();
            firsttime = false;
            TheCamera.GetComponent<Zoom>().enabled = false;
            if (!view.IsMine)
            {
                MyCanvas.SetActive(false);
                TheirCanvas.SetActive(true);
                GetComponent<FirstPersonMovement>().enabled = false;
                GetComponent<Jump>().enabled = false;
                GetComponent<Crouch>().enabled = false;
                TheCamera.GetComponent<Camera>().enabled = false;
                TheCamera.GetComponent<FirstPersonLook>().enabled = false;
                PlayerName.text = view.Owner.NickName;
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else
            {
                MyCanvas.SetActive(true);
                TheirCanvas.SetActive(false);
                TheCamera.GetComponent<AudioListener>().enabled = true;
                GameManager.Instance.TheCamera = TheCamera.GetComponent<Camera>();
            }
            ShowThemKills.text = $"Kills: {Kills}";
            MyKills.text = $"Kills: {Kills}";
            if (GameManager.Instance.colors.Count> view.Owner.ActorNumber)
            mesh.materials[0].color = GameManager.Instance.colors[view.Owner.ActorNumber].color;
        }

        if (!view.IsMine && GameManager.Instance.TheCamera != null)
        {
            TheirCanvas.transform.LookAt(GameManager.Instance.TheCamera.transform.position);
            HpBar.fillAmount = hp.Hp / hp.MaxHp;
        }
        if (view.IsMine)
        {
            MyHpBar.fillAmount = hp.Hp / hp.MaxHp;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }


    [PunRPC]
    public void AddKill(int kills)
    {
        Kills+= kills;
        ShowThemKills.text = $"Kills: {Kills}";
        MyKills.text = $"Kills: {Kills}";
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }
}
