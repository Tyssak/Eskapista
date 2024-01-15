using UnityEngine;

public class LogicPuzzleManager : MonoBehaviour
{
    [SerializeField] private int numberOfSlots;
    [SerializeField] private Animator _animator;
    private int _insertedCorrectly;
    private AudioSource _sound;
    private bool _solved;
    private static readonly int LogicPuzzleSolved = Animator.StringToHash("LogicPuzzleSolved");

    void Start()
    {
        _sound = GetComponent<AudioSource>();
    }

    public void InsertedCorrectPiece()
    {
        _insertedCorrectly++;
        CheckForCompletion();
    }
    
    public void RemovedCorrectPiece() => _insertedCorrectly--;

    private void CheckForCompletion()
    {
        if (!_solved && _insertedCorrectly == numberOfSlots)
        {
            _solved = true;
            _sound.Play();
            _animator.SetBool(LogicPuzzleSolved, true);
        }
    }
}
