using UnityEngine;
using UnityEngine.Networking;
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";
	private const string ENEMY_TAG = "Enemy";
	public PlayerWeapon weapon;
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

	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			partSyst.Play();
			audioSource.Play();
			//audioSource.Stop();
			Shoot();
		}
	}
	[Client]
	void Shoot()
	{
		RaycastHit _hit;
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit,weapon.range, mask))
		{
			if(_hit.collider.tag == PLAYER_TAG || _hit.collider.tag == ENEMY_TAG)
			{
				CmdPlayerShot(_hit.collider.name,weapon.damage);
			}
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
