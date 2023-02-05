using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : MonoBehaviour {

    [SerializeField] float animationSpd;

    public void BeginReset( Transform target, System.Action onEndWaitTime = null ) => StartCoroutine( ResetAnim( target, onEndWaitTime ) );

    IEnumerator ResetAnim( Transform target, System.Action onEndWaitTime ) {
        float scale = 1;
        while ( scale > 0 ) {
            scale = Mathf.Clamp01( scale - Time.deltaTime * animationSpd );
            target.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds( .5f );
        onEndWaitTime?.Invoke();
        while ( scale < 1 ) {
            scale = Mathf.Clamp01( scale + Time.deltaTime * animationSpd );
            target.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
    }

}
