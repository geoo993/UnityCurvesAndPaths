using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwirlPipeSystem : MonoBehaviour 
{

	public SwirlPipe pipePrefab;

	public int pipeCount;

	private SwirlPipe[] pipes;

	private void Awake () 
	{
		
		pipes = new SwirlPipe[pipeCount];
		for (int i = 0; i < pipeCount; i++) 
		{
			pipes[i] = Instantiate<SwirlPipe>(pipePrefab);
			SwirlPipe pipe = pipes[i];

			Vector3 pos = this.transform.localPosition + pipe.transform.localPosition;
			createSphere (pos, pipe.CurveRadius, pipe.CurveAngle - pipe.pipeRadius, pipe.transform);

			pipe.transform.SetParent(transform, false);

			if (i > 0) 
			{
				pipe.AlignWith(pipes[i - 1]);
			}

		}

		print (pipes[0].CurveRadius +"    ang: "+pipes[0].CurveAngle+"    pipe rad: "+pipes[0].pipeRadius);
	}

	Vector3 CircumferencePoint ( Vector3 center , float ang,  float radius  ){
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z;
		return pos;
	}

	private GameObject createSphere(Vector3 pos, float radius, float angle, Transform parent){

		GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
		a.transform.parent = parent;
		a.transform.localPosition = CircumferencePoint(pos, angle, radius);
		a.transform.localScale = new Vector3 (10f, 10f, 10f);

		return a;
	}


}

