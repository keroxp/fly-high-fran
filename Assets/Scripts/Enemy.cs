namespace FlyHighFran
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	private float scaleX = 1.0f;
	private bool hasBecomeVisible = false;
	// Use this for initialization

	private Supervisor superVisor;
	void Awake() {
		var s = this.transform.localScale;
		this.transform.localScale = s;		
	}
	void Start () {
		superVisor = GameObject.FindObjectOfType<Supervisor>();		
	}
	
	// Update is called once per frame
	void Update () {
		if (superVisor.State == GameState.Playing) {
			var pos = this.transform.position;
			var sp = 0.05f + superVisor.Level * 0.005f;
			pos.y -= sp*60f*Time.deltaTime;
			this.transform.position = pos;	
		}			
	}

	void OnDestroy() {
		Debug.Log("enemy destroyed");
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.name == "EnemyFloor") {
			Destroy(gameObject);
		}
	}
}
}