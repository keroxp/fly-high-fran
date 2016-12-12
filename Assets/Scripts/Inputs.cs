namespace FlyHighFran
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Inputs  {

	public static bool OnDown(int index) {
		return (Input.GetMouseButtonDown(index)) ||
	 			(Input.touchCount > 0  && Input.GetTouch(index).phase == TouchPhase.Began);	
	}
	public static bool OnUp(int index) {
		return (Input.GetMouseButtonUp(index)) ||
	 			(Input.touchCount > 0  && Input.GetTouch(index).phase == TouchPhase.Ended);
	}
	public static Vector2 GetPosition() {
		if (Input.touchCount > 0) {
			return Input.GetTouch(0).position;
		}
		var p = Input.mousePosition;
		return new Vector2(p.x, p.y);		
	}
}
}