using UnityEngine;
using System.Collections;

public class RollingBallMovement : MonoBehaviour {

	private Rigidbody rigid; 
	public static float speed = 0.0f;// speed variable is the speed
	private float moveVertical = 0.0f;
	private float moveHorizontal = 0.0f;


	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown ("space")) 
		{
			
			//print ("space");
			this.transform.rotation = Quaternion.Euler(0, 0, 0);

			Vector3 look = new Vector3 (0.0f, Camera.main.gameObject.transform.position.y, Camera.main.gameObject.transform.position.z);
			this.transform.LookAt(Camera.main.gameObject.transform.position,Camera.main.gameObject.transform.forward);

		}
	}
	void FixedUpdate ()
	{
			BallMovement ();

	}

	void BallMovement(){
		
		speed = 200f;
	
		moveHorizontal = Input.GetAxis ("Horizontal"); 
		moveVertical = Input.GetAxis ("Vertical");

		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical) * (speed * 10) * Time.deltaTime;
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, 0.0f) * (speed * 10) * Time.deltaTime;

		//rigid.AddForce(Camera.main.gameObject.transform.forward * (moveVertical * 10) * Time.deltaTime);
		rigid.AddForce((Camera.main.gameObject.transform.forward ) * moveVertical );



	}
}
