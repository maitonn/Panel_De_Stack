using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelfDelete : MonoBehaviour {
	[SerializeField] float selfDeleteTime = 3.0f;
	float time = 0;
	Text text;
// Update is called once per frame
	void Start(){
		text = GetComponent<Text> ();
	}
	void Update () {
		time += Time.deltaTime;
		Color color = text.color;
		color.a =  1- time / selfDeleteTime;
		text.color = color;
		if (time > selfDeleteTime) {
			Destroy (this.gameObject);
		}
	}
}
