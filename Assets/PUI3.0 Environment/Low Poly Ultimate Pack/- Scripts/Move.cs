using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {


	public float speed= 1.5f;
	public float spacing= 1.0f;
	private Vector3 pos;

	void  Start (){
		pos = transform.position;
	}

	void  Update (){

		if (Input.GetKeyDown(KeyCode.D))
			pos.x -= spacing;
		if (Input.GetKeyDown(KeyCode.A))
			pos.x += spacing;

		transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
	}
}