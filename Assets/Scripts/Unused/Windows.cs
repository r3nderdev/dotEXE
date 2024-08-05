using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour
{
    [SerializeField] private GameObject window;

    public void OnPress()
    {
        if (window.activeSelf == true) window.SetActive(false);
        else if (window.activeSelf == false) window.SetActive(true);
    }
}
