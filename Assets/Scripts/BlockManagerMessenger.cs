/// <summary>
/// Block manager messenger.
/// 長ったらしい名前だけどBlockManagerにメッセージを送るクラス
/// 着地した時とか消える時とかメッセージ送りたい時に利用する
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManagerMessenger : MonoBehaviour {
	BlockManager manager;
	// Use this for initialization
	void Start () {
		// そもそも親じゃないとうまくいけないので気をつけて
		manager = transform.parent.GetComponent<BlockManager> ();
		//TODO:うまくポリモーフィズムすればすごい便利そう
	}

	void OnCollisionEnter(Collision col){
		gameObject.layer = 0;
		manager.UpdateScore (transform.position);
		gameObject.GetComponent<Rigidbody> ().Sleep ();
	}
}
