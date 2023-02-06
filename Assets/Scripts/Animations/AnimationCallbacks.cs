using UnityEngine;
using UnityEngine.Events;

public class AnimationCallbacks : MonoBehaviour {

    [SerializeField] UnityEvent[] animationEvent;

    public void CallEvent( int index ) {
        if ( index >= 0 && index < animationEvent.Length )
            animationEvent[index]?.Invoke();
    }

}
