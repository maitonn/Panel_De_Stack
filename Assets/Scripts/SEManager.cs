/// <summary>
/// SEを管理するクラス。
/// SE管理オブジェクトにつけて利用する
/// どう考えてハッシュテーブルで保持した方がいいけど今日は最終日なのでこれで妥協
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour {

	[SerializeField]AudioClip[] clip;
	AudioSource p_audio;
	void Start(){
		p_audio = GetComponent<AudioSource> ();
	}
	public void PlaySE(uint index){
		if (index < clip.Length) {
			Debug.Log("SE:" + index.ToString());
			p_audio.PlayOneShot (clip [index]);
		}
	}
}
