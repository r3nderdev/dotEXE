using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [Header("What Opens The Door")]
    [Tooltip("Enable this to open the door based on kill count.")]
    [SerializeField] private bool killTrigger;

    [Tooltip("Number of kills required to open the door.")]
    [Range(0, 25)]
    [SerializeField] private int killsToOpen;

    [Space(10)]
    [Tooltip("Enable this to open the door based on dialogue.")]
    [SerializeField] private bool dialogueTrigger;

    [Tooltip("Index of the dialogue that triggers the door to open.")]
    [Range(0,20)]
    [SerializeField] private int indexToOpen;

    [Space(10)]
    [Tooltip("Enable to open the door when the player enters the collider.")]
    [SerializeField] private bool colliderTrigger;

    [Header("Door Settings")]
    [Tooltip("Y position to which the door will open.")]
    [SerializeField] private float openPosition = 7f;

    [Tooltip("Speed at which the door opens. 2 is the default value.")]
    [Range(0, 15)]
    [SerializeField] private float openSpeed = 2f;

    [Header("Dialogue On Open")]
    [Tooltip("Reference to the dialogue script.")]
    [SerializeField] private Dialogue dialogueScript;

    [Tooltip("Dialogue to be showed when the door opens. Leave empty for no dialogue.")]
    [SerializeField] private string[] strings;

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
        dialogueScript.StartDialogue(strings);
    }

    IEnumerator OpenDoorByDialogue()
    {
        dialogueScript.StartDialogue(strings);
        yield return new WaitUntil(() => dialogueScript.index == indexToOpen);
        isOpening = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEntered) hasEntered = true;
    }

    IEnumerator OpenDoorByCollider()
    {
        yield return new WaitUntil(() => hasEntered);
        dialogueScript.StartDialogue(strings);
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