using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineObstacle : MonoBehaviour {

    [SerializeField] float timeBeforeFall, animationSpd;
    [SerializeField] Transform graphics;

    IMovable _target;
    float _timer = 0;

    private void Update() {
        if ( _target == null ) return;
        _timer += Time.deltaTime;
        if( _timer >= timeBeforeFall ) {
            _target.Kill();
            _target = null;
            StartCoroutine( Fall() );
        }        
    }

    private void OnTriggerEnter2D( Collider2D collision ) => _target = collision.GetComponent<IMovable>();
    private void OnTriggerExit2D( Collider2D collision ) {
        if ( collision.GetComponent<IMovable>() != null ) {
            _target = null;
            _timer = 0;
        }
    }

    IEnumerator Fall() {
        _timer = 0;
        float scale = 1;
        while( scale > 0 ) {
            scale = Mathf.Clamp01( scale - Time.deltaTime * animationSpd );
            graphics.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds( .5f );
        while ( scale < 1 ) {
            scale = Mathf.Clamp01( scale + Time.deltaTime * animationSpd );
            graphics.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
    }

}
