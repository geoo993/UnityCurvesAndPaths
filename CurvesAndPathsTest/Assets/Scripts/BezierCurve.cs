using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierCurve : MonoBehaviour {



	public enum BezierType { Quadratic, Cubic };
	public BezierType bezierType = BezierType.Quadratic;


	public Vector3[] points;

	public Vector3 GetPoint (float t) {


		Vector3 point = new Vector3();

		switch (bezierType) {

		case BezierType.Quadratic:
			
			point = transform.TransformPoint (
				Bezier.GetPointForQuadraticCurve (points [0], points [1], points [2], t));

			break;
		case BezierType.Cubic:

			point = transform.TransformPoint (
				Bezier.GetPointForCubicCurve (points [0], points [1], points [2], points [3], t));

			break;
		}

		return point;
	}

	////show how the line changes or breaks along the curve overtime using this velocity
	public Vector3 GetVelocity (float t) {

		Vector3 velocityPoint = new Vector3();

		switch (bezierType) {

		case BezierType.Quadratic:
			
			velocityPoint =  transform.TransformPoint(
				Bezier.GetFirstDerivativeForQuadraticCurve(points[0], points[1], points[2], t)) -
				transform.position;

			break;
		case BezierType.Cubic:
			
			velocityPoint = transform.TransformPoint(
				Bezier.GetFirstDerivativeForCubicCurve(points[0], points[1], points[2], points[3], t)) - 
				transform.position;
			
			break;
		}
		return velocityPoint;
	}


	////reduces the velocity line 
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

	public void Reset () {
		points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(3f, 0f, 0f)
		};
	}

	void Awake () {

		this.name = "Curve";
	}
}