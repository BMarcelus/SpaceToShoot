using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

  public GameObject Enemy;
  public float time = 1;
  private float buffer;
	// Use this for initialization
	void Start () {
        buffer = 0;
	}
	

	// Update is called once per frame
	void Update () {
        if (buffer >= time) {
            Spawn();
            buffer = 0;
        }
        buffer += Time.deltaTime;
	}

  private void Spawn() {
    Instantiate(Enemy, transform.position, Quaternion.identity);
  }
}
