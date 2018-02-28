using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

  public GameObject Enemy;
  public float time = 1;
	// Use this for initialization
	void Start () {
		StartCoroutine(run());
	}
	
  IEnumerator run() {
    while(true) {
      yield return new WaitForSeconds(time);
      Spawn();
    }
  }

	// Update is called once per frame
	void Update () {
		
	}

  private void Spawn() {
    Instantiate(Enemy, transform.position, Quaternion.identity);
  }
}
