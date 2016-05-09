using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class TorusKnot : MonoBehaviour {


	[Range(1f, 10f)] public float radius = 1f; ////ringRadius
	[Range(0.05f, 1f)]public float tube = 0.3f; ////pipeRadius
	[Range(10, 200)] public int radialSegments = 24; // radius segments count or radial segments ////ringSegments
	[Range(10, 100)] public int tubularSegments = 50;//18 // also known as side segments count or ////pipeSegments

	[Range(0.0f, 5f)] public float heightScale = 4f;
	[Range(1, 10)] public int p = 4; 
	[Range(1, 10)] public int q = 6;

	private Color color;

	void Start ()
	{
		this.name = "Torus Knot";
		color = new Color( Random.value, Random.value, Random.value, 1.0f);

	}

	private void Update ()
	{

		CreateTorusKnot ();
	
	}
	private void CreateTorusKnot()
	{

		List<Vector2> uvs = new List<Vector2>();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> triangles = new List<int>();

		var center = new Vector3();
		int[][] grid = new int[radialSegments][];

		var tang = new Vector3();
		var n = new Vector3();
		var bitan = new Vector3();


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
		mesh.name = "TorusKnotMesh";
		mesh.Clear ();


		for (var i = 0; i < radialSegments; ++i)
		{
			grid[i] = new int[tubularSegments];
			var u = i/(float) radialSegments * 2.0f * p * Mathf.PI;
			var p1 = GetPos(u, q, p, radius, heightScale);
			var p2 = GetPos(u + 0.01f, q, p, radius, heightScale);
			tang = p2 - p1;
			n = p2 + p1;

			bitan = Vector3.Cross(tang, n);
			n = Vector3.Cross(bitan, tang);
			bitan.Normalize();
			n.Normalize();

			for (var j = 0; j < tubularSegments; ++j)
			{
				var v = j/(float) tubularSegments*2.0f*Mathf.PI;
				var cx = -tube * Mathf.Cos(v); 
				var cy = tube * Mathf.Sin(v);

				var vertex = new Vector3();
				vertex.x = p1.x + cx * n.x + cy * bitan.x;
				vertex.y = p1.y + cx * n.y + cy * bitan.y;
				vertex.z = p1.z + cx * n.z + cy * bitan.z;

				vertices.Add(vertex);
				uvs.Add(new Vector2(i/(float) radialSegments, j/(float) tubularSegments));

				Vector3 normal = vertex - center;
				normal.Normalize();
				normals.Add(normal);

				grid[i][j] = vertices.Count - 1;
			}

		}

		for (var i = 0; i < radialSegments; ++i)
		{
			for (var j = 0; j < tubularSegments; ++j)
			{
				var ip = (i + 1)%radialSegments;
				var jp = (j + 1)%tubularSegments;

				var a = grid[i][j];
				var b = grid[ip][j];
				var c = grid[ip][jp];
				var d = grid[i][jp];

				triangles.Add(a);
				triangles.Add(b);
				triangles.Add(d);

				triangles.Add(b);
				triangles.Add(c);
				triangles.Add(d);

			}
		}



		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.triangles = triangles.ToArray();

		mesh.RecalculateNormals();
		CalculateTangent.TangentSolver (mesh);

		mesh.RecalculateBounds();
		mesh.Optimize();

		//MeshRenderer renderer = GetComponent<MeshRenderer> ();
		//renderer.material.color = color;


	}

	private static Vector3 GetPos(float u, float in_q, float in_p, float radius, float heightScale)
	{
		var cu = Mathf.Cos(u);
		var su = Mathf.Sin(u);
		var quOverP = in_q/in_p * u;
		var cs = Mathf.Cos(quOverP);

		var tx = radius * (2.0f + cs) * 0.5f * cu;
		var ty = radius * (2.0f + cs) * su * 0.5f;
		var tz = heightScale * radius * Mathf.Sin(quOverP) * 0.5f;

		return new Vector3(tx, ty, tz);
	}

}
