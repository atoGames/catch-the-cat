using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour {
    // This For Spawn New Cat
    [SerializeField, Header (" Cats List   ")]
    public List<Cat> Cats;
    // This Points To Spawn
    public GameObject[] _Points;
    public GameObject goCatPrefab;
    public int maxCats = 20;
    // Update is called once per frame
    void Update () {
        if (Cats.Count >= maxCats)
            return;
        SpawnNewCat ();
    }
    public void SpawnNewCat () {
        var p_Point = Random.Range (1, _Points.Length);

        if (_Points[p_Point].activeSelf == false)
            return;
        else {
            GameObject go = Instantiate (goCatPrefab, _Points[p_Point].transform.position, Quaternion.identity);
            Add_Cat (go.GetComponent<Cat> ());
            go.transform.parent = this.transform;
            go.GetComponent<Cat> ().SetSpeed (Random.Range (30, 100));
        }
    }

    public void Add_Cat (Cat c) {
        if (Cats.Contains (c))
            return;

        Cats.Add (c);
    }
    public void Remove_Cat (Cat c) {
        if (Cats.Contains (c))
            Cats.Remove (c);
    }
}