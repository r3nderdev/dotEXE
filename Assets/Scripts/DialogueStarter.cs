using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class DialogueStarter : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private string[] dialogueText;

    private bool done = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && done == false)
        {
            dialogue.StartDialogue(dialogueText);
            gameObject.GetComponent<BoxCollider>().enabled = false;

            done = true;
        }
    }
}
