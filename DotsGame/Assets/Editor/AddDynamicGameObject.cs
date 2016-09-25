using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AddDynamicGameObject
{
	[MenuItem("Tools/CreateDynamicGroup")]
	static void CreateDynamicGroup ()
	{
		if(!GameObject.Find("_Dynamic"))
		{
			GameObject _Dynamic = new GameObject("_Dynamic");
			_Dynamic.transform.position = new Vector3(0, 0, 0);
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}
	}
}
