using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChasePlayer : MonoBehaviour
{
    private Transform Player;
    int MoveSpeed = 2;
    int MaxDist = 2;
    int MinDist = 1;
    bool Sleeping = false;
    float jump = 0;
    CharacterController controller;
    private float Gravity = 20.0f;
    private Vector3 _moveDir = Vector3.zero;
    public GameObject flames;
    bool Attacking = false;
    private System.Random random;

    void Start()
    {
        random = new System.Random();
        Player = GameObject.FindWithTag("Player").transform;
        flames.GetComponent<ParticleSystem>().Stop();
        flames.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Attacking || Sleeping)
        {
            return;
        }

        transform.LookAt(Player);

        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                StartCoroutine("SleepForSeconds");
            }
        }
    }

    private IEnumerator SleepForSeconds()
    {
        Sleeping = true;
        yield return new WaitForSeconds(0.2f);
        Sleeping = false;
        StartCoroutine("AttackForSeconds");
    }

    private IEnumerator AttackForSeconds()
    {
        Attacking = true;
        flames.GetComponent<ParticleSystem>().Play();
        flames.GetComponent<BoxCollider>().enabled = true;

        yield return new WaitForSeconds(random.Next(5));
        flames.GetComponent<ParticleSystem>().Stop();
        flames.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(random.Next(2));
        Attacking = false;

    }
}
