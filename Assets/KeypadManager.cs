using TMPro;
using UnityEngine;

public class KeypadManager : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    private AudioSource _sound;
    private bool _solved;
    public TMP_Text keyPadCode;
    public string expectedCode;
    private static readonly int CodeCorrect = Animator.StringToHash("codeCorrect");
    private const int MaxCodeLength = 9;
    private int currentCodeLength => keyPadCode.text.Length;

    void Start()
    {
        _sound = GetComponent<AudioSource>();
    }

    public void AppendDigit(char digit)
    {
        if (currentCodeLength < MaxCodeLength)
        {
            keyPadCode.text += digit;
            CheckCorrectness();
        }
    }

    public void PopDigit()
    {
        if (currentCodeLength > 0)
        {
            keyPadCode.text = keyPadCode.text.Remove(currentCodeLength - 1);
            CheckCorrectness();
        }
    }

    public void Clear()
    {
        keyPadCode.text = string.Empty;
    }
    
    private void CheckCorrectness()
    {
        if (!_solved && keyPadCode.text == expectedCode)
        {
            _sound.Play();
            _animator.SetBool(CodeCorrect, true);
        }
    }
}
