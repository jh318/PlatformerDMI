using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

	public static int tile = 0;

	public TileMap tileMap{
		get{ return target as TileMap; }
	}

	private Rect textureRect;

	public override void OnInspectorGUI(){
		base.OnInspectorGUI ();

		tile = EditorGUILayout.IntField ("Tile", tile);

		Event e = Event.current;

		float w = Screen.width - 40;
		float h = Screen.width * tileMap.texture.height / tileMap.texture.width;
		if (w > tileMap.texture.width) {
			w = tileMap.texture.width;
			h = tileMap.texture.height;
		}

		textureRect = GUILayoutUtility.GetRect (w, h);
		textureRect.width = textureRect.height * tileMap.texture.width / tileMap.texture.height;
		GUI.DrawTexture (textureRect, tileMap.texture);


		Vector2[] uvs = tileMap.GetUVs (tile);
		Vector3[] verts = System.Array.ConvertAll(uvs, (u) => {
			Vector3 v = (Vector3)(textureRect.position);
			v.x += textureRect.width * u.x;
			v.y += textureRect.height * (1 - u.y);
			return v;
		});

		Handles.DrawSolidRectangleWithOutline (verts, Color.clear, Color.white);
		HandleUtility.Repaint ();

		if (e.type == EventType.mouseDown) {
			if (textureRect.Contains (e.mousePosition)) {
			Vector2 pos = e.mousePosition - textureRect.position;
			pos.x *= tileMap.columns / textureRect.width;
			pos.y *= tileMap.rows / textureRect.height;
			pos.y = tileMap.rows - pos.y;
			tile = (int)pos.x + (int)pos.y * tileMap.columns;
			Debug.Log (tile);
			} 
		}
	}

	void OnSceneGUI(){
		Event e = Event.current;

		if (!e.isMouse)
			return;

		int id = GUIUtility.GetControlID (FocusType.Passive);

		Vector3 pos = MouseToWorld();
		int x = (int)pos.x;
		int y = (int)pos.y;
		if (x >= 0 && x < tileMap.width && y >= 0 && y < tileMap.height) {
			if(e.type == EventType.MouseDown || e.type == EventType.MouseDrag){
				if (e.button == 0) {
					tileMap.SetTile (x, y, tile);
				} else if (e.button == 1) {
					tileMap.SetTile (x, y, -1);
				} 
			}
			e.Use ();
			GUIUtility.hotControl = id;
		}


	}

	Vector3 MouseToWorld() {
		Event e = Event.current;
		Ray ray = HandleUtility.GUIPointToWorldRay (e.mousePosition);
		Plane plane = new Plane (Vector3.forward, tileMap.transform.position);
		float dist = 0;
		if (plane.Raycast (ray, out dist)) {
			return ray.GetPoint (dist) - tileMap.transform.position;	
		}
		return Vector3.zero;
	}
	

}
