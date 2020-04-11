using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
     public Player _Player;

      [SerializeField]
    private Vector3 Offset;
    [SerializeField]
    private float Speed = 1.5f;
    public bool useLimitCamera;

        void Start()
    {
        _Player = FindObjectOfType<Player>();
    }

    void LateUpdate()
    {
        if (useLimitCamera)
        {
           Game_Manager._Instace.LimitMap(this.transform);
          // Debug.Log(" Cam ");
        }

        Follow_Player();

      
    }


    private void Follow_Player () {

        Vector3 posCam = _Player.transform.position + Offset;

        posCam.x = Mathf.Lerp(transform.position.x, posCam.x, Speed * Time.deltaTime);
        posCam.y = Mathf.Lerp (transform.position.y, posCam.y, Speed * Time.deltaTime);
        posCam.z = -10;

        transform.position = posCam;

    }

   

 


}
