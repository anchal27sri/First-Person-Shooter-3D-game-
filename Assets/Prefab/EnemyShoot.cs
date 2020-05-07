using UnityEngine;
using UnityEngine.Networking;
public class EnemyShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";
	private const string ENEMY_TAG = "Enemy";
	//public int waitTime = 1;
	public float fireRate = 0.1f;
	private float lastShot = 0.0f;
	public PlayerWeapon weapon;
	public bool canShoot = false;
	private GameObject shotObject;
	private AudioSource audioSource;
	public ParticleSystem partSyst;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		if(cam == null)
		{
			Debug.Log("error: No Camera Referenced");
			this.enabled = false;
		}
	}
	public void setShooting(bool _canShoot,GameObject _shotObject)
	{
		canShoot = _canShoot;
		shotObject = _shotObject;

	}
	void Update()
	{
		if(canShoot)
			Shoot();		
	}
	[Client]
	void Shoot()
	{
		
		if(Time.time >fireRate + lastShot)
		{
			partSyst.Play();
			audioSource.Play();
			Debug.Log(Time.time+" " +fireRate + " " +lastShot);
			CmdPlayerShot(shotObject.name,weapon.damage);
			lastShot = Time.time;
		}

	}

	[Command] 
	void CmdPlayerShot(string _playerID, int _damage)
	{
		Debug.Log(_playerID + " has been shot");

		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);
	}

}
