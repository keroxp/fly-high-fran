using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassageChecker : MonoBehaviour {

	private AudioSource se;
	// Use this for initialization
	void Start () {
		se = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject.tag == "Enemy") {
			if (!se.isPlaying) se.Play();
		}
	}
}
