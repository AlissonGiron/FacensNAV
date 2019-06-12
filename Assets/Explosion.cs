using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject Exp;
    public GameObject Blood;
    public Transform Parent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            GameObject blood = Instantiate(Blood, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(blood, 1.0f);
        }

        if (other.transform.GetInstanceID() == Parent.GetInstanceID())
        {
            return;
        }

        GameObject rocket = Instantiate(Exp, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 0.25f);
        Destroy(rocket, 1.0f);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }
}
