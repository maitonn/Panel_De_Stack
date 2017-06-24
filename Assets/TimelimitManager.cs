using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelimitManager : MonoBehaviour {
	float time = 0;
	GameManager manager;
	// Use this for initialization
	void Start () {
		manager = GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
	}
}
