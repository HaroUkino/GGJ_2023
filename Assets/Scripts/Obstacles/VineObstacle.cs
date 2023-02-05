using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineObstacle : MonoBehaviour {

    [SerializeField] float timeBeforeFall;
    [SerializeField] Transform graphics;
    [SerializeField] ResetAnimation resetAnimation;

    PlayerMovement _target;
    float _timer = 0;

    private void Update() {
        if ( _target == null ) return;
        _timer += Time.deltaTime;
        if( _timer >= timeBeforeFall ) {
            _target.Kill();
            _target = null;
            _timer = 0;
            resetAnimation.BeginReset( graphics );
        }        
    }

    private void OnTriggerEnter2D( Collider2D collision ) => _target = collision.GetComponent<PlayerMovement>();
    private void OnTriggerExit2D( Collider2D collision ) {
        if ( collision.GetComponent<IMovable>() != null ) {
            _target = null;
            _timer = 0;
        }
    }
}
