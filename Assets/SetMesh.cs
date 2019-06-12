﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.gameObject.AddComponent(typeof(MeshCollider));

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent(typeof(MeshCollider));
        }
    }
}
