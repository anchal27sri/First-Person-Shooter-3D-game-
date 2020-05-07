using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	private Camera cam;
	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private Vector3 thrustForce  = Vector3.zero;

	[SerializeField]
	private float cameraRotationLimit = 85f;
	private float currentCameraRoationX = 0f;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}

	public void Rotate(Vector3 _roation)
	{
		rotation = _roation;
	}

	public void RotateCamera(float _cameraRotation)
	{
		cameraRotationX = _cameraRotation;
	}

	//get a force vector for our thruster
	public void ApplyThruster(Vector3 _thrustForce)
	{
		thrustForce = _thrustForce;
	}

	void FixedUpdate()
	{
		PerformMovement();
		PerformRoation();
	}

	void PerformMovement()
	{
		if(velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}
		if (thrustForce != Vector3.zero)
		{
			rb.AddForce(thrustForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
	}

	void PerformRoation()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
		if(cam != null)
		{
			//set our rotation and clamp it
			currentCameraRoationX -= cameraRotationX;
			currentCameraRoationX = Mathf.Clamp(currentCameraRoationX, -cameraRotationLimit, cameraRotationLimit);
			//apply rotation to the transform of the camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRoationX, 0f,0f); 
		}
	}
}
