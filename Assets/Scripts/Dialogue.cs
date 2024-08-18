using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private float textSpeed;

    public int index;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueBox.activeSelf == true)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(string[] strings)
    {
        if (strings.Length > 0)
        {
            textComponent.text = string.Empty;
            lines = strings;

            index = 0;

            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        dialogueBox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueBox.SetActive(false);
            index = 0;
        }
    }
}
