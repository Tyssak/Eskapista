using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogicPuzzleSlot : MonoBehaviour
{
    [SerializeField] private LogicPuzzleManager _puzzleManager;
    [SerializeField] private Transform _correctTile;
    private XRSocketInteractor _socketInteractor;

    private void Awake() => _socketInteractor = GetComponent<XRSocketInteractor>();

    private void OnEnable()
    {
        _socketInteractor.selectEntered.AddListener(ObjectSnapped);
        _socketInteractor.selectExited.AddListener(ObjectRemoved);
    }

    private void ObjectSnapped(SelectEnterEventArgs arg0)
    {
        var snappedObjectName = arg0.interactableObject.transform.name;

        if (snappedObjectName == _correctTile.name)
        {
            _puzzleManager.InsertedCorrectPiece();
        }
    }
    
    private void ObjectRemoved(SelectExitEventArgs arg0)
    {
        var removedObjectName = arg0.interactableObject.transform.name;

        if (removedObjectName == _correctTile.name)
        {
            _puzzleManager.RemovedCorrectPiece();
        }
    }
}
