using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p1_controller : MonoBehaviour {

    public int player;
    public float maxHealth = 100.0f;
    private float currentHealth;
    public float speed = 1.0f;
    private float ROF = 3.0f;
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

    private float invulTimer = 0;
    private float invulTime = 1;

    public UnityEngine.UI.Slider swapBar;
    public UnityEngine.UI.Slider modeBar;
    private float swapTimer = 0.0f;
    private float swapTime = 1.0f;
    private float modeTimer = 0.0f;
    private float modeTime = 1.0f;

    public GameObject pauseMenu;
    private bool paused;
	void Start () {
        pauseMenu.SetActive(false);
        paused = false;
        rb = GetComponent<Rigidbody>();
        other_rb = other_player.GetComponent<Rigidbody>();
        buffer = 0.0f;
        currentHealth = maxHealth;
        healthBar.value = healthPercent();
        Debug.Log(player + " CurrentHealth: " + currentHealth);
        synced = true;
        swapBar.value = 0;
        modeBar.value = 0;
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
        MeshRenderer r = gameObject.GetComponent<MeshRenderer>();        
        if(invulTimer>0) {
          r.enabled = !r.enabled;
        } else {
          r.enabled = true;
        }
	}

  void Update() {
      if(invulTimer>0) {
        invulTimer -= Time.deltaTime;
      }
      switchMode();
      swap();

      if (!paused && player == 1 && Input.GetKeyDown(KeyCode.Escape)) {
          Time.timeScale = 0;
          paused = true;
          pauseMenu.SetActive(true);
          var cp = pauseMenu.GetComponent<pauseButtons>().controlPanel;
          cp.SetActive(false);
      }
      else if (paused && player == 1 && Input.GetKeyDown(KeyCode.Escape)) {
          Time.timeScale = 1;
          paused = false;
          var cp = pauseMenu.GetComponent<pauseButtons>().controlPanel;
          cp.SetActive(false);
          pauseMenu.SetActive(false);

      }
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
        if (player == 1) {
            if (modeTimer >= modeTime) {
                if (synced && Input.GetKeyDown(KeyCode.RightShift)) {
                    synced = false;
                    ROF = ROF * 5;
                }
                if (!synced) {
                    modeBar.value -= Time.deltaTime/10;
                    if (modeBar.value <= 0) {
                        synced = true;
                        ROF = ROF / 5;
                        modeTimer = 0.0f;
                    }
                }
            }
            if (modeTimer < modeTime) {
                modeTimer += Time.deltaTime/25;
                modeBar.value = modeTimer / modeTime;
            }
        }
        if (player == 2) {
            synced = other_player.GetComponent<p1_controller>().synced;
            ROF = other_player.GetComponent<p1_controller>().ROF;
        }
    }

    void swap() {
        if (player == 1) {
            if (swapTimer >= swapTime) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    Vector3 temp = transform.position;
                    transform.position = other_player.transform.position;
                    other_player.transform.position = temp;
                    swapTimer = 0.0f;
                }
            }
            if (swapTimer < swapTime) {
                swapTimer += Time.deltaTime / 10;
                swapBar.value = swapTimer / swapTime;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "bullet" && other.GetComponent<bullet_controller>().parentId != player){
            if(invulTimer>0)return;            
            TakeDamage(bullet_controller.damage/5.0f);
            Debug.Log(player + " CurrentHealth: " + currentHealth);
        }
    }
    
    void OnCollisionEnter(Collision col) {
      if(col.gameObject.tag == "Enemy") {
          if(invulTimer>0)return;        
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
      invulTimer = invulTime;
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
