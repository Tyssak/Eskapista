using UnityEngine;
using UnityEngine.Events;

public class LogicPuzzleManager : MonoBehaviour
{
    [SerializeField] private int numberOfSlots;
    [SerializeField] private int _insertedCorrectly;

    [Header("Completion Events")] public UnityEvent onPuzzleCompletion;

    public void InsertedCorrectPiece()
    {
        _insertedCorrectly++;
        CheckForCompletion();
    }
    
    public void RemovedCorrectPiece() => _insertedCorrectly--;

    private void CheckForCompletion()
    {
        if (_insertedCorrectly == numberOfSlots)
        {
            onPuzzleCompletion.Invoke();
        }
    }
}
