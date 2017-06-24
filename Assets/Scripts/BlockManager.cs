// ブロックマネージャー
// ゲーム本体といっても過言ではない。
// ブロックをたくさん生成して、マッチングさせて試す
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
	public GameObject obj;
	[SerializeField]
	int Width = 6; // 横幅
	[SerializeField]
	int Height = 30; // 高さ
	[SerializeField]
	int FirstHeight = 10; // 最初の高さ

	[SerializeField]
	int BlockTypeNum = 10;

	Vector3 standardFallPos = Vector3.zero;
	Vector3 nowFallPos = Vector3.zero;

	GameObject fallBlock;

	const RigidbodyConstraints FALL = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
	const RigidbodyConstraints NOT_FALL = RigidbodyConstraints.FreezeAll;
	// Use this for initialization
	void Start () {
		Initialize();
	}
	// 初期化処理
	void Initialize()
	{
		standardFallPos = new Vector3((int)(Width / 2), (int)Height, 0);
		for (int w = 0; w < Width; ++w)
		{
			for (int h = 0; h < FirstHeight; ++h)
			{
				Vector3 pos = new Vector3(w, h, 0);
				GameObject instance = CreateBlock();
				
				Instantiate(instance, pos, transform.rotation, transform);
			}
		}
		fallBlock = Instantiate(obj, standardFallPos, transform.rotation, this.transform);
		fallBlock.GetComponent<Rigidbody>().constraints = NOT_FALL;
	}
	
	// ランダムでブロックを作成する
	GameObject CreateBlock()
	{
		obj.GetComponent<BoxCollider>().enabled = true;
		GameObject instance = obj;
		instance.GetComponent<Block>().BlockType = (uint)Random.Range(0, BlockTypeNum);

		return instance;
	}
	public void FallBlock()
	{
		// 今持ってるブロックを落として制御不能にする
		if (fallBlock)
		{

			fallBlock.GetComponent<BoxCollider>().enabled = true;
			fallBlock.GetComponent<Rigidbody>().constraints = FALL;
			fallBlock.GetComponent<Rigidbody>().AddForce(Vector3.down);
			fallBlock = null;
		}
		GameObject instance = CreateBlock();
		instance.GetComponent<BoxCollider>().enabled = false;
		fallBlock = Instantiate(instance,standardFallPos,transform.rotation,this.transform);
		fallBlock.GetComponent<Rigidbody>().constraints = NOT_FALL;
	}

	public void MoveFallBlock(Vector3 moveDist)
	{
		if (fallBlock)
		{
			fallBlock.transform.Translate(moveDist);
		}
	}
	// Update is called once per frame
	void Update () {

	}
}
