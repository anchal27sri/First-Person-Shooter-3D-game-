using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

	// Use this for initialization
	[SyncVar]
	private bool _isDead = false;

	public bool isDead
	{
		get {
			return _isDead;
		}
		protected set {
			_isDead = value;
		}
	}

	[SerializeField]
	private int maxhealth = 100;
	[SyncVar]
	private int currentHealth;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	public Text healthText;


	public void Setup()
	{
		wasEnabled = new bool[disableOnDeath.Length];
		//Debug.Log("here");
		for (int i = 0; i < wasEnabled.Length; i++)
		{
			//Debug.Log(disableOnDeath[i]);
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		if(transform.tag == "Player")
			Cursor.lockState = CursorLockMode.Locked;
		SetDefaults();
	}

	void Update()
	{
		if(currentHealth>=0)
		healthText.text = "Health: " + currentHealth.ToString("0");
		if(Input.GetButtonDown("offCursor"))
		{
			if(Cursor.lockState == CursorLockMode.Locked)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;
		}
	}


	[ClientRpc]
	public void RpcTakeDamage(int _amount)
	{
		
		currentHealth -= _amount;
		if(currentHealth<-20)
			return;
		Debug.Log(transform.name + "now has" + currentHealth + "health.");
		if(currentHealth <= 0)
		{
			Die();
			Debug.Log(transform.name + " now has " + currentHealth + " health");
		}
	}
	private void Die()
	{
		isDead = true;
		
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;


		Debug.Log(transform.name + "is Dead!");
		//CALL RESPAWN METHOD using a enumerator
		StartCoroutine(Respawn());

	}

	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawenTime);
		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		Debug.Log(transform.name + " respanwed.");


	}



	public void SetDefaults()
	{
		isDead = false;

		currentHealth = maxhealth;

		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;
	}


}
