using UnityEngine;
using System.Collections;

public class Enemyanimation : MonoBehaviour 
{
	public float deadZone = 5f;
	
	private  Transform player;
	private EnemySight enemySight;
	private NavMeshAgent navMeshAgent;
	private Animator anim;
	private HashIDs hash;
	private AnimatorSetup animSetup;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		enemySight = GetComponent<EnemySight>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
		
		navMeshAgent.updateRotation =false;
		animSetup = new AnimatorSetup(anim, hash);
	
		deadZone *= Mathf.Deg2Rad;
	}
	
	void navAnimSetup()
	{
		float speed;
		float angle;
		
		if(enemySight.playerInSight)
		{
			navMeshAgent.destination = player.position;
		}
	}
	
}
