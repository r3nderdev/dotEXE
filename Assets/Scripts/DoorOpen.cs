using System.Collections;
using UnityEditor;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [Header("Kill Trigger")]
    [HideInInspector] public bool killTrigger;
    [HideInInspector] public int killsToOpen;

    [Header("Dialogue Trigger")]
    [HideInInspector] public bool dialogueTrigger;
    [HideInInspector] public int indexToOpen;

    [Header("Collider Trigger")]
    [HideInInspector] public bool colliderTrigger;

    [Header("Door Opening")]
    [HideInInspector] public float openPosition = 7f;
    [HideInInspector] public float openSpeed = 2f;

    [Header("Dialogue")]
    [HideInInspector] public Dialogue dialogue;
    [HideInInspector] public string[] strings;

    private bool isOpening = false;
    private bool hasEntered = false;


    void Start()
    {
        if (killTrigger) StartCoroutine(OpenDoorByKills());
        else if (dialogueTrigger) StartCoroutine(OpenDoorByDialogue());
        else if (colliderTrigger) StartCoroutine(OpenDoorByCollider());
    }

    IEnumerator OpenDoorByKills()
    {
        yield return new WaitUntil(() => PlayerHealth.killCount >= killsToOpen);
        isOpening = true;
        dialogue.StartDialogue(strings);
    }

    IEnumerator OpenDoorByDialogue()
    {
        dialogue.StartDialogue(strings);
        yield return new WaitUntil(() => dialogue.index == indexToOpen);
        isOpening = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && hasEntered == false) hasEntered = true;
    }

    IEnumerator OpenDoorByCollider()
    {
        yield return new WaitUntil(() => hasEntered == true);
        dialogue.StartDialogue(strings);
        isOpening = true;
    }


    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, openPosition, transform.position.z), openSpeed * Time.deltaTime);
        }
    }
}


[CustomEditor(typeof(DoorOpen))]
public class Door_Editor : Editor
{
    private SerializedProperty stringsProperty;

    public void OnEnable()
    {
        stringsProperty = serializedObject.FindProperty("strings");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var script = (DoorOpen)target;


        EditorGUILayout.LabelField("What Opens The Door", EditorStyles.boldLabel); // Trigger Settings

        script.killTrigger = EditorGUILayout.Toggle("Killcount", script.killTrigger); // Kills as trigger
        if (script.killTrigger)
        {
            script.killsToOpen = EditorGUILayout.IntField("Kills To Open: ", script.killsToOpen);
        }
        script.dialogueTrigger = EditorGUILayout.Toggle("Dialogue Index", script.dialogueTrigger); // Dialogue as trigger
        if (script.dialogueTrigger)
        {
            script.indexToOpen = EditorGUILayout.IntField("Index To Open on:", script.indexToOpen);
        }
        script.colliderTrigger = EditorGUILayout.Toggle("Trigger Collider", script.colliderTrigger); // Collider as trigger

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Door Settings", EditorStyles.boldLabel); // Door Settings
        script.openPosition = EditorGUILayout.FloatField("Open Y Position", script.openPosition);
        script.openSpeed = EditorGUILayout.FloatField("Door Opening Speed", script.openSpeed);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Dialogue", EditorStyles.boldLabel);// Dialogue Settings
        script.dialogue = EditorGUILayout.ObjectField("Dialogue Script:", script.dialogue, typeof(Dialogue), true) as Dialogue; // Dialogue script reference
        EditorGUILayout.PropertyField(stringsProperty, new GUIContent("Dialogue on Open"), true); // String array
        serializedObject.ApplyModifiedProperties();
    }
}
