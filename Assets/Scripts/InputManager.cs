using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	BlockManager blockManager;
	// Use this for initialization
	void Start () {
		blockManager = GetComponent<BlockManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Horizontal")) 
		{
			blockManager.MoveFallBlock(new Vector3(Input.GetAxisRaw("Horizontal"),0, 0));
		}
		if (Input.GetButtonDown("Fire1"))
		{
			blockManager.FallBlock();
		}

	}
}
