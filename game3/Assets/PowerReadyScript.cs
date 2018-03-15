using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerReadyScript : MonoBehaviour {

  public UnityEngine.UI.Slider bar;
  private bool ready = false;
  private Vector3 scale;
	// Use this for initialization
	void Start () {
		scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(bar.value==1) {
      if(!ready) {
        transform.localScale = scale*7;
      } else {
        Vector3 target = scale * (1.2f+.2f*Mathf.Cos(Time.time*8));
        transform.localScale += (target-transform.localScale)/5;
      }
      ready = true;
    } else {
      ready = false;
      transform.localScale += (scale - transform.localScale)/10;
    }
	}
}
