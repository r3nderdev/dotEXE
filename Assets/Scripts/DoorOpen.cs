using System.Collections;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private bool useKills;
    [SerializeField] private int killsToOpen;
    [SerializeField] private int indexToOpen;
    [SerializeField] private float openPosition;
    [SerializeField] private float openSpeed = 2f;


    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private string[] strings;

    private bool isOpening = false;

    void Start()
    {
        if (useKills) StartCoroutine(OpenDoorByKills());
        else StartCoroutine(OpenDoorByDialogue());
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

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, openPosition, transform.position.z), openSpeed * Time.deltaTime);
        }
    }
}
