namespace FlyHighFran
{	
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
public class Player : MonoBehaviour, FluxComponent {	
	public AudioSource seCrash;
	public AudioSource seSwing;
	public Supervisor supervisor;
	public Rigidbody2D body;
	public GameObject face;
	public GameObject faceDamage;
	private readonly float forceX = 2f;
	private readonly float forceY = 4.0f;
	private readonly Vector2 forceLeft;
	private readonly Vector2 forceRight;
	private readonly Vector2 forceBottom;
	private SkeletonAnimation anim;
	private bool crashed;

	Player() {
		forceLeft = new Vector2 (-forceX, forceY);
		forceRight = new Vector2 (forceX, forceY);
		forceBottom = new Vector2 (0, -forceY);
	}

	// Use this for initialization
	void Awake() {
		Debug.Log ("player started! ");
		anim = GetComponent<SkeletonAnimation>();
		Flux.Get().subscribe(this);
	}
	void Start () {					
	}
	private enum Direction {Left, Right, Bottom}
	void doJump(Direction dir) {
		var vel = this.body.velocity;
		vel.Set(vel.x*0.2f, 0);
		this.body.velocity = vel;		
		anim.state.SetAnimation(0, "FlyingToTop", false);
		seSwing.Play();		
		switch(dir) {
			case Direction.Right:
				this.body.AddForce(forceRight, ForceMode2D.Impulse);
				break;			
			case Direction.Left:
				this.body.AddForce(forceLeft, ForceMode2D.Impulse);
				break;			
			case Direction.Bottom:
				this.body.AddForce(forceBottom, ForceMode2D.Impulse);
				break;
		}
	}
	public void OnAction(object action) {
		if (action is GameStateChanged){
			var changed = action as GameStateChanged;
			switch(changed.nextState) {
				case GameState.Playing:
					this.body.simulated = true;
					break;
				case GameState.Result:
					face.SetActive(false);
					anim.state.ClearTracks();					
					faceDamage.SetActive(true);
					break;
				default:
					face.SetActive(true);
					faceDamage.SetActive(false);
					this.body.simulated = false;
					break;
			}
		}						
	}
	// Update is called once per frame
	void Update () {
		switch (supervisor.State) {
			case GameState.Playing:
			case GameState.Result:
				updateInPlaying();
				break;
		}
	}


	void updateInPlaying() {
		if (crashed) return;		
		var trans = GetComponent<Transform> ();				
		 if (Input.touchCount > 0) {
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {			
				Debug.Log(string.Format("{0}, {1}", touch.position.x, Screen.width));				
				var dir = touch.position.x < Screen.width*.5f ? Direction.Left : Direction.Right; 
				doJump(dir);
			}
		}else if (Input.GetMouseButtonDown(0)) {
			doJump(Direction.Left);
		} else if (Input.GetMouseButtonDown(1)) {
			doJump(Direction.Right);
		} else if (Input.GetMouseButtonDown(2)) {
			doJump(Direction.Bottom);
		}		
		if (Input.GetKeyDown(KeyCode.JoystickButton16)) {
			doJump(Direction.Left);
		} else if (Input.GetKeyDown(KeyCode.JoystickButton17)) {
			doJump(Direction.Right);
		}		
		var rInput = Input.GetKeyDown (KeyCode.J);
		var lInput = Input.GetKeyDown (KeyCode.F);
		var bInput = Input.GetKeyDown (KeyCode.DownArrow);
		if (rInput) {
			doJump(Direction.Right);
		} else if (lInput) {
			doJump(Direction.Left);
		} else if (bInput) {
			doJump (Direction.Bottom);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {		
		if (supervisor.IsDebug) return;
		if (coll.gameObject.tag == "Enemy") {
			if (!seCrash.isPlaying) {
				seCrash.Play();		
				crashed = true;
				this.body.velocity = Vector2.zero; 
				Flux.Get().passAction(new GameStateChanged(supervisor.State, GameState.Result));		
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll) {		
		if (coll.gameObject.tag == "PlayableArea") {
			// gameover
			if (!crashed) {
				crashed = true;
				GetComponent<MeshRenderer>().enabled = false;
				seCrash.Play();	
				Flux.Get().passAction(new GameStateChanged(supervisor.State, GameState.Result));
			}
		}
	}
}
};