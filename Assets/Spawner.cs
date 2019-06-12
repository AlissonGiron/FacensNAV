using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Entity;
    private GameObject LastSpawn;
    public bool ConstantSpawn;
    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        StartCoroutine("Spawn");
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            if(ConstantSpawn || LastSpawn == null)
            {
                LastSpawn = Instantiate(Entity, this.transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(ConstantSpawn ? 5 : random.Next(20));
        }
    }

}
