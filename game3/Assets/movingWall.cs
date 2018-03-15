using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingWall : MonoBehaviour {

    bool up;
    private float speed;
    private float timer;
	// Use this for initialization
	void Start () {
        up = true;
        speed = Random.Range(1.0f, 5.0f);
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!up) {
            if (transform.position.z >= -2.0f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (speed*Time.deltaTime));
            }
            else { up = true; }
        }
        else {
            if (transform.position.z <= 8.5f) {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (speed*Time.deltaTime));
            }
            else { up = false; }
        }
        if (timer >= 10.0f) {
            speed = Random.Range(1.0f, 5.0f);
            timer = 0.0f;
        }
        timer += Time.deltaTime;
	}
}
