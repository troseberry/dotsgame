using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

public class LevelSetupWindow : EditorWindow
{
    private static string inputedLineList;
    private static string inputedBoardTitle;

	[MenuItem("Tools/LevelSetup")]
    public static void ShowWindow ()
    {
        GetWindow<LevelSetupWindow>(false, "Level Setup", true);
    }

    void OnGUI ()
    {
        EditorGUILayout.Space();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add All Line Listeners", GUILayout.Width(150), GUILayout.Height(15)))
        {
            AddListenersToLines();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();


        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Lines:", GUILayout.Width(50));
        inputedLineList = EditorGUILayout.TextField(inputedLineList);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create Static Lines", GUILayout.Width(150), GUILayout.Height(15)))
        {
            MarkLinesAsStatic();
            //inputedLineList = "";
        }

        if (GUILayout.Button("Delete Static Lines", GUILayout.Width(150), GUILayout.Height(15)))
        {
            UnmarkLinesAsStatic();
            //inputedLineList = "";
        }
        //GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();


        /*GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Board Title:", GUILayout.Width(50));
        inputedBoardTitle = EditorGUILayout.TextField(inputedBoardTitle);
        GUILayout.EndHorizontal();*/

        /*GUILayout.BeginHorizontal();
        if (GUILayout.Button("Set Board Title", GUILayout.Width(150), GUILayout.Height(15)))
        {
            SetBoardTitle();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();*/


    }

    public static void AddListenersToLines ()
    {
        CampaignPlayerController playerController = GameObject.Find("GameManager").GetComponent<CampaignPlayerController>();
        //Debug.Log("Player Controller: " + playerController);
      
        GameObject[] lineButtonsInLevel = GameObject.FindGameObjectsWithTag("LinePlacement");
        //Debug.Log("Line Buttons: " + lineButtonsInLevel.Length);


        foreach (GameObject obj in lineButtonsInLevel)
        {
            UnityEvent btnOnClick = obj.GetComponent<Button>().onClick;

            //Remove 2 empty listeners that are part of the prefab
            UnityEventTools.RemovePersistentListener(btnOnClick, 0);
            UnityEventTools.RemovePersistentListener(btnOnClick, 0);

            //Create Actions for methods to add
            UnityAction drawMethod = new UnityAction(playerController.PlayerDrawLine);
            UnityAction destroyMethod = new UnityAction(playerController.DestroyStaticLine);

            //Add persistent listeners in editor
            UnityEventTools.AddPersistentListener( btnOnClick, drawMethod );
            UnityEventTools.AddPersistentListener( btnOnClick, destroyMethod );
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Added listeners to " + lineButtonsInLevel.Length + " line buttons.");
    }



    public static void MarkLinesAsStatic ()
    {
        inputedLineList = inputedLineList.Replace(" ", string.Empty);
        Debug.Log(inputedLineList);

        string[] staticLinesArray =  inputedLineList.Split(',');

        foreach (string line in staticLinesArray)
        {
            Debug.Log("Line to Make Static: " + line);
            GameObject current = GameObject.Find("Line_" + line);

            //If that line exists in the scene
            if(current)
            {
                current.GetComponent<SpriteRenderer>().enabled = true;
                current.transform.GetComponentInChildren<Line>().isStatic = true;
            }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    public static void UnmarkLinesAsStatic ()
    {
        inputedLineList = inputedLineList.Replace(" ", string.Empty);
        Debug.Log(inputedLineList);

        string[] staticLinesArray =  inputedLineList.Split(',');

        foreach (string line in staticLinesArray)
        {
            Debug.Log("Line to Make Static: " + line);
            GameObject current = GameObject.Find("Line_" + line);

            //If that line exists in the scene
            if(current)
            {
                current.GetComponent<SpriteRenderer>().enabled = false;
                current.transform.GetComponentInChildren<Line>().isStatic = false;
            }
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }


   /* public static void SetBoardTitle ()
    {
        Text boardTitleText = GameObject.Find("LevelTitle").GetComponentInChildren<Text>();

        string sceneName = EditorSceneManager.GetActiveScene().name;
        string toNameBoard = sceneName.Split('_')[1];

        Debug.Log(toNameBoard);

        //boardTitleText.text = "Board " + toNameBoard;

        //EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());


        SerializedObject serialzedBoardTitleText = new UnityEditor.SerializedObject(boardTitleText);
        SerializedProperty serialzedToNameBoard = serialzedBoardTitleText.FindProperty("text");

        Debug.Log("Serialized Property Exists: " + serialzedToNameBoard);

        serialzedToNameBoard.stringValue = toNameBoard;

        //serialzedBoardTitleText.Update();
        serialzedBoardTitleText.ApplyModifiedProperties();
        
    }*/
}
