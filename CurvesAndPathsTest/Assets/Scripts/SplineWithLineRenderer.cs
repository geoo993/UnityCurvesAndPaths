using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SplineWithLineRenderer : MonoBehaviour {

	[SerializeField] private bool loop;

	public GameObject sphere;
	private GameObject walker;
	[Range(10f,100f)] public float duration = 10f;
	private float progress = 0f;
	public bool lookForward = true;
	private bool goingForward = true;
	public enum WalkerMode {
		Once,
		Loop,
		PingPong
	}
	public WalkerMode walkerMode = WalkerMode.Once;

	private List<GameObject> points = new List<GameObject>();
	private LineRenderer lineRenderer;
	private const int stepsPerCurve = 10;

	private const float directionScale = 0.5f;

	void Start ()
	{
		walker = GameObject.Find("Diamond");
		SetupLineRenderer ();
		generateSpheres ();


	}
	private void Update () {
		
		AddCurves ();

		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
			
				if (walkerMode == WalkerMode.Once) {
					progress = 1f;
				}
				else if (walkerMode == WalkerMode.Loop) {
					progress -= 1f;
				}
				else {
					progress = 2f - progress;
					goingForward = false;
				}
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		Vector3 position = GetPoint(progress);
		walker.transform.localPosition = position;

		if (lookForward) {
			walker.transform.LookAt(position + GetDirection(progress));
		}


	}

	void generateSpheres()
	{
		
		for (int i = 0; i <= 30; i++) {
			Vector3 rand = new Vector3 (Random.Range (-100, 100), Random.Range (0, 100), Random.Range (-100, 100));
			createSphere (rand, points);
		}
		print (points.Count);
	}

	private GameObject createSphere(Vector3 pos , List <GameObject> arr){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.GetComponent<Renderer> ().material.color = ExtensionMethods.RandomColor();
		a.transform.parent = this.transform;
		arr.Add (a);

		return a;
	}
	private void SetupLineRenderer()
	{

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.red, Color.green);
		lineRenderer.SetWidth(1f, 1f);


	}
	void AddCurves ()
	{
		int curveSteps = stepsPerCurve * CurveCount;////

		lineRenderer.SetVertexCount(curveSteps + 1);

		Vector3 lineStart = GetPoint(0f);////
		lineRenderer.SetPosition(0, lineStart + GetDirection(0f) * directionScale);////

		for (int i = 1; i <= curveSteps; i++) {
			Vector3 lineEnd = GetPoint(i / (float)curveSteps);

			//lineRenderer.SetPosition (i  , lineEnd);
			lineRenderer.SetPosition (i  , lineEnd + GetDirection(i / (float)curveSteps) * directionScale);

		}

		if (loop) {
			points[points.Count - 1].transform.localPosition = points[0].transform.localPosition;
		}


	}
		

	public int CurveCount {
		get {
			return (points.Count - 1) / 3;
		}
	}

	public Vector3 GetPoint (float t) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return transform.TransformPoint (
			GetPointForCubicCurve (
				points [i].transform.localPosition, 
				points [i + 1].transform.localPosition, 
				points [i + 2].transform.localPosition,
				points [i + 3].transform.localPosition, t));


	}

	public Vector3 GetVelocity (float t) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Count - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}

		return transform.TransformPoint(
			GetFirstDerivativeForCubicCurve(
				points[i].transform.localPosition, 
				points[i + 1].transform.localPosition, 
				points[i + 2].transform.localPosition, 
				points[i + 3].transform.localPosition, t)) - 
		transform.position;


	}

	////reduces the velocity line 
	public Vector3 GetDirection (float t) {
		return GetVelocity(t).normalized;
	}

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

}
