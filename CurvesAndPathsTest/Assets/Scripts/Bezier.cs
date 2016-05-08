using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Bezier {

	////connecting points with line
//	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
//		return Vector3.Lerp(p0, p2, t);
//	}

	////connecting point 1 and point 2 and use last point as control point of the bezier
	//// This kind of curve is known as a quadratic Beziér curve 
	//// however, this is really just the linear curve with P0 and P1 replaced by two new linear curves
//	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
//		return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
//	}

	////this is the quadratic bezier formula instead of the above three calls to Vector3.Lerp
	//// this is the orignal polynomial function of a quadratic bezier

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
