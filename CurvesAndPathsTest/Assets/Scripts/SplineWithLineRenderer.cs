using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SplineWithLineRenderer : MonoBehaviour {

	[SerializeField] private bool loop;

	public GameObject sphere;
	private GameObject diamond;
	private GameObject torus;
	[Range(10f,100f)] public int itemsFrequency;
	[Range(10f,100f)] public float duration = 10f;

	public bool stopMovement;
	public bool curvesControlPoints = false;
	private List<Transform> Objects = new List<Transform> ();

	private List<float> progressTimes = new List<float>();

	private float progress = 0f;
	private bool lookForward = true;
	private bool goingForward = true;

	private List<Transform> items = new List<Transform>();

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
		this.name = "Roller Coaster";

		diamond = GameObject.Find("Diamond");
		torus = GameObject.Find("Torus");
		SetupLineRenderer ();
		generateSpheres ();


		AddItems ();

	}
	private void Update () {
		
		AddCurves ();
		ShowControlPoints ();

		MoveWalker (diamond);
		MoveWalker (torus);
		MoveItems ();
	}


	void generateSpheres()
	{
		
		for (int i = 0; i <= 30; i++) {
			Vector3 rand = new Vector3 (Random.Range (-200, 200), Random.Range (0, 200), Random.Range (-200, 200));
			createSphere (rand, points);

		}
		//print (points.Count);
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
		

	private void MoveWalker( GameObject walker)
	{
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
	private void AddItems(){

		items.Add (diamond.transform);
		items.Add (torus.transform);

		if (itemsFrequency <= 0 || items == null || items.Count == 0) {
			return;
		}
		float stepSize = itemsFrequency * items.Count;
		if (loop || stepSize == 1) {
			stepSize = 1f / stepSize;
		}
		else {
			stepSize = 1f / (stepSize - 1);
		}


		//float stepSize = 1f / (frequency * items.Length);
		for (int p = 0, f = 0; f < itemsFrequency; f++) {
			for (int i = 0; i < items.Count; i++, p++) {

				//print (p * stepSize);

				Transform item = Instantiate(items[i]) as Transform;
				Vector3 position = GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) {
					item.transform.LookAt(position + GetDirection(p * stepSize));
				}
				item.transform.parent = transform;

				Objects.Add (item);

				progressTimes.Add (p * stepSize);

			}
		}
	}
	private void MoveItems()
	{
		if (!stopMovement) {

			for (int i = 0; i < Objects.Count; i++) {

				progressTimes [i] += Time.deltaTime / duration;
				if (progressTimes [i] > 1f) {
					
					if (walkerMode == WalkerMode.Once)  {
						progressTimes [i] = 1f;
					} else if (walkerMode == WalkerMode.Loop)  {
						progressTimes [i] -= 1f;
					} 
				}

				Vector3 position = GetPoint (progressTimes [i]);
				Objects [i].localPosition = position;
				if (lookForward) {
					Objects [i].LookAt(position + GetDirection(progressTimes [i]));
				}
			}
		}

	}

	private void ShowControlPoints() {

		for (int i = 0; i < points.Count; i++) {
			
			points [i].transform.localScale = curvesControlPoints ?  new Vector3 (5f, 5f, 5f) : Vector3.zero;
			//a.active = false;
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
