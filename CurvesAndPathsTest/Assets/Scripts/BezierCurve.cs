using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour {

	public Vector3[] points;

	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(3f, 0f, 0f)
		};
	}

	public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
	}

	////show how the line changes or breaks along the curve overtime using this velocity
	public Vector3 GetVelocity (float t) {
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], t)) -
			transform.position;
	}

	////reduces the velocity line 
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	void Awake () {

		this.name = "Curve";
	}
}