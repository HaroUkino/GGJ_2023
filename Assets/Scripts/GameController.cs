using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

    [SerializeField] UnityEvent onPlayerDeath;
    IResetable[] _resetables;

    private void Awake() => FindResetables();
    
    [ContextMenu("Find all resetables")]
    void FindResetables() => _resetables = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>().ToArray();

    public void ReestAllResetables() {
        foreach ( var resetable in _resetables ) 
            resetable.ResetPosition();
    }
}
