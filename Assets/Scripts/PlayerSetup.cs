using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	GameObject playerUIPrefab;
	private GameObject playerUIInstance;

	public Text healthText;

	Camera sceneCamera;

	void Start () {

		if(!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
			
		}
		else
		{
			sceneCamera = Camera.main;
			if(sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(false);
			}
			//create playerUI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			//playerHealthUIInstance = Instantiate(playerHealthUIPrefab);
			//playerHealthUIInstance.
			if(healthText.tag=="Player")
				healthText.text = "Health: ";
		}
		GetComponent<Player>().Setup();
		
		
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();

		GameManager.RegisterPlayer(_netID, _player);
	}

	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}
	 
	void DisableComponents()
	{
		for(int i=0;i<componentsToDisable.Length;i++)
			{
				componentsToDisable[i].enabled = false;
			}
	}

	void OnDisable()
	{

		Destroy(playerUIInstance);
		if(sceneCamera !=null)
		{
			sceneCamera.gameObject.SetActive(true);
		}

		GameManager.UnregisterPlayer(transform.name);
	}


}
