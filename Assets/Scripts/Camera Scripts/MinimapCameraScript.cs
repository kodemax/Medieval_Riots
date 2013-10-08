using UnityEngine;
using System.Collections;

public class MinimapCameraScript : MonoBehaviour {
	
	public Transform player;
	
	void LateUpdate () 
	{
		Vector3 playerPosition = player.position;
		transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
	}
}
