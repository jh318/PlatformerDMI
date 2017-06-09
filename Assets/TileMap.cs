//John Hawley
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TileMap : MonoBehaviour {

	//public int tileCount = 20;
	public int width = 10;
	public int height = 10;

	public Texture2D texture;
	public int tileWidth = 64;
	public int tileHeight = 64;
	public int tilePadding;
	public int tileSpacing;

	[HideInInspector]public int[] tileArray;


	public void SetTile(int x, int y, int tile){
		if (tileArray == null || tileArray.Length != (width * height)) {
			tileArray = new int[width * height];
		}

		tileArray [x + y * width] = tile;
		Setup ();
	}

	Vector2[] GetUVs(int tile){
		int columns = texture.width / tileWidth;
		int row = tile / columns;
		int column = tile % columns;

		float uvWidth = (float)tileWidth / (float)texture.width;
		float uvHeight = (float)tileHeight / (float)texture.height;

		float uvX = (float) (tilePadding + column * (tileWidth + tileSpacing)) / (float) texture.width;
		float uvY = (float) (tilePadding + row * (tileHeight + tileSpacing)) / (float) texture.height;

		return new Vector2[]{
			new Vector2(uvX, uvY),
			new Vector2(uvX, uvY + uvHeight),
			new Vector2(uvX + uvWidth, uvY + uvHeight),
			new Vector2(uvX + uvWidth, uvY)
		};

	}

	[ContextMenu("Setup")]
	void Setup(){
		MeshFilter meshFilter = GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		List<Vector3> verts = new List<Vector3> ();
		List<Vector2> uvs = new List<Vector2> ();
		List<int> triangles = new List<int> ();

		int index = 0;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int tile = tileArray [x + y * width];
				if (tile < 0) continue;

				verts.AddRange (new Vector3[] {
					new Vector3(x, y, 0),
					new Vector3(x, y+1, 0),
					new Vector3(x+1, y+1, 0),
					new Vector3(x+1, y, 0)
				});
		

				uvs.AddRange(GetUVs(tile));

			
				triangles.AddRange(new int[]{
					index, index+1, index+2,
					index, index+2, index+3
				});

				index += 4;

			}
		}
		Mesh mesh = new Mesh();
		mesh.vertices = verts.ToArray();
		mesh.uv = uvs.ToArray ();

		mesh.triangles = triangles.ToArray();
		meshFilter.sharedMesh = mesh;



	}
}
