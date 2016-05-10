using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TorusShape : MonoBehaviour {


	[Range(1f, 10f)] public float ringRadius = 1f;
	[Range(0.05f, 1f)]public float pipeRadius = 0.3f;
	[Range(8, 72)] public int ringSegments = 24; // radius segments count or radial segments
	[Range(8, 72)] public int pipeSegments = 50;//18 // also known as side segments count or 

	private Color color;

	void Start ()
	{
		this.name = "Torus";
		color = new Color( Random.value, Random.value, Random.value, 1.0f);


	}
//	private void windowsArray (){
//
//		int[] arr = new int[51];
//
//		for (int i = 0; i < 51; i++) {
//
//			arr[i] = i;
//			//print (arr[i]);
//		}
//
//		for (int a = 0; a < arr.Length/3; a++) {
//
//			print(a * 3);
//		}
//
//	}
	private void Update ()
	{
		CreateTorus ();	

	}

	private void CreateTorus()
	{


		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter == null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		Mesh mesh = filter.sharedMesh;
		if (mesh == null){
			filter.mesh = new Mesh();
			mesh = filter.sharedMesh;
		}
		mesh.Clear ();


		int vertexCount = (ringSegments + 1) * (pipeSegments + 1);

		#region Vertices		
		Vector3[] vertices = new Vector3[vertexCount];

		float _2pi = Mathf.PI * 2f;
		for (int seg = 0; seg <= ringSegments; seg++) {
			
			int currSeg = seg == ringSegments ? 0 : seg;

			float t1 = (float)currSeg / ringSegments * _2pi;
			Vector3 r1 = new Vector3 (Mathf.Sin (t1) * ringRadius, Mathf.Cos (t1) * ringRadius, 0f);

			for (int side = 0; side <= pipeSegments; side++) {
				
				int currSide = side == pipeSegments ? 0 : side;

				Vector3 normale = Vector3.Cross (r1, Vector3.forward);
				float t2 = (float)currSide / pipeSegments * _2pi;
				Vector3 r2 =  Quaternion.AngleAxis (-t1 * Mathf.Rad2Deg, Vector3.forward) * new Vector3 (0f, Mathf.Sin (t2) * pipeRadius, Mathf.Cos (t2) * pipeRadius);

				vertices [side + seg * (pipeSegments + 1)] = r1 + r2;
			}
		}
		#endregion


		#region Normales		
		Vector3[] normales = new Vector3[vertices.Length];
		for (int seg = 0; seg <= ringSegments; seg++) {
			int currSeg = seg == ringSegments ? 0 : seg;

			float t1 = (float)currSeg / ringSegments * _2pi;
			Vector3 r1 = new Vector3 ( Mathf.Sin (t1) * ringRadius, Mathf.Cos (t1) * ringRadius, 0f);

			for (int side = 0; side <= pipeSegments; side++) {
				normales [side + seg * (pipeSegments + 1)] = (vertices [side + seg * (pipeSegments + 1)] - r1).normalized;
			}
		}
		#endregion


		#region UVs
		Vector2[] uvs = new Vector2[vertices.Length];
		for (int seg = 0; seg <= ringSegments; seg++)
			for (int side = 0; side <= pipeSegments; side++)
				uvs [side + seg * (pipeSegments + 1)] = new Vector2 ((float)seg / ringSegments, (float)side / pipeSegments);
		#endregion


		#region Triangles
		int nbFaces = vertices.Length;
		int nbTriangles = nbFaces * 2;
		int nbIndexes = nbTriangles * 3;
		int[] triangles = new int[ nbIndexes ];

		int i = 0;
		for (int seg = 0; seg <= ringSegments; seg++) {			
			for (int side = 0; side <= pipeSegments - 1; side++) {
				int current = side + seg * (pipeSegments + 1);
				int next = side + (seg < (ringSegments) ? (seg + 1) * (pipeSegments + 1) : 0);

				if (i < triangles.Length - 6) {
					triangles [i++] = current;
					triangles [i++] = next;
					triangles [i++] = next + 1;

					triangles [i++] = current;
					triangles [i++] = next + 1;
					triangles [i++] = current + 1;
				
				}
			}
		}
		#endregion



		mesh.vertices = vertices;
		mesh.normals = normales;
		mesh.uv = uvs;
		mesh.triangles = triangles;


		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);

		mesh.RecalculateBounds();
		mesh.Optimize();


		//MeshRenderer renderer = GetComponent<MeshRenderer> ();
		//renderer.material.color = color;

	}



}
