//John Hawley
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
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

	public int columns{
		get { 
			if (texture == null || tileWidth == 0)
				return 1;
			return texture.width / tileWidth; 
		}
	}

	public int rows{
		get { 
			if (texture == null || tileHeight == 0)
				return 1;
			return texture.height / tileHeight; 
		}
	}

	public void SetTile(int x, int y, int tile){
		if (tileArray == null || tileArray.Length != (width * height)) {
			tileArray = new int[width * height];
		}

		tileArray [x + y * width] = tile;
		Setup ();
	}

	public Vector2[] GetUVs(int tile){
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
		List<List<IntPoint>> paths = new List<List<IntPoint>> ();

		int index = 0;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int tile = tileArray [x + y * width];
				if (tile < 0) continue;

				Vector3 pos = new Vector3 (x, y, 0);
				verts.AddRange (new Vector3[] {
					new Vector3(x, y, 0),
					new Vector3(x, y+1, 0),
					new Vector3(x+1, y+1, 0),
					new Vector3(x+1, y, 0)
				});
		
				List<IntPoint> points = new List<IntPoint> ();

				points.Add (new IntPoint (x, y));
				points.Add (new IntPoint (x, y + 1));
				points.Add (new IntPoint (x + 1, y + 1));
				points.Add (new IntPoint (x + 1, y));
				paths.Add (points);


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

		paths = Clipper.SimplifyPolygons (paths);
		PolygonCollider2D polygon = GetComponent<PolygonCollider2D> ();
		polygon.pathCount = paths.Count;
		for (int i = 0; i < polygon.pathCount; i++) {
			List<IntPoint> path = RemoveColinear (paths [i]);
			Vector2[] points = new Vector2[paths [i].Count];
			for (int j = 0; j < points.Length; j++) {
				points [j] = new Vector2 (paths [i] [j].X, paths [i] [j].Y);
			}
			polygon.SetPath (i, points);
		}
	}


	List<IntPoint> RemoveColinear (List<IntPoint> path){
		List<IntPoint> newPath = new List<IntPoint> (path);
		for (int i = 1; i < path.Count-1; i++) {
			Vector2 a = new Vector2 (path [i].X - path [i - 1].X, path [i].Y - path [i - 1].Y);
			Vector2 b = new Vector2 (path [i+1].X - path [i].X, path [i+1].Y - path [i].Y);
			float dot = Vector2.Dot (a.normalized, b.normalized);
			if (dot > 0.9f) 
				newPath.Remove (path[i]);
			
		}

		return newPath;
	}
}
