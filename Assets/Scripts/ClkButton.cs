using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ClkButton : MonoBehaviour
{
    [SerializeField]
    UiController controller;
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
            textMeshPro.text = "Проверка по Циалковксому выкл";
        }
        else
        {
            active = true;
            textMeshPro.text = "Проверка по Циалковксому вкл";
        }

        controller.UseCialkovskiy = active;
    }
}
