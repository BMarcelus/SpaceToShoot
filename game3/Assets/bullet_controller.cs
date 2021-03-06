﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_controller : MonoBehaviour {

    public float speed = 5.0f;
    public static float damage = 20.0f;
    private Rigidbody rb;
    public int parentId;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other){
        if (other.tag != "bullet") {
            EnemeyScript es = other.GetComponent<EnemeyScript>();
            if(es && es.playerId!=0&&(parentId == 7 || other.GetComponent<EnemeyScript>().playerId != parentId))return;
            Destroy(gameObject);
        }
    }
}
