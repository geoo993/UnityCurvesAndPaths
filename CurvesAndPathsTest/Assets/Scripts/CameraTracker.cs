using UnityEngine;
using System.Collections;

public class CameraTracker : MonoBehaviour {

	public Transform target;
	[Range(-50.0f, 50.0f)]public float distanceUP, distanceBack, minimumHeight =  1.0f;

	private Vector3 positionVelocity;
	private Vector3 offset;

	bool getPos = true;



	private Vector3 gotoPos()
	{

		////calculate a new position to place the camera:
		Vector3 newPosition =  target.position + (target.forward * distanceBack);
		float newPosY = Mathf.Max (newPosition.y + distanceUP, minimumHeight);
		newPosition = new Vector3(newPosition.x, newPosY, newPosition.z);

		return newPosition;

	}

	void LateUpdate () {

			FollowTarget ();
	
	}


	void FollowTarget (){

		getPos = true;

		////move to camera:
		//transform.position = newPosition;
		transform.position = Vector3.SmoothDamp(transform.position, gotoPos(), ref positionVelocity, 0.18f);


		////rotate the camera to look at where the target is pointing
		Vector3 lookAt = target.position + (target.forward * 5);
		transform.LookAt (lookAt);
	}



}
