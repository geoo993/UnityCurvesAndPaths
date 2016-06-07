using UnityEngine;
using System.Collections;

public class RollingBallCamera : MonoBehaviour {

	private float distance = 50.0f;
	private float currentX = 0.0f;
	private float currentY = 0.0f;

	public Transform target;


	void Start () {
	
	}

	void Update () {
	
		currentX += Input.GetAxis ("Vertical2");
		currentY += Input.GetAxis ("Horizontal2");

	}
	void LateUpdate () {

		FollowTargetWhenRolling ();

	}
	void FollowTargetWhenRolling ()
	{
		Vector3 dir = new Vector3 (0.0f, 0.0f, -distance);
		Quaternion rotation = Quaternion.Euler (currentX, currentY, 0);
		transform.position = target.position + rotation * dir;
		transform.LookAt (target.position);

	}
}
