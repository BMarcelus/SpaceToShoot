using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

  public GameObject Enemy;
  public float time = 1;
  private int counter;
  private float buffer;
	// Use this for initialization
	void Start () {
        buffer = 0;
        counter = 0;
	}
	

	// Update is called once per frame
	void Update () {
        if (buffer >= time) {
            Spawn();
            ++counter;
            buffer = 0;
        }
        buffer += Time.deltaTime;
	}

  private void Spawn() {
    Vector3 position = new Vector3(Random.Range(-11.0f, 11.0f), 10.0f, Random.Range(10.0f, 5.0f));
    GameObject enemy = Instantiate(Enemy, position, Quaternion.identity);
    enemy.SetActive(true);
  }
}
