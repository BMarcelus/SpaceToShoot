using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1_controller : MonoBehaviour {

    public int player;
    public float maxHealth = 100.0f;
    private float currentHealth;
    public float speed = 1.0f;
    public float ROF = 25.0f;
    public GameObject bullet;
    public UnityEngine.UI.Slider healthBar;
    public GameObject other_player;

    private Rigidbody rb;
    private Rigidbody other_rb;
    private float buffer;
    private bool synced;


	void Start () {
        rb = GetComponent<Rigidbody>();
        other_rb = other_player.GetComponent<Rigidbody>();
        buffer = 0.0f;
        currentHealth = maxHealth;
        healthBar.value = healthPercent();
        Debug.Log(player + " CurrentHealth: " + currentHealth);
        synced = false;
	}
	
	void FixedUpdate () {
        move();
        shoot();
        switchMode();
	}

    void move() {
        Vector3 input = Vector3.zero;
        if (player == 1) {
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) {
                transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
                transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) {
                transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
                transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.W)) {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.S)) {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.D)) {
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.A)) {
                transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                input += transform.forward;
            }
        }

        else if (player == 2) {
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.UpArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.DownArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                input += transform.forward;
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.rotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
                input += transform.forward;
            }
        }
        input.Normalize();
        Vector3 movement = input * speed * Time.deltaTime;
        //Vector3 newPos = transform.position + movement;

        //if (newPos.x <= 12.5f && newPos.x >= -12.5f && newPos.z >= -3.0f && newPos.z <= 13.0f) {
            rb.MovePosition(transform.position + movement);
        //}
    }

    void shoot() {
        if (!synced) {
            if (Input.GetKey(KeyCode.Space)) {
                if (buffer >= 1 / ROF) {
                    var b = Instantiate(bullet, transform.position, transform.rotation);
                    b.transform.position += 1.0f * b.transform.forward;
                    buffer = 0.0f;
                }
                else {
                    buffer += Time.deltaTime;
                }
            }
        }

        else if (synced) {
            if (Input.GetKey(KeyCode.Space)) {
                if (buffer >= 1 / ROF) {
                    Vector3 otherDirection = (other_rb.position - transform.position).normalized;
                    Quaternion pointToOther = Quaternion.LookRotation(otherDirection);
                    var b = Instantiate(bullet, transform.position, pointToOther);
                    b.transform.position += 1.0f * b.transform.forward;
                    buffer = 0.0f;
                }
                else {
                    buffer += Time.deltaTime;
                }
            }
        }
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
        if(other.tag == "bullet"){
            currentHealth -= bullet_controller.damage;
            Debug.Log(player + " CurrentHealth: " + currentHealth);
        }
        healthBar.value = healthPercent();
    }

    float healthPercent() {
        return currentHealth / maxHealth;
    }
}
