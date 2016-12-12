namespace FlyHighFran
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
	public GameObject above;
	public float floorY;	
	private Supervisor superVisor;
	// Use this for initialization
	void Awake () {
		superVisor = GameObject.FindObjectOfType<Supervisor>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(superVisor.State) {
			case GameState.Playing:
			case GameState.Start: {
				var pos = this.transform.position;
				var sp = 0.05f + superVisor.Level * 0.005f;
				pos.y -= sp * 60.0f * Time.deltaTime;
				if (pos.y <= floorY) {
					var box = GetComponent<BoxCollider>();			
					pos.y = above.transform.position.y + box.size.y;			
				}
				this.transform.position = pos;
				break;
			}
		}	
	}
}
	
}