using UnityEngine;
using System.Collections;

public class EnemyAIScript : MonoBehaviour 
{
	public Transform[] waypoints;
	public float patrollingSpeed = 0.3f;
	public float patrolSpeedRatio = 1/60f;
	public float chaseSpeed = 5.0f;
	public float chaseSpeedRatio = 0.2f;
	public float attackDistance = 0.2f;
	
	private NavMeshAgent navMeshAgent;
	int currentWaypointIndex;
	
	//TIMERS
	private float patrolWaitTime = 3f;
	private float patrolTimer = 0f;
	
	private float deathWaitTimer = 6f;
	private float deathTimer = 0f;
	
	Vector3 previousNavMeshDestination;
	private AnimatorSetup animSetup;
	private Transform player;
	
	//SCRIPTS
	EnemySight enemySight;
	EnemyHealth enemyHealth;
		
	private Animator anim;
	private HashIDs hash;
	void Awake () 
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		animSetup = new AnimatorSetup(anim, hash);
		enemySight = GetComponent<EnemySight>();
		enemyHealth = GetComponent<EnemyHealth>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		
		//INITIALIZING THE PATROLLING
		currentWaypointIndex = 0;
		navMeshAgent.destination = waypoints[currentWaypointIndex].position;
		anim.SetFloat("Speed", patrollingSpeed * patrolSpeedRatio);
		navMeshAgent.speed = patrollingSpeed;
		previousNavMeshDestination = Vector3.zero;
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//GETTING THE HEALTH OF THE ENEMY
		 int health = enemyHealth.health;
		
		if(health > 0)
		{
			//CHECKING IF THE PLAYER IS IN SIGHT
			float distance = Vector3.Distance(transform.position, player.position);
			//IF THE ENEMY IS NEAR THE PLAYER
			if(enemySight.playerInSight && distance < 20)
				attacking();
			//IF THE PLAYER IS IN SIGHT BUT FAR AWAY
			else if(enemySight.playerInSight && distance >= 20)
				chasing();	
			//IF NOT CHASING PATROL
			else
				Patrolling();
		}
		else
			dying();
	}
	void dying()
	{
		navMeshAgent.speed = 0;
		deathTimer += Time.deltaTime;
		
		if(deathTimer > deathWaitTimer)
			GameObject.Destroy(gameObject);
	}
	
	void attacking()
	{
		//ALWAYS LOOK AT THE PLAYER WHILE ATTACKING
		Vector3 playerLookAtPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
		transform.LookAt(playerLookAtPosition);
		
		//CHANGING THE ANIMATION
		anim.SetBool("CanAttack", true);
		
		//SETTING THE SPEED OF ANIMATION AND OF NAVMESH AGENT TO 0
		anim.SetFloat("Speed", 0);
		navMeshAgent.speed = 0;
	}
	
	void chasing()
	{	
		//MAKING SURE THE ENEMY WONT ATTACK WHEN CHASING
		anim.SetBool("CanAttack", false);
		
		//SETTING THE SPEED OF THE NAVMESH AGENT
		navMeshAgent.speed = chaseSpeed;
		anim.SetFloat("Speed", chaseSpeed * chaseSpeedRatio);
		
		//SETTING THE DESTINATION OF THE NAVMESH AGENT AS THE PLAYER
		navMeshAgent.destination = player.position;
	}
	void Patrolling()
	{
		//MAKING SURE THE ENEMY WONT ATTACK WHEN CHASING
		anim.SetBool("CanAttack", false);
	
		//SETTING THE SPEED OF THE NAVMESH AGENT
		navMeshAgent.speed = patrollingSpeed;
		
		//CHECKING IF THE ENEMY REACHED THE DESTINATION
		float distance = navMeshAgent.remainingDistance;
		if(distance <= 2)       //A THRESHOLD FOR THE STOPPING DISTANCE
		{
			patrolTimer += Time.deltaTime;
			if(patrolTimer >= patrolWaitTime && previousNavMeshDestination != navMeshAgent.destination)
			{
				patrolTimer = 0;
				currentWaypointIndex++;
				if(currentWaypointIndex > waypoints.Length - 1)
					currentWaypointIndex = 0;
			
				previousNavMeshDestination = navMeshAgent.destination;
				navMeshAgent.destination = waypoints[currentWaypointIndex].position;	
				
				anim.SetFloat("Speed", patrollingSpeed * patrolSpeedRatio);
			}
			else
				anim.SetFloat("Speed", 0.0f);
		}
		
	}
}
