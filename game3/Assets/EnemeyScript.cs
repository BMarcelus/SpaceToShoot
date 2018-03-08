using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyScript : MonoBehaviour {

  public GameObject[] players;
  private Rigidbody rb;
  public float speed = 200.0f;
  private Vector3 velocity = new Vector3();
  private Transform healthDisplay;
  private Vector3 startHealthSize;
  public float maxHealth = 30;
  private float health;
  public static float damage = 5;
  public int playerId;
  private float fallSpeed = 0.05f;
	// Use this for initialization
	void Start () {
    rb = GetComponent<Rigidbody>();
    healthDisplay = transform.Find("Health");
    health = maxHealth;
    startHealthSize = healthDisplay.localScale;
	}
	
	// Update is called once per frame
	void Update () {
    // rb.velocity = velocity;
    // velocity.x = Mathf.Cos(Time.time*10);
    // velocity.y = Mathf.Sin(Time.time*10);
        if (transform.position.y < 0) {
            rb.MovePosition(new Vector3(transform.position.x,0,transform.position.z));
        }
        else if (transform.position.y == 0) {
            moveToPlayer();
        }
        else {
            rb.MovePosition(new Vector3(transform.position.x, transform.position.y - fallSpeed, transform.position.z));
        }
    //rb.MovePosition(transform.position + (velocity.normalized * Time.deltaTime * speed));
	}

    void moveToPlayer() {
        GameObject p = FindClosestPlayer();
        velocity = p.transform.position - transform.position;
        rb.velocity = velocity.normalized * speed;
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
      if(playerId!=0&&col.GetComponent<bullet_controller>().parentId!=playerId) {
        health += bullet_controller.damage;
        if(health>maxHealth)health=maxHealth;  
      } else {
        health -= bullet_controller.damage;
        if(health<=0) {
          Destroy(gameObject);
          b_level1.killCount += 1;
          Debug.Log(b_level1.killCount);
          return;
        }
      }
      Vector3 healthSize = startHealthSize;
      healthSize.x *= health/maxHealth;
      healthDisplay.localScale = healthSize;
    }
  }

  void OnCollisionEnter(Collision col) {
      if (col.gameObject.tag == "Player") {
          Vector3 dist = col.gameObject.transform.position - transform.position;
          rb.AddForce(-dist * 50, ForceMode.Impulse);
      }
  }
}
