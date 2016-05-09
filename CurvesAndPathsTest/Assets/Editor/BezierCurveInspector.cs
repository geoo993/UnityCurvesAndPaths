using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveInspector : Editor {

	private BezierCurve curve;
	private Transform handleTransform;
	private Quaternion handleRotation;

	private const int lineSteps = 10;

	private const float directionScale = 0.5f;

	private void OnSceneGUI () {
		curve = target as BezierCurve;
		handleTransform = curve.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			handleTransform.rotation : Quaternion.identity;

		Vector3 p0 = ShowPoint(0);
		Vector3 p1 = ShowPoint(1);
		Vector3 p2 = ShowPoint(2);
		Vector3 p3 = ShowPoint(3);

		Handles.color = Color.yellow;
		Handles.DrawLine(p0, p1);
		Handles.DrawLine(p1, p2);
		Handles.DrawLine(p2, p3);


		switch (curve.bezierType) {

		case BezierCurve.BezierType.Quadratic:
			
				ShowQuadraticCurveDirections ();
				break;
		case BezierCurve.BezierType.Cubic:

				ShowCubicCurveDirections();
				Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
			break;
		}


	}

	private void ShowQuadraticCurveDirections (){

		//Handles.color = Color.white;
		Vector3 lineStart = curve.GetPoint(0f);


		//////add green line that shows the speed with which we move along the curve
		//Handles.color = Color.green;
		//Handles.DrawLine(lineStart, lineStart + curve.GetVelocity(0f)); ////type1
		//Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f)); ////type2

		for (int i = 1; i <= lineSteps; i++) {
			
			Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
			Handles.color = Color.white;
			Handles.DrawLine(lineStart, lineEnd);

			//////add green line that shows the speed with which we move along the curve
			//Handles.color = Color.green;
			//Handles.DrawLine(lineEnd, lineEnd + curve.GetVelocity(i / (float)lineSteps)); ////type1
			//Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection(i / (float)lineSteps));  ////type2
			lineStart = lineEnd;
		}

	}
	private void ShowCubicCurveDirections () {
		
		//////add green line that shows the speed with which we move along the curve
		//Handles.color = Color.green;
		Vector3 point = curve.GetPoint(0f);
		Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
		for (int i = 1; i <= lineSteps; i++) 
		{
			point = curve.GetPoint(i / (float)lineSteps);
			//Handles.DrawLine(point, point + curve.GetDirection(i / (float)lineSteps) * directionScale);
		}
	}

	private Vector3 ShowPoint (int index) {
		Vector3 point = handleTransform.TransformPoint(curve.points[index]);
		EditorGUI.BeginChangeCheck();

		point = Handles.DoPositionHandle(point, handleRotation);

		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(curve, "Move Point");
			EditorUtility.SetDirty(curve);
			curve.points[index] = handleTransform.InverseTransformPoint(point);
		}
		return point;
	}
}