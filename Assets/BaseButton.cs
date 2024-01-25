using UnityEngine;
using UnityEngine.Events;

public abstract class BaseButton : MonoBehaviour 
{
    public GameObject button;
    public UnityEvent onPress;
    private GameObject _presser;
    private AudioSource _sound;
    private bool _isPressed;
    public UnityEvent onRelease;

    void Start()
    {
        _sound = GetComponent<AudioSource>();
        _isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isPressed) 
        {
            button.transform.localPosition = new Vector3(0, -0.4f, 0);
            _presser = other.gameObject;
            onPress.Invoke();
            _sound.Play();
            _isPressed = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _presser)
        {
            button.transform.localPosition = new Vector3(0, 0, 0);
            onRelease.Invoke();
            
            ButtonAction();
            
            _isPressed = false;
        }
    }

    protected abstract void ButtonAction();
}