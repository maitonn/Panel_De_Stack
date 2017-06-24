// Rayを前と横と下に飛ばして色が同じなら消す奴

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Block), typeof(Rigidbody))]
public class DestroyBlock : MonoBehaviour
{
	private Block block;
	bool deleteFlag;
	List<GameObject> list;
	[SerializeField]
	Vector3[] rayDistans;
	[SerializeField]
	uint DestroyNum;
	void Awake()
	{
		block = GetComponent<Block>();
		list = new List<GameObject>();
	}
	// 衝突したらブロック消える判定するよ
	// colは使わず、下左右にrayを飛ばして判断する
	void OnCollisionEnter()
	{
		RaycastHit hit;
		for (int i = 0; i < rayDistans.Length; ++i)
		{
			Ray ray = new Ray(transform.position, rayDistans[i]);
			if (Physics.Raycast(ray, out hit, 1.0f))
			{
				if (hit.collider.tag != "Block")
				{
					continue;
				}
				if (hit.collider.GetComponent<Block>().BlockType == block.BlockType)
				{
					deleteFlag = true;
					list.Add(hit.collider.gameObject);
					//hit.collider.GetComponent<DestroyBlock>().CheckDestroy();
				}
			}
		}
		if (deleteFlag)
		{
			list.Add(this.gameObject);
		}
		if (list.Count >= DestroyNum)
		{
			Debug.Log(list.Count);
			foreach (GameObject obj in list)
			{
				// 削除
				Destroy(obj);
			}
		}
		list.Clear();
		CheckDestroy();
	}
	public void CheckDestroy()
	{
		

	}

}
