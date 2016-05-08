using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BezierSpline : MonoBehaviour {


//	public enum BezierControlPointMode {
//		Free,
//		Aligned,
//		Mirrored
//	}
//
	[SerializeField] private Vector3[] points;

//	[SerializeField] private BezierControlPointMode[] modes;
//
//
//	public BezierControlPointMode GetControlPointMode (int index) {
//		return modes[(index + 1) / 3];
//	}
//
//	public void SetControlPointMode (int index, BezierControlPointMode mode) {
//		modes[(index + 1) / 3] = mode;
//	}

	public int ControlPointCount {
		get {
			return points.Length;
		}
	}

	public Vector3 GetControlPoint (int index) {
		return points[index];
	}

	public void SetControlPoint (int index, Vector3 point) {
		points[index] = point;
	}
		

	public int CurveCount {
		get {
			return (points.Length - 1) / 3;
		}
	}


	public Vector3 GetPoint (float t) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
		return	transform.TransformPoint (
				Bezier.GetPointForCubicCurve (points [i], points [i + 1], points [i + 2], points [i + 3], t));

	}

	////show how the line changes or breaks along the curve overtime using this velocity
	public Vector3 GetVelocity (float t) {

		int i;
		if (t >= 1f) {
			t = 1f;
			i = points.Length - 4;
		}
		else {
			t = Mathf.Clamp01(t) * CurveCount;
			i = (int)t;
			t -= i;
			i *= 3;
		}
			
		return transform.TransformPoint(
				Bezier.GetFirstDerivativeForCubicCurve(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - 
				transform.position;
		
	}

	public void AddCurve () {
		Vector3 point = points[points.Length - 1];
		Array.Resize(ref points, points.Length + 3);
		point.x += 1f;
		points[points.Length - 3] = point;
		point.x += 1f;
		points[points.Length - 2] = point;
		point.x += 1f;
		points[points.Length - 1] = point;


//		Array.Resize(ref modes, modes.Length + 1);
//		modes[modes.Length - 1] = modes[modes.Length - 2];
//
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

//		modes = new BezierControlPointMode[] {
//			BezierControlPointMode.Free,
//			BezierControlPointMode.Free
//		};

	}


	void Awake () {

		this.name = "Spline";
	}
}