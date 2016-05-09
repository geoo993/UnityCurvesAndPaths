using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public int frequency;
	[Range( 2f, 20f)]public float duration = 10f;

	public bool lookForward;
	public bool stopMovement;

	public Transform[] items;

	private List<Transform> Objects = new List<Transform> ();

	private List<float> progressTimes = new List<float>();


	public SplineDecoratorMode modeFromBegining;

	private void Awake () {

		this.name = "Decorator";

		if (frequency <= 0 || items == null || items.Length == 0) {
			return;
		}
		float stepSize = frequency * items.Length;
		if (spline.Loop || stepSize == 1) {
			stepSize = 1f / stepSize;
		}
		else {
			stepSize = 1f / (stepSize - 1);
		}


		//float stepSize = 1f / (frequency * items.Length);
		for (int p = 0, f = 0; f < frequency; f++) {
			for (int i = 0; i < items.Length; i++, p++) {

				//print (p * stepSize);

				Transform item = Instantiate(items[i]) as Transform;
				Vector3 position = spline.GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) {
					item.transform.LookAt(position + spline.GetDirection(p * stepSize));
				}
				item.transform.parent = transform;

				Objects.Add (item);

				progressTimes.Add (p * stepSize);

			}
		}
	}



	private void Update()
	{

		if (!stopMovement) {
			
			for (int i = 0; i < Objects.Count; i++) {

				progressTimes [i] += Time.deltaTime / duration;
				if (progressTimes [i] > 1f) {
					if (modeFromBegining == SplineDecoratorMode.Once) {
						progressTimes [i] = 1f;
					} else if (modeFromBegining == SplineDecoratorMode.Loop) {
						progressTimes [i] -= 1f;
					} 
				}

				Vector3 position = spline.GetPoint (progressTimes [i]);
				Objects [i].localPosition = position;
				if (lookForward) {
					Objects [i].LookAt(position + spline.GetDirection(progressTimes [i]));
				}
			}
		}




	}

}
