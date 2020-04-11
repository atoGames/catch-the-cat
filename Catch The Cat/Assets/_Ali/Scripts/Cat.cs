using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Cat : MonoBehaviour
{
    private Player _Player;

    private Rigidbody2D m_Rigidbody2D;


    public Action<Cat> OnDelogShow;

    // Change This if CAT hit Obstacle or Player
    [SerializeField]
    private float _CircleAngle = 0;
    public float _DistanceToObstacle = 10f;
    public float _DistanceToPlayer = 10f;
    public LayerMask _Obstacle;

    public int _Health;
    public int max_Health  = 10;

    [SerializeField]
    private float Speed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        if (_Player == null) _Player = FindObjectOfType<Player>();
        if (m_Rigidbody2D == null) m_Rigidbody2D = GetComponent<Rigidbody2D>();

        _CircleAngle = UnityEngine.Random.Range(0, 180);

        _Health = UnityEngine.Random.Range(3 , max_Health);
        OnDelogShow = DelogShowBegin;
    }

    void FixedUpdate()
    {
        if (Game_Manager._Instace.mGame == GameMode.PLAY_MODE)
            MoveCat();
    }

    public void SetSpeed(float _speed) =>  Speed = _speed;
     private void MoveCat()
    {
        var dir =  Vector2.up; // Vector2.up- (Vector2)transform.position;
        dir.x = Mathf.Sin(Mathf.Deg2Rad * -_CircleAngle);
        dir.y = Mathf.Cos(Mathf.Deg2Rad * -_CircleAngle);

        Debug.DrawRay( transform.position , dir, Color.red);

        if (Physics2D.Raycast(transform.position, dir, _DistanceToObstacle, _Obstacle))
            _CircleAngle += 1; 

        transform.rotation = Quaternion.Euler(0f, 0f, _CircleAngle);
        m_Rigidbody2D.velocity = transform.rotation  * Vector2.up * Time.fixedDeltaTime * Speed ;
    }


    public void DelogShowBegin(Cat c)
    {
        if (Game_Manager._Instace.goDelogShow != null)
        {
            if (c._Health < max_Health)
            {
             //   Debug.Log(" DelogShowBegin ");
                Game_Manager._Instace.goDelogShow.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OutMap"))
        {
            Game_Manager._Instace._Spawn_Manager.Remove_Cat(this);
            Destroy(this.gameObject);
        }
    }
}