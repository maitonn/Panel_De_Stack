// ブロック管理クラス
// ブロックIDとエフェクトを管理してる。
//　こいつ単体では仕事しないのでBlockMangerを別途用意したげる

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	public uint BlockType; // ブロックの種類。0~5番目くらい利用したい
	public Color[] color;
	// 自身のブロックタイプに応じて色を変える
	void Start()
	{	
		GetComponent<Renderer>().material.color = color[BlockType];
	}
}
