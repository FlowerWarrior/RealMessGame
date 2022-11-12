using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorUI : MonoBehaviour
{
    [SerializeField] Image imgCursor;
    [SerializeField] GameObject txtPickup;

    void UpdateOutline(Outline activeOutline)
    {
        if (activeOutline == null)
        {
            imgCursor.color = new Color32(255, 255, 255, 100);
            txtPickup.SetActive(false);
        }
        else
        {
            imgCursor.color = new Color32(255, 255, 255, 180);
            txtPickup.SetActive(true);
        }
    }

    private void OnEnable()
    {
        SC_Interactor.UpdateOutlines += UpdateOutline;
    }

    private void OnDisable()
    {
        SC_Interactor.UpdateOutlines -= UpdateOutline;
    }
}
