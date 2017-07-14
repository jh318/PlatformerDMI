using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public static Spawner instance;

	public GameObject[] prefabs;

	private Dictionary<string, GameObject> prefabsDict;
	private Dictionary<string, List<GameObject>> poolsDict;

	void Awake () {
		if (instance == null) {
			instance = this;
			prefabsDict = new Dictionary<string, GameObject>();
			poolsDict = new Dictionary<string, List<GameObject>>();

			foreach (GameObject prefab in prefabs) {
				poolsDict[prefab.name] = new List<GameObject>();
				prefabsDict[prefab.name] = prefab;
			}
		}
	}

	public static void ResetPools() {
		instance.poolsDict = new Dictionary<string, List<GameObject>>();

		foreach (GameObject prefab in instance.prefabs) {
			instance.poolsDict[prefab.name] = new List<GameObject>();
		}
	}

	public static GameObject Spawn(string name) {
		if (instance == null || 
			string.IsNullOrEmpty(name) ||
			!instance.poolsDict.ContainsKey(name) ||
			!instance.prefabsDict.ContainsKey(name)) {

			return null;
		}

		List<GameObject> pool = instance.poolsDict[name];
		GameObject g = pool.Find((i) => i != null && !i.activeSelf);
		if (g == null) {
			g = Instantiate(instance.prefabsDict[name]) as GameObject;
			pool.Add(g);
		}

		g.SetActive(true);

		return g;
	}
}