using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
	
	public float     fieldOfViewAngle = 110f;
	public bool      playerInSight;
	public Vector3   personalLastSighting;
	
	private NavMeshAgent          navMeshAgent;
	private SphereCollider        sphereCollider;
	private Animator              animator;
	private LastPlayerSighting    lastPlayerSighting;
	private GameObject            player;
	private Animator              playerAnim;
	private Vector3               previousSighting;
	
	void Awake ()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		sphereCollider = GetComponent<SphereCollider>();
		animator = GetComponent<Animator>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
		player = GameObject.FindGameObjectWithTag(Tags.player);
		playerAnim = player.GetComponent<Animator>();
		
		personalLastSighting = lastPlayerSighting.resetPosition;
		previousSighting = lastPlayerSighting.resetPosition;
	}

	void Update () 
	{
		if(lastPlayerSighting.position != previousSighting)
			personalLastSighting = lastPlayerSighting.position;
		
		previousSighting = lastPlayerSighting.position;
		
		//GETTING THE PLAYER'S HEALTH AND CHECKING IF HE IS DEAD
	}
	
	void OnTriggerStay(Collider colliderObject)
	{
		//Debug.Log("Triggered by "+colliderObject.gameObject.name);
		if(colliderObject.gameObject == player)
		{
			playerInSight = false;
			
			Vector3 direction = colliderObject.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);
			
			if(angle < fieldOfViewAngle/2)
			{
				playerInSight = true;
				lastPlayerSighting.position = player.transform.position;
			}
		}
	}
	
	void OnTriggerExit(Collider colliderObject)
	{
		if(colliderObject.gameObject == player)
		{
			playerInSight = false;
		}
	}
	
	float CalculatePathLength(Vector3 targetPosition)
	{
		NavMeshPath path = new NavMeshPath();
		
		if(navMeshAgent.enabled)
			navMeshAgent.CalculatePath(targetPosition, path);
		
		Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
		
		allWayPoints[0] = transform.position;
		allWayPoints[allWayPoints.Length - 1] = targetPosition;
		
		for(int i=0 ; i < path.corners.Length ; i++)
		{
			allWayPoints[i+1] = path.corners[i];
		}
		
		float pathLength = 0.0f;
		for(int i=0 ; i<allWayPoints.Length-1 ; i++)
		{
			pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i+1]);
		}
		return pathLength;
	}
}
