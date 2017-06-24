// ブロックマネージャー
// ゲーム本体といっても過言ではない。
// ブロックをたくさん生成して、マッチングさせて試す
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockManager : MonoBehaviour {
	public GameObject block;		// ランダムで生成さ
	[SerializeField]
	int Width = 6; // 横幅
	[SerializeField]
	int Height = 30; // 高さ
	[SerializeField]
	int FirstHeight = 10; // 最初の高さ
	[SerializeField]
	Text popupText;
	[SerializeField]
	int BlockTypeNum = 10;
	public bool isFinish{ set; get; }
	int[] score; // スコア。このゲームにおけるスコアは、最低落下地点
	 GameObject Canvas;
	GameObject fallBlock; // 落下ブロック
	GameObject nextFallBlock;
	Vector3 standardFallPos = Vector3.zero;	// 初期落下地点
	[SerializeField] Vector3 nextFallPos;
	ScoreManager scoreManager;		// スコアマネージャー
	GameManager gameManager;
	[SerializeField] float maxFallScore = 200;
	[SerializeField] int oneLineScore = 1000;
	float dropTime = 0;
	private float prevMaxHeight = 0;

	int climb = 0; // どれだけ登っているか

	const RigidbodyConstraints FALL = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
	const RigidbodyConstraints NOT_FALL = RigidbodyConstraints.FreezeAll;
	// Use this for initialization
	void Start () {
		Canvas = GameObject.Find ("Canvas");
		GameObject manager = GameObject.FindWithTag ("GameController");
		gameManager = manager.GetComponent<GameManager> ();
		scoreManager = manager.GetComponent<ScoreManager> ();
		Initialize();
	}
	void Update(){
		dropTime += Time.deltaTime * 100;
	}
	// 初期化処理
	public void Initialize()
	{
		// 子オブジェクトは全部消す
		foreach (Transform chil in gameObject.transform) {
			Destroy (chil.gameObject);
		}
		scoreManager.ResetScore();
		isFinish = false;
		Camera.main.transform.position += climb * Vector3.down;
		climb = 0;
		prevMaxHeight = 0;
		standardFallPos = new Vector3((int)(Width / 2), (int)Height, 0);
		// 最初にブロック生成するはする。
		for (int w = 0; w < Width; ++w)
		{
			for (int h = 0; h < FirstHeight; ++h)
			{
				Vector3 pos = new Vector3(w, h, 0);
				GameObject instance = CreateBlock();
				
				Instantiate(instance, pos, transform.rotation, transform);
			}
		}
		// 落下地点を追加する
		fallBlock = Instantiate(block, standardFallPos, transform.rotation, this.transform);
		// ネクストも追加
		nextFallBlock = Instantiate (block, nextFallPos, transform.rotation, this.transform);

		// フォールブロックは落とさない
		fallBlock.GetComponent<Rigidbody>().constraints = NOT_FALL;
		nextFallBlock.GetComponent<Rigidbody> ().constraints = NOT_FALL;
		// スコアの初期化
		score = new int[Width];
		for (int i = 0; i < score.Length; ++i) {
			score [i] = 0;
		}
	}
	
	// ランダムでブロックを作成する
	GameObject CreateBlock()
	{
		block.GetComponent<BoxCollider>().enabled = true;
		GameObject instance = block;
		instance.GetComponent<Block>().BlockType = (uint)Random.Range(0, BlockTypeNum);

		return instance;
	}
	public void FallBlock()
	{
		// 終了したら落とせない
		if (isFinish) {
			return;
		}
		// 今持ってるブロックを落として制御不能にする
		if (fallBlock)
		{

			fallBlock.GetComponent<BoxCollider>().enabled = true;
			fallBlock.GetComponent<Rigidbody>().constraints = FALL;
			fallBlock.GetComponent<Rigidbody>().AddForce(Vector3.down * 10,ForceMode.Impulse);
			fallBlock = null;
		}
		GameObject instance = CreateBlock();
		instance.GetComponent<BoxCollider>().enabled = false;
		fallBlock = nextFallBlock;
		fallBlock.transform.position = standardFallPos;
		nextFallBlock = Instantiate(instance,nextFallPos,transform.rotation,this.transform);
		nextFallBlock.GetComponent<Rigidbody> ().constraints = NOT_FALL;
		int score = (int)(maxFallScore - dropTime);
		dropTime = 0;
		scoreManager.AddScore (score);
	}

	public void MoveFallBlock(Vector3 moveDist)
	{
		if (isFinish) {
			return;
		}
		Vector3 pos = moveDist + fallBlock.transform.position;
		if (pos.x >= 0 && pos.x < Width) {
			if (fallBlock) {
				fallBlock.transform.Translate (moveDist);
			}
		}
	}

	public void UpdateScore(Vector3 pos){
		// 上からレイを出して確認する
		int min = 9999;
		int max = 0;
		for (int i = 0; i < Width; ++i) {
			RaycastHit hit;
			Ray ray = new Ray(new Vector3(i,fallBlock.transform.position.y - 1,0),Vector3.down);
			Physics.Raycast(ray,out hit);
			score [i] = Mathf.RoundToInt(hit.transform.position.y + 0.3f); // 時間がないのでマジックナンバー
			if (min > score [i]) {
				min = score [i];
			}
			if (max < score [i]) {
				max = score [i];
			}
		}
		Debug.Log (max);
		//minとmaxが5以上離れていれば終わり
		Debug.Log (min.ToString() +  ":" + max.ToString());
		if (min + 5 < max) {
			gameManager.FinishGame ();
			return;
		}
		if (prevMaxHeight < min) {
			prevMaxHeight = min;
			scoreManager.AddScore (oneLineScore);
			ViewOneLineScore (pos);
		}

		// 一番下が3を超えたらだんだん高くなる
		if (min - 1 > climb) {
			++climb;
			Camera.main.transform.position += Vector3.up;
			fallBlock.transform.position += Vector3.up;
			nextFallBlock.transform.position += Vector3.up;
			nextFallPos += Vector3.up;
			standardFallPos += Vector3.up;
		}
	}
	// ラインボーナスの表示
	// http://hajimete-program.com/blog/2016/08/16/unity5でポップアップのダメージ表示を行う/
	void ViewOneLineScore(Vector3 pos){
		Text text = Instantiate (popupText) as Text;
		Vector2 screenPosition = Camera.main.WorldToScreenPoint (pos);
		text.transform.SetParent (Canvas.transform,false);
		text.transform.position = screenPosition;
	}
}
