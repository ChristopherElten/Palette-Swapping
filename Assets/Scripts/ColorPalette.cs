using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[Serializable]
public class ColorPalette : ScriptableObject {

	[MenuItem("Assets/Create/Color Palette")]
	public static void CreateColorPalette(){

		//Checking for correct file type
		if(Selection.activeObject is Texture2D){

			var selectedTexture = Selection.activeObject as Texture2D;
			var selectionPath = AssetDatabase.GetAssetPath(selectedTexture);
			//Creating asset path with appended name
			selectionPath = selectionPath.Replace(".png", "-color-palette.asset");
			var newPalette = CustomAssetUtil.CreateAsset<ColorPalette>(selectionPath);

			Debug.Log ("Creating a Palette : " + selectionPath);
		} else {
			Debug.Log ("Can't create a Palette");
		}

	}
	
}
