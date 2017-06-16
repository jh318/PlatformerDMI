using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
		Debug.Log("HERE");
	}

	private Rect textureRect;
	public override void OnInspectorGUI() {
		EditorGUI.BeginChangeCheck();
		int mapWidth = EditorGUILayout.IntField("Map Width", tileMap.width);
		int mapHeight = EditorGUILayout.IntField("Map Height", tileMap.height);
		if (mapWidth < 1) mapWidth = 1;
		if (mapHeight < 1) mapHeight = 1;
		if (EditorGUI.EndChangeCheck()) {
			Undo.RecordObject(target, "Changed map size");
			tileMap.width = mapWidth;
			tileMap.height = mapHeight;
			tileMap.Setup();
		}

		base.OnInspectorGUI();

		if (tileMap.texture == null) return;

		Event e = Event.current;

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

	int undoGroup = -1;
	void OnSceneGUI() {
		Event e = Event.current;

		if (!e.isMouse) return;

		int id = GUIUtility.GetControlID(FocusType.Passive);

		Vector3 pos = MouseToWorld();
		int x = (int)pos.x;
		int y = (int)pos.y;
		if (x >= 0 && x < tileMap.width && y >= 0 && y < tileMap.height) {
			if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) {
				if (e.type == EventType.MouseDown) {
					Undo.IncrementCurrentGroup();
					undoGroup = Undo.GetCurrentGroup();
				}
				Undo.RecordObject(target, "Sets tilemap");
				if (e.button == 0) {
					tileMap.SetTile(x, y, tile + 1);
				}
				else if (e.button == 1) {
					tileMap.SetTile(x, y, 0);
				}
			}
			e.Use();
			GUIUtility.hotControl = id;
		}
		if (e.type == EventType.MouseUp) {
			if (undoGroup > -1)	Undo.CollapseUndoOperations(undoGroup);
			undoGroup = -1;
		}
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

}
