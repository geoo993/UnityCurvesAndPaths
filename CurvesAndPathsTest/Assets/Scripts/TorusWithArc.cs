using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class TorusWithArc : MonoBehaviour {


	[Range(1f, 10f)] public float radius = 1f; ////ringRadius
	[Range(0.05f, 1f)]public float tube = 0.3f; ////pipeRadius
	[Range(8, 72)] public int radialSegments = 24; // radius segments count or radial segments ////ringSegments
	[Range(8, 72)] public int tubularSegments = 50;//18 // also known as side segments count or ////pipeSegments

	[Range(0.0f, 6.28f)] public float arc = 4f;

	private Color color;

	void Start ()
	{
		this.name = "TorusArc";
		color = new Color( Random.value, Random.value, Random.value, 1.0f);


	}

	private void Update ()
	{

		CreateTorus ();
	
	}
	private void CreateTorus()
	{

		List<Vector2> uvs = new List<Vector2>();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<int> triangles = new List<int>();

		var center = new Vector3();

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
		mesh.name = "TorusMesh";
		mesh.Clear ();



		for (var j = 0; j <= radialSegments; j++)
		{
			for (var i = 0; i <= tubularSegments; i++)
			{
				var u = i/(float) tubularSegments * arc;
				var v = j/(float) radialSegments * Mathf.PI * 2.0f;

				center.x = radius * Mathf.Cos(u);
				center.y = radius * Mathf.Sin(u);

				var vertex = new Vector3();
				vertex.x = (radius + tube * Mathf.Cos(v)) * Mathf.Cos(u);
				vertex.y = (radius + tube * Mathf.Cos(v)) * Mathf.Sin(u);
				vertex.z = tube * Mathf.Sin(v);

				vertices.Add(vertex);

				uvs.Add(new Vector2(i/(float) tubularSegments, j/(float) radialSegments));
				Vector3 normal = vertex - center;
				normal.Normalize();
				normals.Add(normal);
			}
		}


		for (var j = 1; j <= radialSegments; j++)
		{
			for (var i = 1; i <= tubularSegments; i++)
			{
				var a = (tubularSegments + 1) * j + i - 1;
				var b = (tubularSegments + 1) * (j - 1) + i - 1;
				var c = (tubularSegments + 1) * (j - 1) + i;
				var d = (tubularSegments + 1) * j + i;

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

		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		renderer.material.color = color;


		//print (vertices.Count);

	}

}
