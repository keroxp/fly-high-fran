﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableAspect : MonoBehaviour {
	public Camera cam;
	// 画像のサイズ
	private float width = 640f;
	private float height = 960f;
	// 画像のPixel Per Unit
	private float pixelPerUnit = 100f;

	void Awake () {
		float aspect = (float)Screen.height / (float)Screen.width;
		float bgAcpect = height / width;

		// カメラのorthographicSizeを設定
		cam.orthographicSize = (height / 2f / pixelPerUnit);


		if (bgAcpect > aspect) {
			// 倍率
			float bgScale = height / Screen.height;
			// viewport rectの幅
			float camWidth = width / (Screen.width * bgScale);
			// viewportRectを設定
			cam.rect = new Rect ((1f - camWidth) / 2f, 0f, camWidth, 1f);
		} else {
			// 倍率
			float bgScale = width / Screen.width;
			// viewport rectの幅
			float camHeight = height / (Screen.height * bgScale);
			// viewportRectを設定
			cam.rect = new Rect (0f, (1f - camHeight) / 2f, 1f, camHeight);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
