using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class GameManager : MonoBehaviour {

	[SerializeField] BlockManager blockManager;
	[SerializeField] GameObject FinishUI;
	[SerializeField] GameObject StartMenueUI;
	[SerializeField] Text TimeUI;
	float time = 0;
	[SerializeField] float LimitTime = 60.0f;
	bool isGame;
	// Use this for initialization
	void Start ()
	{
		// 開始時はメインメニューやんけ
		MainMenue ();

	}

	void Update(){
		if (!isGame) {
			return;
		}
		// もはやこれゲーム全体をマネージメントじゃなくてなんかすごいところまでマネージし始めたのでひどい
		time -= Time.deltaTime;
		if (time <= 0) {
			FinishGame ();
			time = 0;
		}if (TimeUI) {
			TimeUI.text = time.ToString ("f2");
		}
	}
	/// <summary>
	/// ゲーム終了の合図
	/// </summary>
	public void FinishGame(){
		blockManager.isFinish = true;
		isGame = false;
		FinishUI.SetActive (true);
		GameObject obj = FinishUI.transform.Find ("Restart").gameObject;
		EventSystem.current.SetSelectedGameObject (obj);

	}

	/// <summary>
	/// ゲーム開始の合図
	/// </summary>
	public void StartGame(){
		FinishUI.SetActive (false);
		StartMenueUI.SetActive (false);
		blockManager.Initialize ();
		isGame = true;
		time = LimitTime;
	}
	/// <summary>
	/// メインメニュー表示する
	/// </summary>
	public void MainMenue(){
		StartMenueUI.SetActive (true);
		FinishUI.SetActive (false);
		blockManager.isFinish = true;
		isGame = false;
		GameObject obj = StartMenueUI.transform.Find ("Start").gameObject;
		EventSystem.current.SetSelectedGameObject (obj);
	}
}
