//John Hawley
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMesh : MonoBehaviour {

	public int tileCount = 20;

	[ContextMenu("Setup Mesh")]
	void SetupMesh(){
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		List<Vector3> verts = new List<Vector3> ();
		List<Vector2> uvs = new List<Vector2> ();
		List<int> triangles = new List<int> ();

		int index = 0;
		for (int x = 0; x < tileCount; x++) {
			for (int y = 0; y < tileCount; y++) {
				verts.AddRange (new Vector3[] {
					new Vector3(x, y, 0),
					new Vector3(x, y+1, 0),
					new Vector3(x+1, y+1, 0),
					new Vector3(x+1, y, 0)
				});
		

			uvs.AddRange(new Vector2[]{
				Vector2.zero,
				Vector2.up,
				Vector2.one,
				Vector2.right
			});

			
			triangles.AddRange(new int[]{
				index, index+1, index+2,
				index, index+2, index+3
			});
			index += 4;

			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();

		mesh.triangles = triangles.ToArray();
		meshFilter.sharedMesh = mesh;



	}
}
