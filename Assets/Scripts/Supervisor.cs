namespace FlyHighFran
{

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Supervisor : MonoBehaviour, FluxComponent {
	/** Components **/
	public Text scoreText;
	public Text resultText;
	public Text levelText;
	public Text debugText;
	public Text highScoreText;
	public AudioSource seStart;	
	public AudioSource bgm;
	private bool _isDebug;
	private static readonly string PREF_HIGH_SCORE_KEY = "PLAYER_HIGH_SCORE";
	public bool IsDebug {
		get { return _isDebug; }
	}
	/** Properties **/
	private float _elapsedTime;
	public float ElapsedTime {
		get { return _elapsedTime; }
	}
	private GameState _state;
	public GameState State {
		get { return _state; }
	}
	private float _score;
	public float Score {
		get { return _score; }		
	}
	/** Members **/
	private bool isPaused;
	private GameObject[] startUIs;
	private GameObject[] playingUIs;
	private GameObject[] resultUIs;
	private int _level = 0;
	public int Level {
		get { return _level; }
	}
	void Awake() {
		startUIs = GameObject.FindGameObjectsWithTag("StartUI");
		playingUIs = GameObject.FindGameObjectsWithTag("PlayingUI");
		resultUIs = GameObject.FindGameObjectsWithTag("ResultUI");
		Flux.Get().subscribe(this);
	}
	// Use this for initialization
	void Start () {
		updateScore(_elapsedTime = 0);
		debugText.enabled = false;
		updateHighScore();
		goToStart();	
	}
	
	// Update is called once per frame
	void Update () {
		if (Debug.isDebugBuild) {
			debugText.enabled = _isDebug = Input.GetKey(KeyCode.D);
		}			
		switch (_state) {
			case GameState.Start:
			updateInStart();
			break;
			case GameState.Playing:
			updateInPlaying();
			break;
			case GameState.Result:
			updateInResult();
			break;
		}
		
	}
	private void updateInStart() {
		if (Inputs.OnUp(0) || Input.GetKeyUp(KeyCode.Space)) {
			goToPlaying();
		}
	}
	private void updateInPlaying() {
		updateScore(_elapsedTime += Time.deltaTime);
		scoreCounter += Time.deltaTime;
		if (scoreCounter > 10f) {						
			_level++;
			levelText.text = "LEVEL: "+(_level+1);
			scoreCounter = 0;
		}
		// ポーズ処理		
		if (Input.GetKeyDown(KeyCode.Space)) {
			Time.timeScale = isPaused ? 1.0f : 0.0f;
			isPaused = !isPaused;
		}
	}
	private void updateInResult() {

	}
	private string makeScoreText(float score) {		
		var text = ""+score;
		if (score - Mathf.Round(score) == 0) {
			text += ".0";
		}
		return string.Format("{0}M", text);
	}
	private float scoreCounter;
	private void updateScore(float etime) {				
		_score =  Mathf.Round(etime*10f)/10f;		
		scoreText.text = makeScoreText(_score);		
	}
	private void updateHighScore() {
		var hs = PlayerPrefs.GetFloat(PREF_HIGH_SCORE_KEY,0f);
		highScoreText.text = "HIGH SCORE: " + makeScoreText(hs);
	}
	private void initScene(GameState state) {
		_elapsedTime = 0;		
		foreach (var u in startUIs) {
			u.SetActive(state == GameState.Start);
		}
		foreach (var u in playingUIs) {			
			u.SetActive(state == GameState.Playing);
		}
		foreach (var u in resultUIs) {
			u.SetActive(state == GameState.Result);
		}
	}
	private void goToPlaying() {				
		seStart.Play();
		Flux.Get().passAction(new GameStateChanged(_state, GameState.Playing));		
	}
	private void goToStart() {
		Flux.Get().passAction(new GameStateChanged(_state, GameState.Start));
	}	
	public void OnAction(object action) {
		if (action is GameStateChanged) {
			var changed = action as GameStateChanged;
			initScene(_state = changed.nextState);
			switch (changed.nextState) {
				case GameState.Result: {
					bgm.Stop();
					resultText.text = makeScoreText(_score);
					// ハイスコア更新
					var hs = PlayerPrefs.GetFloat(PREF_HIGH_SCORE_KEY, 0f);
					PlayerPrefs.SetFloat(PREF_HIGH_SCORE_KEY, Mathf.Max(_score, hs));
					break;
				}					
			}
		}
	}
	public void OnRestartButtonClick() {
		SceneManager.LoadScene("Main");
	}
	public void OnTweetButtonClick() {
		var text = WWW.EscapeURL(string.Format("Fly! High! フランで{0}飛びました！", makeScoreText(_score)));
		var hash = WWW.EscapeURL("ふらフラ");
		var url = string.Format("https://twitter.com/intent/tweet?text={0}&hashtags={1}", text, hash);		
		Application.OpenURL(url);		
	}
}
}