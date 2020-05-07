using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	private PlayerMotor motor;
	[SerializeField]
	public float lookSensitivity = 3f;

	[SerializeField]
	private float thrustForce = 1000f;
	void Start()
	{
		motor = GetComponent<PlayerMotor>();
	}

	void Update(){
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movhorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		Vector3 _velocity = (_movhorizontal + _movVertical) * speed;

		motor.Move(_velocity);

		//when mouse is moved on screen in x-direction (left-right)
		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _roation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		motor.Rotate(_roation);

		float _xRot = Input.GetAxisRaw("Mouse Y");
		float _cameraRotation = _xRot * lookSensitivity;

		motor.RotateCamera(_cameraRotation);

		Vector3 _thrustForce = Vector3.zero;
		//apply the thrust force
		if (Input.GetButton("Jump"))
		{
			_thrustForce = Vector3.up * thrustForce;
		}

		motor.ApplyThruster(_thrustForce);

	}
}
