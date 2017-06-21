using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClipperLib;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class TileMap : MonoBehaviour {

	[HideInInspector] public int width = 100;
	[HideInInspector] public int height = 100;
    public Texture2D texture;
    public int tileWidth = 64;
    public int tileHeight = 64;
    public int tileMargin = 0;
    public int tileSpacing = 0;

	[HideInInspector] public int[] tileArray;
	public List<Vector2>[] polygonArray;

	public int columns {
		get { 
			if (texture == null || tileWidth == 0) return 1;
			return texture.width / tileWidth; 
		}
	}

	public int rows {
		get { 
			if (texture == null || tileHeight == 0) return 1;
			return texture.height / tileHeight; 
		}
	}

	[ContextMenu("InitPolygons")]
	public void InitPolygons () {
		polygonArray = new List<Vector2>[rows * columns];
		polygonArray[0] = new List<Vector2>(new Vector2[] {
			new Vector2(0,0),
			new Vector2(0, 30),
			new Vector2(45, 30)
		});
	}

	public List<Vector2> GetPolygon(int tile){
		if(polygonArray == null) return null;

		return polygonArray[tile];
	}

	public void ResizeTileArray (int w, int h) {
		if (tileArray == null || tileArray.Length != width * height) {
			tileArray = new int[w * h];
		}
		else {
			int[] newTileArray = new int[w * h];
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					int index = Mathf.Min(x, w-1) + Mathf.Min(y, h-1) * w;
					if (index >= newTileArray.Length) continue;
					int tile = tileArray[x + y * width];
					newTileArray[index] = tile;
				}
			}
			tileArray = newTileArray;
		}

		width = w;
		height = h;
	}
	public void SetTile (int x, int y, int tile) {
		if (tileArray == null || tileArray.Length != width * height) {
			ResizeTileArray(width, height);
		}
		tileArray[x + y * width] = tile;
		UpdateMesh();
	}

    public Vector2[] GetUVs (int tile) {
        int row = tile / columns;
        int col = tile % columns;

        float x = (float)(tileMargin + col * (tileWidth + tileSpacing)) / (float)texture.width;
        float y = (float)(tileMargin + row * (tileHeight + tileSpacing)) / (float)texture.height;
        float w = (float)tileWidth / (float)texture.width;
        float h = (float)tileHeight / (float)texture.height;

        return new Vector2[] {
            new Vector2(x,   y  ),
            new Vector2(x,   y+h),
            new Vector2(x+w, y+h),
            new Vector2(x+w, y  )            
        };
    }

	[ContextMenu("UpdateMesh")]
	public void UpdateMesh () {
		List<Vector3> verts = new List<Vector3>();
        List<Vector3> norms = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();
		List<List<IntPoint>> paths = new List<List<IntPoint>>();

        int index = 0;
        for (int x = 0; x < width; x++) {
        	for (int y = 0; y < height; y++) {
                int tile = tileArray[x + y * width];
        		if (tile <= 0) continue;

        		Vector3 pos = new Vector3(x, y, 0);
        		verts.AddRange(new Vector3[] {
        			pos, 
        			pos + Vector3.up,
        			pos + new Vector3(1,1,0),
        			pos + Vector3.right
        		});

				List<Vector2> poly = GetPolygon(tile-1);
				if (poly != null && poly.Count > 0) {
					List<IntPoint> points = new List<IntPoint>();
					for (int i = 0; i < poly.Count; i++) {
						Vector2 pt = poly[i];
						points.Add(new IntPoint(x * tileWidth + pt.x, y * tileHeight + pt.y));
					}
					paths.Add(points);
				}

        		norms.AddRange(new Vector3[] {
        			Vector3.back,
        			Vector3.back,
        			Vector3.back,
        			Vector3.back
        		});

        		uvs.AddRange(GetUVs(tile-1));

        		colors.AddRange(new Color[] {
        			Color.white,
        			Color.white,
        			Color.white,
        			Color.white
        		});

        		tris.AddRange(new int[] {
        			index, index + 1, index + 2,
        			index, index + 2, index + 3
       			});

       			index += 4;
        	}
        }

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.colors = colors.ToArray();
        mesh.normals = norms.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = tris.ToArray();

        meshFilter.sharedMesh = mesh;

		paths = Clipper.SimplifyPolygons(paths);
		PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();
		polygon.pathCount = paths.Count;
		for (int i = 0; i < polygon.pathCount; i++) {
			List<IntPoint> path = RemoveColinear(paths[i]);
			Vector2[] points = new Vector2[path.Count];
			for (int j = 0; j < points.Length; j++) {
				points[j] = new Vector2(
					(float)path[j].X / (float)tileWidth, 
					(float)path[j].Y / (float)tileHeight
				);
			}
			polygon.SetPath(i, points);
		}
	}

	List<IntPoint> RemoveColinear (List<IntPoint> path) {
		List<IntPoint> newPath = new List<IntPoint>(path);
		for (int i = 1; i < path.Count-1; i++) {
			Vector2 a = new Vector2(path[i].X - path[i-1].X, path[i].Y - path[i-1].Y);
			Vector2 b = new Vector2(path[i+1].X - path[i].X, path[i+1].Y - path[i].Y);
			float dot = Vector2.Dot(a.normalized, b.normalized);
			if (dot > 0.9f) newPath.Remove(path[i]);
		}
		return newPath;
	}
}
