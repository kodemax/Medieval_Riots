using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour 
{
	public int dyingState;
	public int deadBool;
	public int locomotionState;
	public int shoutState;
	public int speedFloat;
	public int sneakingBool;
	public int shoutingBool;
	public int playerInSightBool;
	public int angularSpeedFloat;
	
	void Awake()
	{
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		speedFloat = Animator.StringToHash("Speed");
		angularSpeedFloat = Animator.StringToHash("Direction");
	}
}
