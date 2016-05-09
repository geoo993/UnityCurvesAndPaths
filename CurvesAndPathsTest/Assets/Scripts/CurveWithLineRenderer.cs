using UnityEngine;
using System.Collections;


//[RequireComponent (typeof (BoxCollider))]
[RequireComponent (typeof (LineRenderer))]
//[RequireComponent (typeof (MeshRenderer))]


public class CurveWithLineRenderer: MonoBehaviour {

	public enum BezierType { Quadratic, Cubic };
	public BezierType bezierType = BezierType.Quadratic;




	private LineRenderer lineRenderer;

	public GameObject[] points;

	private const int lineSteps = 10;
	private const float directionScale = 0.5f;

	void Start ()
	{
		SetupLineRenderer ();

	}


	void Update () {

		DrawSimpleLines ();
		DrawCurve ();
	}
		

	//
	public static Vector3 GetPointForQuadraticCurve (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}
	public static Vector3 GetFirstDerivativeForQuadraticCurve (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}


	//
	public static Vector3 GetPointForCubicCurve (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}
	public static Vector3 GetFirstDerivativeForCubicCurve  (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}


	public Vector3 GetPoint (float t) {


		Vector3 point = new Vector3();

		switch (bezierType) {

		case BezierType.Quadratic:

			point = transform.TransformPoint (
				GetPointForQuadraticCurve (
				points [0].transform.localPosition, 
				points [1].transform.localPosition, 
				points [2].transform.localPosition, t));
	
			break;
		case BezierType.Cubic:

			point = transform.TransformPoint (
			GetPointForCubicCurve (
				points [0].transform.localPosition, 
				points [1].transform.localPosition, 
				points [2].transform.localPosition,
				points [3].transform.localPosition, t));


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
				GetFirstDerivativeForQuadraticCurve(
				points[0].transform.localPosition, 
				points[1].transform.localPosition, 
				points[2].transform.localPosition, t)) -
					transform.position;
			break;
		case BezierType.Cubic:

			velocityPoint = transform.TransformPoint(
				GetFirstDerivativeForCubicCurve(
				points[0].transform.localPosition, 
				points[1].transform.localPosition, 
				points[2].transform.localPosition, 
				points[3].transform.localPosition, t)) - 
				transform.position;
			
			break;
		}
		return velocityPoint;

	}

	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}



	private void SetupLineRenderer()
	{

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.green);
		lineRenderer.SetWidth(0.2F, 0.2F);


	}

	void DrawSimpleLines ()
	{
		
		lineRenderer.SetVertexCount(points.Length + 1);

		//start line
		lineRenderer.SetPosition(0, points[0].transform.position);
		for (int a = 1; a < points.Length; a++) {

			//mid points
			lineRenderer.SetPosition (a , points [a].transform.position);
		}
		//end line
		lineRenderer.SetPosition(points.Length, points[0].transform.position);


	}

	void DrawCurve ()
	{
		
		lineRenderer.SetVertexCount(lineSteps+1);
		Vector3 lineStart = GetPoint(0f);


		switch (bezierType) {

		case BezierType.Quadratic:
			lineRenderer.SetPosition(0, lineStart);


			for (int i = 1; i <= lineSteps; i++) {

				Vector3 lineEnd = GetPoint(i / (float)lineSteps);
				lineRenderer.SetPosition (i  , lineEnd);

			}

			break;
		case BezierType.Cubic:
			lineRenderer.SetPosition(0, lineStart + GetDirection(0f) * directionScale);
		
			for (int i = 1; i <= lineSteps; i++) {
				Vector3 lineEnd = GetPoint(i / (float)lineSteps);
			
				lineRenderer.SetPosition (i  , lineEnd);
			}

			break;
		}


	}
		


}
