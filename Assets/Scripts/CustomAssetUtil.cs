using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CustomAssetUtil {

	#if UNITY_EDITOR
	public static T CreateAsset<T>(string path) where T : ScriptableObject{

		T asset = null;

		asset = ScriptableObject.CreateInstance<T> ();

		var newPath = AssetDatabase.GenerateUniqueAssetPath(path);

		AssetDatabase.CreateAsset(asset, newPath);

		AssetDatabase.SaveAssets();

		return asset;

	}
#endif
}
