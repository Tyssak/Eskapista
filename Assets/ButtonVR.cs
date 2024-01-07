using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ButtonVR : MonoBehaviour
{
    public enum ButtonType
    {
        OnOff,
        Reset,
        Next,
        Prev,
        Reverse,
        Plus,
        Minus
    }

    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;
    public ButtonType buttonType;
    public changePicture pictureScript;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed) 
        {
            button.transform.localPosition = new Vector3(0, -0.4f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0, 0);
            onRelease.Invoke();

            // Call the function in the changePicture script
            if (pictureScript != null)
            {
                pictureScript.HandleButtonRelease(buttonType);
            }
            isPressed = false;
        }
    }

}
