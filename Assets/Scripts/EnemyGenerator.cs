namespace FlyHighFran {
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour, FluxComponent {
	public Supervisor supervisor;
	public GameObject enemyPrefab;
	private float elapsedTime = 2f;
	private float scheduledTime = 4f; // s	

	// Use this for initialization
	void Awake () {
		Flux.Get().subscribe(this);	
	}

	float createRandom() {
		float ret = 0.0f;
		for (var i = 0; i < 5; i++) {
			ret += Random.value;
		}
		return ret / 5;
	}
	// Update is called once per frame
	void Update () {		
		switch(supervisor.State) {
			case GameState.Playing:
				updateInPlaying();
				break;
		}
	}
	private void updateInPlaying() {
		var dt = Time.deltaTime;
		elapsedTime += dt;		
		if (elapsedTime >= scheduledTime) {
			Debug.Log("new enemy");
			elapsedTime = 0;			
			var x = -5.0f + createRandom() * 10.0f;			
			var enm = Instantiate(enemyPrefab, new Vector3(x, 6.0f, 0.0f), Quaternion.identity);			
		}		
	}
	public void OnAction(object action) {
		if (action is GameStateChanged) {
			var changed = action as GameStateChanged;			
		}
	}
}

}