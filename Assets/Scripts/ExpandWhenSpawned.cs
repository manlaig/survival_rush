﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandWhenSpawned : MonoBehaviour
{
    [SerializeField] float speed = 15f;

	void Start ()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
                rb.velocity = -child.right * speed;
            Destroy(gameObject, 0.3f);
        }
	}
}
