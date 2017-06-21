using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ClipperLib;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

	public static int tile = 0;
	
	public TileMap tileMap {
		get { return target as TileMap; }
	}

	void OnEnable () {
		Undo.undoRedoPerformed += UndoRedoPerformed;
	}

	void OnDisable () {
		Undo.undoRedoPerformed -= UndoRedoPerformed;
	}

	void UndoRedoPerformed () { 
		tileMap.UpdateMesh();
	}

	private Rect textureRect;
	public override void OnInspectorGUI() {
		Event e = Event.current;
		
		EditorGUI.BeginChangeCheck();
		int mapWidth = EditorGUILayout.IntField("Map Width", tileMap.width);
		int mapHeight = EditorGUILayout.IntField("Map Height", tileMap.height);
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(target, "resize map");
			tileMap.ResizeTileArray(mapWidth, mapHeight);
			tileMap.UpdateMesh();
		}

		base.OnInspectorGUI();

		if (tileMap.texture == null) return;

		float w = Screen.width - 40;
		float h = w * tileMap.texture.height / tileMap.texture.width;
		if (w > tileMap.texture.width) {
			w = tileMap.texture.width;
			h = tileMap.texture.height;
		}
		textureRect = GUILayoutUtility.GetRect(w, h);
		textureRect.width = textureRect.height * tileMap.texture.width / tileMap.texture.height;
		GUI.DrawTexture(textureRect, tileMap.texture);

		Vector2[] uvs = tileMap.GetUVs(tile);
		Vector3[] verts = System.Array.ConvertAll(uvs, (u) => {
			Vector3 v = (Vector3)(textureRect.position);
			v.x += textureRect.width * u.x;
			v.y += textureRect.height * (1 - u.y);
			return v;
		});

		Handles.DrawSolidRectangleWithOutline(verts, Color.clear, Color.white);
		HandleUtility.Repaint();

		if (e.type == EventType.MouseDown) {
			if (textureRect.Contains(e.mousePosition)) {
				Vector2 pos = e.mousePosition - textureRect.position;
				pos.x *= tileMap.columns / textureRect.width;
				pos.y *= tileMap.rows / textureRect.height;
				pos.y = tileMap.rows - pos.y;
				tile = (int)pos.x + (int)pos.y * tileMap.columns;
			}
		}
	}

	void OnSceneGUI() {
		Event e = Event.current;

		if (!e.isMouse) return;

		Vector3 pos = MouseToWorld();
		int x = (int)pos.x;
		int y = (int)pos.y;
		bool paintEvent = (e.button < 2) && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag);
		if (paintEvent && x >= 0 && x < tileMap.width && y >= 0 && y < tileMap.height) {
			if (e.type == EventType.MouseDown) GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
			Undo.RecordObject(target, "resize map");
			if (e.button == 0) {
				tileMap.SetTile(x, y, tile + 1);
			}
			else if (e.button == 1) {
				tileMap.SetTile(x, y, 0);
			}
			e.Use();
		}
		
		if (e.type == EventType.MouseUp) GUIUtility.hotControl = 0;
	}

	Vector3 MouseToWorld () {
		Event e = Event.current;
		Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
		Plane plane = new Plane(Vector3.forward, tileMap.transform.position);
		float dist = 0;
		if (plane.Raycast(ray, out dist)) {
			return ray.GetPoint(dist) - tileMap.transform.position;
		}
		return Vector3.zero;
	}

	public override bool HasPreviewGUI() {
		return true;
	}

	public override GUIContent GetPreviewTitle () {
		return new GUIContent("Collision Editor");
	}

	public override void OnPreviewGUI(Rect r, GUIStyle background) {
		Vector2[] uvs = tileMap.GetUVs(tile);
		Vector2 size = uvs[2] - uvs[0];
		Rect uvRect = new Rect(uvs[0].x, uvs[0].y, size.x, size.y);
		float width = tileMap.texture.width * size.x;
		float height = tileMap.texture.height * size.y;
		r.x += (Screen.width - width) * 0.5f;
		r.y += (r.height - height) * 0.5f;
		r.width = width;
		r.height = height;
		GUI.DrawTextureWithTexCoords(r, tileMap.texture, uvRect, true);

		List<Vector2> points = tileMap.GetPolygon(tile);
		if(points == null) return;
		//tileMap.InitPolygons();

		Vector3[] path = new Vector3[points.Count];
		for(int i = 0; i < path.Length-1; i++){
			path[i] = new Vector3(
				r.x + points[i].x, 
				r.y + tileMap.tileHeight - points[i].y, 
				0);	
		}
		path[path.Length - 1] = path[0];

		Handles.DrawPolyLine(path);
		HandleUtility.Repaint();
	}
}
