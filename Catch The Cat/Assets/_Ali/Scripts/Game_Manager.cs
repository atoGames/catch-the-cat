using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameMode {
    MAIN_MENU,
    PLAY_MODE
}

public class Game_Manager : MonoBehaviour {
    public static Game_Manager _Instace;

    [HideInInspector]
    public Spawn_Manager _Spawn_Manager;

    public GameMode mGame = GameMode.MAIN_MENU;

    [SerializeField]
    private AudioSource audioClip_Play;
    [SerializeField]
    private AudioSource audioClip_GameStart;

    public Animator AnimPlay;
    public GameObject goDelogShow;
    public GameObject goMainMenu;
    public GameObject goTutorial; // Tutorial
    public GameObject goBackGround;
    public GameObject goWin;
    public GameObject goGameOver;

    [Header (" -- UI --  ")]
    public TextMeshProUGUI m_Timer;
    public TextMeshProUGUI m_CountCat;
    public TextMeshProUGUI m_Medicine;

    public float _Timer;
    private float d_Timer = 120;

    [HideInInspector]
    public int _CountCat;
    public int _MaxCountCat = 10;

    [HideInInspector]
    public int _Medicine;
    public bool useMedicine;

    // We Set This -- if Trigger Run
    [HideInInspector]
    public GameObject currntCat;

    // Start is called before the first frame update
    void Awake () {
        if (_Instace == null) _Instace = this;

        if (_Spawn_Manager == null)
            _Spawn_Manager = GetComponent<Spawn_Manager> ();

        _Medicine = 5;
        _Timer = d_Timer;
        useMedicine = false;

    }
    // Update is called once per frame
    void Update () {
        if (mGame == GameMode.PLAY_MODE) {
            if (AnimPlay.GetBool ("isPlay"))
                Invoke ("InvAnimPlay", .5f);

            UI_Player ();

            if (useMedicine)
                Invoke ("RestTimerMedicine", 0.01f);
        }
    }
    void InvAnimPlay () {
        Debug.Log (" InvAnimPlay ");
        goBackGround.SetActive (false);
        AnimPlay.SetBool ("isPlay", false);
    }
    void RestTimerMedicine () {
        useMedicine = false;
    }

    private void UI_Player () {
        _Timer -= Time.deltaTime;
        if (_Timer <= 0) {
            ShowDelog_GameOver ();
            _Timer = 0;
        }
        if (_CountCat >= _MaxCountCat) {
            ShowDelog_Win ();
            // _Timer = 0;
        }

        m_Timer.text = this._Timer.ToString ("#.#");
        m_CountCat.text = this._CountCat + "/" + _MaxCountCat; //.ToString(); //;
        m_Medicine.text = this._Medicine.ToString ();
    }
    public void ShowDelog_Win () {
        goWin.SetActive (true);
    }
    public void ShowDelog_GameOver () {
        Debug.Log ("  Game Over ");
        goGameOver.SetActive (true);

        FindObjectOfType<Player> ().m_Rigidbody2D.bodyType = RigidbodyType2D.Static;

        if (mGame == GameMode.PLAY_MODE)
            mGame = GameMode.MAIN_MENU;
    }

    public void btn_DelogOptions (int Index) {
        switch (Index) {
            case 0:
                if (_Medicine > 0 && currntCat != null) {
                    Debug.Log (" we use 1 Medicine ");
                    _Medicine -= 1;
                    useMedicine = true;

                    if (useMedicine == true) {
                        //  Debug.Log(" DelogShowBegin ");
                        _CountCat += 1;
                        _Spawn_Manager.Remove_Cat (currntCat.GetComponent<Cat> ());

                        Destroy (currntCat);
                    }
                    goDelogShow.SetActive (false);

                }
                break;
            case 1:
                goDelogShow.SetActive (false);
                break;
            default:
                Debug.LogError (" -- Bla! ");
                break;
        }
    }

    public void btn_Play () {
        if (mGame == GameMode.MAIN_MENU)
            mGame = GameMode.PLAY_MODE;

        AnimPlay.SetBool ("isPlay", true);
        goMainMenu.SetActive (false);
        goTutorial.SetActive (false);

        audioClip_Play.Play ();
        audioClip_GameStart.Play ();

    }
    public void btn_PlayAgain () {
        Debug.Log (" Play Again ");
        _Timer = d_Timer;

        _Medicine = 5;
        _CountCat = 0;
        FindObjectOfType<Player> ().m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;

        goGameOver.SetActive (false);

        if (mGame == GameMode.MAIN_MENU)
            mGame = GameMode.PLAY_MODE;
    }

    public void btn_Quit () {
        Application.Quit ();

        if (mGame == GameMode.PLAY_MODE)
            mGame = GameMode.MAIN_MENU;
    }

    [Header (" This For Limit Map ")]
    public Vector2 limitMap = new Vector2 (70, 70);

    public void LimitMap (Transform p) {
        p.position = new Vector3 (
            Mathf.Clamp (p.transform.position.x, -limitMap.x, limitMap.x),
            Mathf.Clamp (p.transform.position.y, -limitMap.y, limitMap.y),
            p.transform.position.z
        );
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube (transform.position, limitMap);
    }
}