using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RainbowRoll : MonoBehaviour {

	public SpriteRenderer srImage;
	public Material particleMaterial;

	void Update(){
		Texture2D tex = srImage.sprite.texture;
		Rect rect = srImage.sprite.rect;
		Vector2 offset = new Vector2(rect.x / (float)tex.width, rect.y / (float)tex.height);
		Vector2 scale = new Vector2(rect.width / (float)tex.width, rect.height / (float)tex.height);
		particleMaterial.SetTexture("_MainTex", tex);
		particleMaterial.SetTextureOffset("_MainTex", offset);
		particleMaterial.SetTextureScale("_MainTex", scale);
	}
}
