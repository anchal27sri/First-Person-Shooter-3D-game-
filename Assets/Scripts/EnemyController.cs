using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	public float range;
	public float stoppingDistance = 5f;
	private Transform target;
	private GameObject playerObject; 
	private EnemyShoot enemyShootObject;

	void Start(){
		enemyShootObject = GetComponent<EnemyShoot>();
	}
	void Update () {
		playerObject = GameObject.FindGameObjectWithTag("Player");
		
		if(playerObject)
		{
			target = playerObject.GetComponent<Transform>();
			float dist = Vector3.Distance(transform.position,target.position);
			Vector3 targetDirection = target.position - transform.position;
			if(dist<range && dist>stoppingDistance)
			{
				enemyShootObject.setShooting(true,playerObject);	
				transform.position = Vector3.MoveTowards(transform.position,target.position,speed*Time.deltaTime);
				Vector3 newDirection = Vector3.RotateTowards(transform.forward,targetDirection,speed*Time.deltaTime,0f);
				transform.rotation = Quaternion.LookRotation(newDirection);
			}
			else
				enemyShootObject.setShooting(false,playerObject);
		}
	}
}
