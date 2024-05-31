using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ClkButton : MonoBehaviour
{
    [SerializeField] GameObject Text;

    TMP_Text textMeshPro;
    bool active = false;

    private void Awake()
    {
        textMeshPro = Text.GetComponent<TMP_Text>();
    }

    public void OnCllick()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }
}
