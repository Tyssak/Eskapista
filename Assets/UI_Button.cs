using System;
using TMPro;
using UnityEngine;

public class UI_Button : BaseButton
{
    public GameObject objectToHide;
    public GameObject textToHide;
    public GameObject sterowaniePng;
    private Boolean hide;

    void Start()
    {
        hide = false;
        textToHide.SetActive(false);
    }

    // Update is called once per frame
    protected override void ButtonAction()
    {
        if(hide == false)
        {
            hide = true;
            sterowaniePng.SetActive(false);
            textToHide.SetActive(true);
        }
        else if (objectToHide != null)
        {
            objectToHide.SetActive(false);  // Set the object inactive to hide it
        }
        else
        {
            Debug.LogWarning("objectToHide is not assigned!");
        }
    }
}
