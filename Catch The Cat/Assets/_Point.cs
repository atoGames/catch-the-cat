using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Point : MonoBehaviour
{
    private Player _Player;


    [SerializeField]
    private float Radius = 20;

    void Start()
    {
        if (_Player == null) _Player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
            var d = Vector2.Distance(this.transform.position , _Player.transform.position);
            if (d < Radius)
            {
                if (this.gameObject.activeSelf == true)
                     this.gameObject.SetActive(false);
            }
            else
        {
            if (this.gameObject.activeSelf == false)
                this.gameObject.SetActive(true);
        }
    }
}
