using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class DialogueEditor : EditorWindow
{

    public DialogueList dialogueList;
    private int viewIndex = 0;

    private Vector2 scrollPos;
    protected static bool showDialogue = true;

    [MenuItem("Tools/Dialogue/Dialogues Editor %#e")]
    static void Init()
    {
        DialogueEditor window = (DialogueEditor)EditorWindow.GetWindow(typeof(DialogueEditor), true, "DialogueEditor");
        window.Show();
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            dialogueList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(DialogueList)) as DialogueList;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Dialogue Editor", EditorStyles.boldLabel);
        if (dialogueList != null)
        {
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = dialogueList;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenDialogueList();
        }
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = dialogueList;
        }
        GUILayout.EndHorizontal();

        GUI.enabled = dialogueList != null ? false : true;
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        if (GUILayout.Button("Create New Dialogue List", GUILayout.ExpandWidth(false)))
        {
            CreateNewDialogueList();
        }
        if (GUILayout.Button("Open Existing Dialogue List", GUILayout.ExpandWidth(false)))
        {
            OpenDialogueList();
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;

        GUI.enabled = dialogueList == null ? false : true;
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("Create New Dialogue", GUILayout.ExpandWidth(false)))
        {
            AddDialogue();
        }
        GUI.enabled = (dialogueList == null || dialogueList.dialogueList.Count <= 0) ? false : true;
        if (GUILayout.Button("Delete Dialogue", GUILayout.ExpandWidth(false)))
        {
            DeleteDialogue();
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;

        GUILayout.Space(20);
        if (dialogueList != null && dialogueList.dialogueList.Count > 0)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 0)
                    viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < dialogueList.dialogueList.Count)
                {
                    viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
            {
                AddDialogueSentence();
            }
            if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
            {
                DeleteLastDialogueSentence();
            }

            GUILayout.EndHorizontal();
            if (dialogueList.dialogueList == null)
                Debug.Log("wtf");
            if (dialogueList.dialogueList.Count > 0 && dialogueList.dialogueList[viewIndex - 1].dialogue.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, dialogueList.dialogueList.Count);
                EditorGUILayout.LabelField("of   " + dialogueList.dialogueList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                showDialogue = EditorGUILayout.Foldout(showDialogue, "Dialogue", true);
                if (showDialogue)
                {
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                    foreach (DialogueSentence currentDialogueSentence in dialogueList.dialogueList[viewIndex - 1].dialogue)
                    {
                        if (currentDialogueSentence != null)
                        {
                            currentDialogueSentence.name = EditorGUILayout.TextField("Locutor Name", currentDialogueSentence.name as string);
                            currentDialogueSentence.locutorImage = EditorGUILayout.ObjectField("Locutor Icon", currentDialogueSentence.locutorImage, typeof(Texture2D), false) as Texture2D;
                            currentDialogueSentence.soundExpression = EditorGUILayout.ObjectField("Sound Expression", currentDialogueSentence.soundExpression, typeof(AudioClip), false) as AudioClip;
                            currentDialogueSentence.sentence = EditorGUILayout.TextField("Locutor sentence", currentDialogueSentence.sentence as string);

                            GUILayout.Space(10);

                            GUILayout.BeginHorizontal();
                            currentDialogueSentence.playSong = (bool)EditorGUILayout.Toggle("playSong", currentDialogueSentence.playSong, GUILayout.ExpandWidth(false));
                            GUILayout.EndHorizontal();

                            GUILayout.Space(10);
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
            }
            else
            {
                GUILayout.Label("This Dialogue List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(dialogueList);
            foreach(Dialogue currentDialogue in dialogueList.dialogueList)
            {
                if(currentDialogue != null)
                {
                    EditorUtility.SetDirty(currentDialogue);
                }
            }
        }
    }

    void CreateNewDialogueList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        viewIndex = 0;
        dialogueList = ScriptableObjectCreatorTool.CreateDialogueList();
        if (dialogueList)
        {
            dialogueList.dialogueList = new List<Dialogue>();
            string relPath = AssetDatabase.GetAssetPath(dialogueList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenDialogueList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Dialogue List", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            dialogueList = AssetDatabase.LoadAssetAtPath(relPath, typeof(DialogueList)) as DialogueList;
            if (dialogueList.dialogueList == null)
                dialogueList.dialogueList = new List<Dialogue>();
            if (dialogueList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddDialogue()
    {
        Dialogue newDialogue = ScriptableObjectCreatorTool.CreateDialogue(viewIndex);
        if (newDialogue)
        {
            newDialogue.dialogue = new List<DialogueSentence>();
            string relPath = AssetDatabase.GetAssetPath(newDialogue);
            EditorPrefs.SetString("ObjectPath", relPath);

            dialogueList.dialogueList.Add(newDialogue);
            viewIndex = dialogueList.dialogueList.Count;
        }
    }

    void DeleteDialogue()
    {
        dialogueList.dialogueList.RemoveAt(viewIndex - 1);
    }

    void OpenDialogue()
    {
        Debug.Log("OpenDialogue!");
        Dialogue newDialogue = null;
        string absPath = EditorUtility.OpenFilePanel("Select Dialogue", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            newDialogue = AssetDatabase.LoadAssetAtPath(relPath, typeof(Dialogue)) as Dialogue;
            if (newDialogue.dialogue == null)
                newDialogue.dialogue = new List<DialogueSentence>();
            if (dialogueList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
            dialogueList.dialogueList.Add(newDialogue);
            viewIndex = dialogueList.dialogueList.Count;
        }
    }

    void AddDialogueSentence()
    {
        DialogueSentence sentence = new DialogueSentence();
        dialogueList.dialogueList[viewIndex - 1].dialogue.Add(sentence);
    }

    void DeleteLastDialogueSentence()
    {
        int dialogueCount = dialogueList.dialogueList[viewIndex - 1].dialogue.Count;
        dialogueList.dialogueList[viewIndex - 1].dialogue.RemoveAt(dialogueCount - 1);
    }
}
