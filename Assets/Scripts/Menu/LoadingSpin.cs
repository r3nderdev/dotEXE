using UnityEngine;

public class LoadingSpin : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool doneRotating = true;
    public bool glitched = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        if (doneRotating && !glitched)
        {
            Invoke(nameof(RotateSpinnyBoi), 0.08f);
            doneRotating = false;
        }

        else if (glitched)
        {
            Invoke(nameof(GlitchyBoi), Random.Range(0.1f, 0.2f));
        }
    }


    void RotateSpinnyBoi()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, 30f);
        rectTransform.rotation *= rotation;
        doneRotating = true;
    }

    void GlitchyBoi()
    {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
        rectTransform.rotation *= rotation;
    }

}
