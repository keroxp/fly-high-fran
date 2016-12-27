using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.S)) {
			var now = Time.time;
			Application.CaptureScreenshot("~/Desktop/ss"+now+".png");
		}
	}
}
