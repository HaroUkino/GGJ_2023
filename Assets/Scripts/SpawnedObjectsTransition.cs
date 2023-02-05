using System.Collections;
using UnityEngine;

public class SpawnedObjectsTransition : MonoBehaviour {

    [SerializeField] Transform graphics;
    [SerializeField] float animTime;

    bool _animating;

    private void Awake() {
        graphics.localScale = Vector3.zero;
        Show(); 
    }

    void Show() { if ( !_animating ) StartCoroutine( AniamteTowards( 1 ) ); }
    public void Hide() { if(!_animating) StartCoroutine( AniamteTowards( 0 ) ); }

    IEnumerator AniamteTowards( int target ) {
        _animating = true;
        var targetScale = Vector2.one * target;
        if ( animTime <= 0 ) 
            graphics.localScale = targetScale;
        while ( Vector3.Distance( graphics.localScale, targetScale ) > .01f ) {
            graphics.localScale = Vector3.MoveTowards( graphics.localScale, targetScale, Time.deltaTime/animTime );
            yield return new WaitForEndOfFrame();
        }
        graphics.localScale = targetScale;
        _animating = false;
    }

}
