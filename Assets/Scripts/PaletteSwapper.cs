using UnityEngine;
using System.Collections;

public class PaletteSwapper : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public ColorPalette[] palettes;

	private Texture2D texture;
	private MaterialPropertyBlock block;

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start () {
		if (palettes.Length > 0){
			SwapColors(palettes[Random.Range(0, palettes.Length)]);
		}

	}

	void SwapColors(ColorPalette palette){

		if (palette.cachedTexture == null) {

			//Copying Texture
			texture = spriteRenderer.sprite.texture;
			
			var w = texture.width;
			var h = texture.height;
			
			var cloneTexture = new Texture2D(w, h);
			cloneTexture.wrapMode = TextureWrapMode.Clamp;
			cloneTexture.filterMode = FilterMode.Point;
			
			var colors = texture.GetPixels();

			for(int i = 0; i < colors.Length; i++){
				colors[i] = palette.GetColor(colors[i]);
			}

			cloneTexture.SetPixels(colors);
			cloneTexture.Apply();

			palette.cachedTexture = cloneTexture;
		}

		
		
		block = new MaterialPropertyBlock();
		block.AddTexture("_MainTex", palette.cachedTexture);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate(){
		spriteRenderer.SetPropertyBlock(block);
	}
}
