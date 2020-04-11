using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D), typeof (Rigidbody2D))]
public class Player : MonoBehaviour {
    private Game_Manager m_Game_Manager;
    private BoxCollider2D m_BoxCollider2D;

    [HideInInspector]
    public Rigidbody2D m_Rigidbody2D;

    [SerializeField]
    private Vector2 m_Movement;

    [SerializeField]
    private Animator anim_Player;
    [SerializeField]
    private AudioSource audioClip_Catch;

    [SerializeField]
    private float m_Speed = 300f;
    private float m_NormalSpeed;
    private float m_DeSpeed = 600f;
    public bool isCatch;

    // Input
    private const string _Horizontal = "Horizontal";
    private const string _Vertical = "Vertical";

    private bool _TakeMedicine;
    private GameObject _goMedicine;

    void Start () {
        m_NormalSpeed = m_Speed;
        _TakeMedicine = false;
        if (m_Game_Manager == null) m_Game_Manager = FindObjectOfType<Game_Manager> ();

        if (m_BoxCollider2D == null) m_BoxCollider2D = GetComponent<BoxCollider2D> ();
        if (m_Rigidbody2D == null) m_Rigidbody2D = GetComponent<Rigidbody2D> ();

        if (anim_Player == null) anim_Player = GetComponentInChildren<Animator> ();

        //  Rigidbody2D Setting -- if Game Start -- For Team
        m_Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        m_Rigidbody2D.gravityScale = 0;
        m_Rigidbody2D.freezeRotation = true;
    }
    void Update () {
        if (m_Game_Manager.mGame == GameMode.PLAY_MODE) {
            m_Game_Manager.LimitMap (this.transform);
            // Animation -- if we Use it
            PlayerAnimation ();
            // Catch  The Cat
            if (Input.GetKeyDown (KeyCode.Space) && isCatch) {
                if (m_Game_Manager.currntCat != null) {
                    Cat _Cat = m_Game_Manager.currntCat.GetComponent<Cat> ();
                    // Debug.Log(" Health  " + _Cat._Health);
                    _Cat.OnDelogShow (_Cat);
                    audioClip_Catch.Play ();
                }
            }
            if (Input.GetKeyDown (KeyCode.R) && _TakeMedicine) {
                m_Game_Manager._Medicine += 5;
                _TakeMedicine = false;
                Destroy (_goMedicine);
                if (_goMedicine != null)
                    _goMedicine = null;

            }

            if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
                m_Speed = m_DeSpeed;
            else
                m_Speed = m_NormalSpeed;

        }
    }
    void FixedUpdate () {
        if (m_Game_Manager.mGame == GameMode.PLAY_MODE)
            MovePlayer ();
    }

    // Move Player
    private void MovePlayer () {
        // Get Input
        m_Movement = new Vector2 (Input.GetAxis (_Horizontal), Input.GetAxis (_Vertical)) * Time.fixedDeltaTime * m_Speed;
        // Move the Player
        m_Rigidbody2D.velocity = m_Movement;
    }

    // Animation
    private void PlayerAnimation () {
        if (m_Movement.magnitude > 1) {
            // Is the player moving -- Run Animation walk
            anim_Player.SetBool ("Walk", true);

            if (m_Game_Manager.goDelogShow.activeSelf == true)
                m_Game_Manager.goDelogShow.SetActive (false);
        } else {
            // The player does not move  --  stop Animation walk
            anim_Player.SetBool ("Walk", false);
        }
    }

    // Collision
    void OnCollisionEnter2D (Collision2D other) {
        // To Ignore some Objects >>>> In Unity > Edit > Project Settings > Physics 2D > Layer Collision
        m_Movement = Vector2.zero;
        m_Rigidbody2D.velocity = Vector2.zero;

    }

    // Trigger
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Cat")) {
            isCatch = true;
            m_Game_Manager.currntCat = other.gameObject;
        }
        if (other.CompareTag ("Medicine")) {
            _goMedicine = other.gameObject;
            _TakeMedicine = true;
        }
    }
    private void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Cat")) {
            m_Game_Manager.currntCat = null;
            isCatch = false;
        }
        if (other.CompareTag ("Medicine")) {
            _TakeMedicine = false;
            _goMedicine = null;
        }
    }

}