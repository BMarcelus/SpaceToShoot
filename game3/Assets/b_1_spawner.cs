using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class b_1_spawner : MonoBehaviour {

    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject Enemy3;
    public float time = 1.0f;
    private int counter;
    private float buffer;
    public int level;
    // Use this for initialization
    void Start() {
        buffer = 0;
        counter = 0;
    }


    // Update is called once per frame
    void Update() {
        if (buffer >= time) {
            Spawn();
            buffer = 0.0f;
        }
        buffer += Time.deltaTime;
    }

    private void Spawn() {
        if (level == 1) {
            level1();
        }
        else if (level == 2) {
            level2();
        }
        else if (level == 3) {
            level3();
        }
    }

    void level1() {
        if (counter < 15) {
            instance(Enemy);
            ++counter;
            if (counter >= 3) {
                Debug.Log("decreasing time" + time);
                if (time >= 1.0f) {
                    time -= 0.25f;
                }
            }
        }
    }

    void level2() {
        if (counter == 0) {
            instance(Enemy2);
            ++counter;
        }
        else if (counter == 1) {
            instance(Enemy3);
            ++counter;
            if (time >= 1.0f) {
                time += 3.0f;
            }
        }
        else if (counter < 10) {
            instance(Enemy2);
            instance(Enemy3);
            counter += 2;

        }
        else if (counter < 20) {
            instance(Enemy);
            instance(Enemy2);
            instance(Enemy3);
            counter += 3;
        }
    }

    void level3() {
        if (counter == 0) {
            for (int i = 0; i < 4; ++i) {
                instance(Enemy2);
                ++counter;
                time += 4f;
            }
        }
        else if (counter == 4) {
            for (int i = 0; i < 4; ++i){
                instance(Enemy3);
                ++counter;
                time -= 4f;
            }
        }
        else if(counter >= 8){
            int type = Random.Range(1,4);
            if(type == 1){
                instance(Enemy);
            }
            else if(type == 2){
                instance(Enemy2);
            }
            else if(type == 3){
                instance(Enemy3);
            }
            ++counter;
            if(time >=1){
                time -= .5f;
            }
        }
    }
    Vector3 randomPos() {
        return new Vector3(Random.Range(-11.0f, 11.0f), 10.0f, Random.Range(10.0f, -5.0f));
    }

    void instance(GameObject enemy) {
        Vector3 p = randomPos();
        GameObject e = Instantiate(enemy, p, Quaternion.identity);
        e.SetActive(true);
    }
}
