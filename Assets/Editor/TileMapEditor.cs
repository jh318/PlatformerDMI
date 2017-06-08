using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileMap))]
public class TileMapEditor : Editor {

	public TileMap tileMap{
		get{ return target as TileMap; }
	}

	public override void OnInspectorGUI(){
		base.OnInspectorGUI ();
	}

	void OnSceneGUI(){
		Event e = Event.current;

		if (!e.isMouse)
			return;

		int id = GUIUtility.GetControlID (FocusType.Passive);

		Vector3 pos = MouseToWorld();
		int x = (int)pos.x;
		int y = (int)pos.y;
		if (x > 0 && x < tileMap.width && y >= 0 && y < tileMap.height) {
			if(e.type == EventType.MouseDown || e.type == EventType.MouseDrag){
				tileMap.SetTile (x, y, e.button);
			}
		}

		e.Use ();
		GUIUtility.hotControl = id;
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
