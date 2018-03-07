using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1_controller : MonoBehaviour {

    public int player;
    public float maxHealth = 100.0f;
    private float currentHealth;
    public float speed = 1.0f;
    private float ROF = 5.0f;
    public GameObject bullet;
    public UnityEngine.UI.Slider healthBar;
    public GameObject other_player;

    private Rigidbody rb;
    private Rigidbody other_rb;
    private float buffer;
    private bool synced;
    private KeyCode leftKey;
    private KeyCode upKey;
    private KeyCode rightKey;
    private KeyCode downKey;
    
	void Start () {
        rb = GetComponent<Rigidbody>();
        other_rb = other_player.GetComponent<Rigidbody>();
        buffer = 0.0f;
        currentHealth = maxHealth;
        healthBar.value = healthPercent();
        Debug.Log(player + " CurrentHealth: " + currentHealth);
        synced = false;
        if(player==1) {
            leftKey = KeyCode.A;
            upKey = KeyCode.W;
            rightKey = KeyCode.D;
            downKey = KeyCode.S;
        } else {
            leftKey = KeyCode.LeftArrow;
            upKey = KeyCode.UpArrow;
            rightKey = KeyCode.RightArrow;
            downKey = KeyCode.DownArrow;
        }
	}
	
	void FixedUpdate () {
        move();
        shoot();
	}

  void Update() {
      switchMode();
  }

    void move() {
        Vector3 input = Vector3.zero;
        if(Input.GetKey(leftKey)) {
          input.x += -1;
        }
        if(Input.GetKey(upKey)) {
          input.z += 1;
        }
        if(Input.GetKey(rightKey)) {
          input.x += 1;
        }
        if(Input.GetKey(downKey)) {
          input.z += -1;
        }
        input.Normalize();
        if(input.magnitude>0) {
            transform.rotation = Quaternion.LookRotation(input);
        }
        Vector3 movement = input * speed * Time.deltaTime;
        //Vector3 newPos = transform.position + movement;

        //if (newPos.x <= 12.5f && newPos.x >= -12.5f && newPos.z >= -3.0f && newPos.z <= 13.0f) {
            rb.MovePosition(transform.position + movement);
            //rb.velocity = movement.normalized * speed;
            rb.velocity = movement;
            // transform.position += movement;
            // rb.AddForce(movement, ForceMode.Impulse);
            //rb.Sleep();
            // rb.AddForce(movement*10, ForceMode.Impulse);
        //}
    }

    void shoot() {
        if (Input.GetKey(KeyCode.Space)) {
            if (buffer >= 1 / ROF) {
                Quaternion rotation;
                if(!synced) {
                    rotation = transform.rotation;
                } else {
                    Vector3 otherDirection = (other_rb.position - transform.position).normalized;
                    rotation = Quaternion.LookRotation(otherDirection);
                }
                var b = Instantiate(bullet, transform.position, rotation);
                b.transform.position += 1.5f * b.transform.forward;
                b.GetComponent<bullet_controller>().parentId = player;
                b.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
                buffer = 0.0f;
            }
        }
        buffer += Time.deltaTime;
    }

    void switchMode() {
        if(Input.GetKeyDown(KeyCode.B)){
            if(synced){
                synced = false;
            }
            else{
                synced = true;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "bullet" && other.GetComponent<bullet_controller>().parentId != player){
            TakeDamage(bullet_controller.damage);
            Debug.Log(player + " CurrentHealth: " + currentHealth);
        }
    }
    
    void OnCollisionEnter(Collision col) {
      if(col.gameObject.tag == "Enemy") {
          TakeDamage(EnemeyScript.damage);
          Vector3 dist = col.gameObject.transform.position - transform.position;
          // transform.position -= dist.normalized*1;
          //rb.MovePosition(transform.position - dist.normalized*1);
          //rb.Sleep();
          rb.AddForce(-dist*20, ForceMode.Impulse);
          // col.gameObject.transform.position += dist.normalized*1;
      }
    }

    private void TakeDamage(float amount) {
      currentHealth -= amount;
      healthBar.value = healthPercent();
      if(currentHealth<=0) {
        PlayerPrefs.SetString("previous", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.SceneManagement.SceneManager.LoadScene("death_screen");
      }
    }
    float healthPercent() {
        return currentHealth / maxHealth;
    }
}
