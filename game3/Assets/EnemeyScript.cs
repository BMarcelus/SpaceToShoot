using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemeyScript : MonoBehaviour {

  public GameObject[] players;
  private Rigidbody rb;
  private float speed = 4.0f;
  private Vector3 velocity = new Vector3();
  private Transform healthDisplay;
  private Vector3 startHealthSize;
  public float maxHealth = 30;
  private float health;
  public static float damage = 5;
  public int playerId;
  private float fallSpeed = 0;
	// Use this for initialization
  private float reflectEffectTimer = 0;
  private float reflectEffectTime = .2f;
  private float hitEffectTimer = 0;
  private float hitEffectTime = .1f;
  private Vector3 targetPosition;
  private bool targetting = false;
  public LayerMask obstructionlayers;
  private bool dead = false;

    //private NavMeshAgent navAgent;
  AudioSource gettingHit;
  AudioSource reflectAud;
  public GameObject deathAudClass;
  AudioSource deathAud;



    void Start () {
    AudioSource[] auds = GetComponents<AudioSource>();
    gettingHit = auds[0];
    reflectAud = auds[1];
    deathAudClass = GameObject.Find("deathAudClass");
    deathAud = deathAudClass.GetComponent<AudioSource>();
    rb = GetComponent<Rigidbody>();
    healthDisplay = transform.Find("Health");
    health = maxHealth;
    startHealthSize = healthDisplay.localScale;

    //navAgent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
    // rb.velocity = velocity;
    // velocity.x = Mathf.Cos(Time.time*10);
    // velocity.y = Mathf.Sin(Time.time*10);
    rb.velocity = rb.velocity*.9f;    
    if(reflectEffectTimer>0) {
      reflectEffectTimer -= Time.deltaTime;
      // rb.velocity = Vector3.zero;
      if(reflectEffectTimer<=0) {
        transform.localScale = new Vector3(1,1,1);
      }
      return;
    }
    if(hitEffectTimer>0) {
      hitEffectTimer -= Time.deltaTime;
      // rb.velocity = Vector3.zero;
      if(hitEffectTimer<=0) {
        transform.localScale = new Vector3(1,1,1);
      }
      return;
    }
        if (transform.position.y < 0) {
            rb.MovePosition(new Vector3(transform.position.x,0,transform.position.z));
        }
        else if (transform.position.y == 0) {
            moveToPlayer();
        }
        else {
          fallSpeed += 20*Time.deltaTime;
          rb.MovePosition(new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z));
        }
    //rb.MovePosition(transform.position + (velocity.normalized * Time.deltaTime * speed));
	}

    void moveToPlayer() {
        GameObject p = FindClosestPlayer();
        //navAgent.destination = p.transform.position;
        if(!targetting) {
          if(Random.Range(0,20)<1) {
            velocity = Quaternion.Euler(0, Random.Range(0,360), 0) * transform.forward;
          }
          if(Random.Range(0,20)<1) {
            velocity = Vector3.zero;
          }
        }
        // if(targetting) {
          // rb.velocity = velocity.normalized * speed;
          // rb.velocity=Vector3.zero;
          rb.MovePosition(transform.position + velocity.normalized*speed*Time.deltaTime);
        // }
    }

  private GameObject FindClosestPlayer() {
    GameObject player = players[0];
    float minDist = - 1;
    for(int i=0;i<players.Length;i++) {
      GameObject p = players[i];
      float dist = (transform.position - p.transform.position).magnitude;
      Ray ray = new Ray();
      ray.origin = transform.position;
      ray.direction = p.transform.position-transform.position;
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, 20f, obstructionlayers)) {
        if(hit.collider.tag == "Player") {
          if(minDist<0||dist<minDist) {
            minDist = dist;
            player = p;
          }
          targetPosition = p.transform.position;
          velocity = targetPosition - transform.position;            
          targetting = true;
        }
      }
    }
    return player;
  }

  void OnTriggerEnter(Collider col) {
    if(col.tag == "bullet") {
      bullet_controller bc = col.GetComponent<bullet_controller>();
      if(bc.parentId==7) return;
      if(playerId!=0&&bc.parentId!=playerId) {
        reflectAud.Play();
        bc.parentId = 7;
        bc.transform.Rotate(new Vector3(0, 180, 0));
        bc.speed = bc.speed/2;
        reflectEffectTimer=reflectEffectTime;
        transform.localScale = new Vector3(1.1f,1.1f,1.1f);
      } else {
        gettingHit.Play();
        health -= bullet_controller.damage;
        transform.localScale = new Vector3(.9f,1f,.9f);
        hitEffectTimer = hitEffectTime;
        if(health<=0) {
          deathAud.Play();
          Destroy(gameObject);
          if(!dead)
          ++b_level1.killCount;
          dead=true;
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
