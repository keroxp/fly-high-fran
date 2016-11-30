using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public string myName;
	public bool flag;
	public int num;
	public Rect labelRect;
	public Rigidbody2D body;
	private bool gameover = false;
	private readonly float forceX = 1.414f;
	private readonly float forceY = 4.0f;
	private readonly Vector2 forceLeft;
	private readonly Vector2 forceRight;

	Player() {
		labelRect = new Rect (0,0,300,100);
		forceLeft = new Vector2 (-forceX, forceY);
		forceRight = new Vector2 (forceX, forceY);
	}

	// Use this for initialization
	void Start () {
		var msg = string.Format ("name = {0}, flag = {1}, num = {2}", name, flag, num);			
		//Physics2D.gravity = new Vector2 (0, -9.81f / 2);
		Debug.Log ("player started! " + msg);
	}

	// Update is called once per frame
	void Update () {
		var trans = GetComponent<Transform> ();
		var rInput = Input.GetKeyDown (KeyCode.RightArrow);
		var lInput = Input.GetKeyDown (KeyCode.LeftArrow);		
		if (rInput || lInput) {
			Debug.Log(string.Format("input! r = {0}, l = {1}", rInput, lInput));
			var vel = this.body.velocity;
			this.body.velocity = new Vector2(vel.x*0.2f, 0);
			if (rInput) {
				// 右				
				this.body.AddForce(forceRight, ForceMode2D.Impulse);
			} else if (lInput) {
				// 左
				this.body.AddForce(forceLeft, ForceMode2D.Impulse);
			}
		}
		if (trans.position.y <= -400) {
			gameover = true;
		}
	}

	void OnGUI () {
		if (gameover) {
			GUI.Label (labelRect, "Game Over");
		}
	}
}
