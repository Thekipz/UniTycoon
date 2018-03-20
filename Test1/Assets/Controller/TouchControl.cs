using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {
    Vector3 lastFramePosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("mouse pressed");
        }
      
        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

}
