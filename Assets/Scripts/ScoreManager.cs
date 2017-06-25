/// <summary>
/// Score manager.
/// スコアを管理し、SocreTextに描画するクラス
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour {
	[SerializeField] Text[] scoreUI;
	[SerializeField] int nowScore;
	private double viewScore;
	[SerializeField] double scoreUpLength = 1;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (scoreUI.Length > 0) {
			if (nowScore >= viewScore) {
				viewScore += scoreUpLength * Time.deltaTime;
				if (viewScore >= nowScore) {
					viewScore = nowScore;
				}
				for (int i = 0; i < scoreUI.Length; ++i) {
					scoreUI [i].text = viewScore.ToString ("#,0");
				}
			} 

		}
	}

	public void AddScore(int num){
		nowScore += num;
	}
	public void SetScore(int num){
		nowScore = num;
	}
	public int GetScore(){
		return nowScore;
	}
	public void ResetScore(){
		nowScore = 0;
		viewScore = -1;
	}
}
