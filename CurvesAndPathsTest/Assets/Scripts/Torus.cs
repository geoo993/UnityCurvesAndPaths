using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class Torus : MonoBehaviour {

	[Range(1.0f, 20.0f)] public float curveRadius, pipeRadius;
	[Range(0,100)] public int curveSegmentCount, pipeSegmentCount;

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;


	void Awake () {

		this.name = "Pipe";
	}

	void Update () {

		CreateMesh ();
	}


//	private void OnDrawGizmos () {
//		
//
//		float uStep = (2f * Mathf.PI) / curveSegmentCount;
//		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
//
//		for (int u = 0; u < curveSegmentCount; u++) {
//			for (int v = 0; v < pipeSegmentCount; v++) {
//				Vector3 point = GetPointOnTorus(u * uStep, v * vStep);
//				Gizmos.color = new Color(
//					1f,
//					(float)v / pipeSegmentCount,
//					(float)u / curveSegmentCount);
//				Gizmos.DrawSphere(point, 0.1f);
//			}
//		}
//
//	}

	private void CreateMesh(){

		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter == null)
		{
			Debug.LogError("MeshFilter not found!");
			return;
		}

		mesh = filter.sharedMesh;
		if (mesh == null){
			filter.mesh = new Mesh();
			mesh = filter.sharedMesh;
		}
		mesh.name = "Pipe Mesh";
		mesh.Clear ();

		SetVertices();
		SetTriangles();
		SetMeshRenderer ();
		//SetMeshCollider ();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}
	private void SetVertices () 
	{
		int numberOfVertices = pipeSegmentCount * curveSegmentCount * 4;
		vertices = new Vector3[numberOfVertices];
		float uStep = (2f * Mathf.PI) / curveSegmentCount;
		CreateFirstQuadRing(uStep);
		int iDelta = pipeSegmentCount * 4;
		for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
			CreateQuadRing(u * uStep, i);
		}
		mesh.vertices = vertices;

	}

	private void SetTriangles () 
	{
		int numberOfTriangles = pipeSegmentCount * curveSegmentCount * 6;
		triangles = new int[numberOfTriangles];
		for (int t = 0, i = 0; t < triangles.Length; t += 6, i += 4) {
			triangles[t] = i;
			triangles[t + 1] = triangles[t + 4] = i + 1;
			triangles[t + 2] = triangles[t + 3] = i + 2;
			triangles[t + 5] = i + 3;
		}
		mesh.triangles = triangles;

	}

	private void SetMeshRenderer()
	{
		MeshRenderer meshRenderer = GetComponent (typeof(MeshRenderer)) as MeshRenderer;
		Material material = new Material (Shader.Find ("Standard"));
		material.color = Color.white;
		meshRenderer.material = material;
	}
//	private void SetMeshCollider()
//	{
//		MeshCollider meshCollider = gameObject.AddComponent (typeof(MeshCollider)) as MeshCollider;
//		meshCollider.sharedMesh = mesh;
//	}

	private Vector3 GetPointOnTorus (float u, float v) {
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos(v));
		p.x = r * Mathf.Sin(u);
		p.y = r * Mathf.Cos(u);
		p.z = pipeRadius * Mathf.Sin(v);
		return p;
	}
	private void CreateFirstQuadRing (float u) 
	{
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;

		Vector3 vertexA = GetPointOnTorus(0f, 0f);
		Vector3 vertexB = GetPointOnTorus(u, 0f);
		for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4) {
			vertices[i] = vertexA;
			vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
			vertices[i + 2] = vertexB;
			vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
		}
	}
	private void CreateQuadRing (float u, int i) 
	{
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		int ringOffset = pipeSegmentCount * 4;

		Vector3 vertex = GetPointOnTorus(u, 0f);
		for (int v = 1; v <= pipeSegmentCount; v++, i += 4) {
			vertices[i] = vertices[i - ringOffset + 2];
			vertices[i + 1] = vertices[i - ringOffset + 3];
			vertices[i + 2] = vertex;
			vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
		}
	}
}
