using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Origami : MonoBehaviour {



	private Color color;

	void Start ()
	{
		this.name = "Origami";
		color = new Color( Random.value, Random.value, Random.value, 1.0f);
		CreateCube ();	

	}

	private void CreateCube()
	{

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		if (meshFilter==null){
			Debug.LogError("MeshFilter not found!");
			return;
		}

		Mesh mesh = meshFilter.sharedMesh;
		//mesh = meshFilter.mesh;
		if (mesh == null){
			meshFilter.mesh = new Mesh();
			mesh = meshFilter.sharedMesh;
			//mesh = meshFilter.mesh;
		}

		mesh.Clear();


		Vector3 v0 = new Vector3 (0, 2.5f, 1.5f); //left (top, front, left)
		Vector3 v1 = new Vector3 (6f, 3, 0f); //right (top, front, right)
		Vector3 v2 = new Vector3 (0 , 2.5f, -1.5f); //right (top, back, right)
		Vector3 v3 = new Vector3 (-1f , 0f, 0f); //left (top, back, left)

//		Vector3 v0 = new Vector3 (0, -2.5f, 1.5f); //left (top, front, left)
//		Vector3 v1 = new Vector3 (6f, 3, 0f); //right (top, front, right)
//		Vector3 v2 = new Vector3 (0 , -2.5f, -1.5f); //right (top, back, right)
//		Vector3 v3 = new Vector3 (-1f , 0f, 0); //left (top, back, left)
//
//

		Vector3 v4 = new Vector3 (-1f , 0f, 0f); //left (mid, front, left)
		Vector3 v5 = new Vector3 (1, 0.5f, 1f ); //right (mid, front, right)
		Vector3 v6 = new Vector3 (1 , 0.5f, -1f); //right (mid, back, right)
		Vector3 v7 = new Vector3 (-1f , 0f , 0f); //left (mid, back, left)
		
     
		Vector3 v8 = new Vector3 (-2f, 0.5f, 0 ); //left (bottom, front, left)
		Vector3 v9 = new Vector3 (0, 3f, 4f ); //right (bottom, front, right)
		Vector3 v10 = new Vector3 (-1 , -0.5f, 0); //right (bottom, back, right)
		Vector3 v11 = new Vector3 (0 , 3f , -4f); //left (bottom, back, left)






		//Add region Vertices
		mesh.vertices = new Vector3[]{


			// top Front face 
			v0, v1, v4, v5,

			// top Back face 
			v2, v3, v6, v7,

			// top Left face 
			v3, v0, v7, v4,

			// top Right face
			v1, v2, v5, v6,



			// bottom Front face 
			v4, v5, v8, v9,


			// bottom Back face 
			v6, v7, v10, v11,

			// bottom Left face 
			v7, v4, v11, v8,

			// bottom Right face
			v5, v6, v9, v10,




			// Top face 
			v3, v2, v0, v1,

			// Bottom face 
			v8, v9, v11, v10




		};
		//end vertices region

		//Add Triangles region 
		//these are three point, and work clockwise to determine what side is visible
		mesh.triangles = new int[]{

			// top front face
			0,2,3, // first triangle
			3,1,0, // second triangle

			// top back face
			4,6,7, // first triangle
			7,5,4, // second triangle

			// top left face
			8,10,11, // first triangle
			11,9,8, // second triangle

			// top right face
			12,14,15, // first triangle
			15,13,12, // second triangle


			//bottom front face
			16,18,19, // first triangle
			19,17,16, // second triangle

			//bottom back face
			20,22,23, // first triangle
			23,21,20, // second triangle

			//bottom left face 
			24,26,27, // first triangle
			27,25,24, // second triangle

			//bottom right face 
			28,30,31, // first triangle
			31,29,28, // second triangle



			//top face 
			32,34,35, // first triangle
			35,33,32, // second triangle

			//bottom face 
			36,38,39, // first triangle
			39,37,36 // second triangle

		};
		//end triangles region


		//Normales vector3 region
		Vector3 front 	= Vector3.forward;
		Vector3 back 	= Vector3.back;
		Vector3 left 	= Vector3.left;
		Vector3 right 	= Vector3.right;
		Vector3 up 		= Vector3.up;
		Vector3 down 	= Vector3.down;

		//Add Normales region
		mesh.normals = new Vector3[]
		{
			// top Front face
			front, front, front, front,

			// top Back face
			back, back, back, back,

			// top Left face
			left, left, left, left,

			// top Right face
			right, right, right, right,


			// bottom Front face
			front, front, front, front,

			// bottom Back face
			back, back, back, back,

			// bottom Left face
			left, left, left, left,

			// bottom Right face
			right, right, right, right,



			// Top face
			up, up, up, up,

			// Bottom face
			down, down, down, down

		};
		//end Normales region

		//Add vector2 regions 
		Vector2 u00 = new Vector2( 0f, 0f );
		Vector2 u10 = new Vector2( 1f, 0f );
		Vector2 u01 = new Vector2( 0f, 1f );
		Vector2 u11 = new Vector2( 1f, 1f );

		//Add UVs region 
		mesh.uv = new Vector2[]
		{
			// Front face uv
			u01, u00, u11, u10,

			// Back face uv
			u01, u00, u11, u10,

			// Left face uv
			u01, u00, u11, u10,

			// Right face uv
			u01, u00, u11, u10,



			// Front face uv
			u01, u00, u11, u10,

			// Back face uv
			u01, u00, u11, u10,

			// Left face uv
			u01, u00, u11, u10,

			// Right face uv
			u01, u00, u11, u10,


			// Top face uv
			u01, u00, u11, u10,

			// Bottom face uv
			u01, u00, u11, u10
		};
		//End UVs region
		

//		Texture texture = Resources.Load ("TextureComplete1") as Texture;
//		material.mainTexture = texture;


		//MeshCollider meshCollider = cube.AddComponent<MeshCollider> ();
		//meshCollider.isTrigger = false;


		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();


		MeshRenderer renderer = GetComponent<MeshRenderer> ();
		renderer.material.color = color;
	}


}
