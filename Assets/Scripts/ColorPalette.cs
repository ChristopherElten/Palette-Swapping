using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ColorPalette : ScriptableObject {

#if UNITY_EDITOR
	[MenuItem("Assets/Create/Color Palette")]
	public static void CreateColorPalette(){

		//Checking for correct file type
		if(Selection.activeObject is Texture2D){

			var selectedTexture = Selection.activeObject as Texture2D;
			var selectionPath = AssetDatabase.GetAssetPath(selectedTexture);
			//Creating asset path with appended name
			selectionPath = selectionPath.Replace(".png", "-color-palette.asset");
			var newPalette = CustomAssetUtil.CreateAsset<ColorPalette>(selectionPath);

			//Prepopulating setting here
			//Referencing source we are going to edit
			newPalette.source = selectedTexture;
			newPalette.ResetPalette();
			Debug.Log ("Creating a Palette : " + selectionPath);
		} else {
			Debug.Log ("Can't create a Palette");
		}

	}
#endif

	public Texture2D source;
	public List<Color>palette = new List<Color>();
	public List<Color>newPalette = new List<Color>();
	public Texture2D cachedTexture;

	//Sampling color palette
	private List<Color> BuildPalette(Texture2D texture){

		List <Color> palette = new List<Color>();
		//Getting color information from texture
		var colors = texture.GetPixels();

		foreach(var color in colors){
			if (!palette.Contains(color)){
				//Only working with colors we see (alpha of 1)
				if (color.a == 1)
					palette.Add (color);
			}
		}

		return palette;
	}

	//Reset the palette
	public void ResetPalette(){
		palette = BuildPalette(source);
		newPalette = new List<Color> (palette);
	}

	public Color GetColor(Color color){

		for (var i = 0; i < palette.Count; i++){
			var tmpColor = palette[i];

			if ((Mathf.Approximately(color.r, tmpColor.r)) && 
			    (Mathf.Approximately(color.g, tmpColor.g)) &&
			    (Mathf.Approximately(color.b, tmpColor.b)) &&
			    (Mathf.Approximately(color.a, tmpColor.a)))
			 {return newPalette[i];}
		}

		return color;
	}
}



//Editor
#if UNITY_EDITOR
[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor{

	public ColorPalette colorPalette;

	void OnEnable(){
		colorPalette = target as ColorPalette;
	}

	//Renders what is scene in editor (i.e inspector)
	override public void OnInspectorGUI(){

		GUILayout.Label ("Source Texture");

		//Field showing texture we are sampling from
		//input
		colorPalette.source = EditorGUILayout.ObjectField(colorPalette.source, typeof(Texture2D), false) as Texture2D;


		//Horizontal block
		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Current Color");
		GUILayout.Label("New Color");

		EditorGUILayout.EndHorizontal();

		for (var i = 0; i < colorPalette.palette.Count; i++){

			EditorGUILayout.BeginHorizontal();

			//Default color will not be editable
			EditorGUILayout.ColorField(colorPalette.palette[i]);
			//New color will be editable
			colorPalette.newPalette[i] = EditorGUILayout.ColorField(colorPalette.newPalette[i]);


			EditorGUILayout.EndHorizontal();
		}

		if (GUILayout.Button("Revert Palette")){
			colorPalette.ResetPalette();
		}

		//Saving Changes
		EditorUtility.SetDirty(colorPalette);
		
	}
}
#endif
