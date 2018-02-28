using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyScript : MonoBehaviour {

  public GameObject[] players;
  private Rigidbody rb;
  public float speed = 2f;
  private Vector3 velocity = new Vector3();
  private Transform healthDisplay;
  private Vector3 startHealthSize;
  public float maxHealth = 30;
  private float health;
  public static float damage = 5;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
    healthDisplay = transform.Find("Health");
    health = maxHealth;
    startHealthSize = healthDisplay.localScale;
	}
	
	// Update is called once per frame
	void Update () {
    rb.MovePosition(transform.position + velocity * Time.deltaTime * speed);
    // rb.velocity = velocity;
    // velocity.x = Mathf.Cos(Time.time*10);
    // velocity.y = Mathf.Sin(Time.time*10);
    GameObject p = FindClosestPlayer();
    velocity = p.transform.position - transform.position;
	}
  
  private GameObject FindClosestPlayer() {
    GameObject player = players[0];
    float minDist = - 1;
    for(int i=0;i<players.Length;i++) {
      GameObject p = players[i];
      float dist = (transform.position - p.transform.position).magnitude;
      if(minDist<0||dist<minDist) {
        minDist = dist;
        player = p;
      }
    }
    return player;
  }

  void OnTriggerEnter(Collider col) {
    if(col.tag == "bullet") {
      health -= bullet_controller.damage;
      if(health<=0) {
        Destroy(gameObject);
        return;
      }
      Vector3 healthSize = startHealthSize;
      healthSize.x *= health/maxHealth;
      healthDisplay.localScale = healthSize;
    }
  }
}
