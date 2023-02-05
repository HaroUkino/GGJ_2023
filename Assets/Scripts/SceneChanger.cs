using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    [SerializeField] string targetScene;
    [SerializeField] CanvasGroup fader;
    [SerializeField] float fadeTime;
    [SerializeField] UnityEvent onFadedIn;

    SpawnedObjectsTransition[] _interestObjects;

    private void Awake() {
        _interestObjects = FindObjectsOfType<SpawnedObjectsTransition>();
        StartCoroutine( FadeIn() );
    }

    IEnumerator FadeIn() {
        if( fadeTime <= 0 ) fader.alpha = 0;
        while ( fader.alpha > 0 ) {
            fader.alpha -= Time.deltaTime / fadeTime;
            yield return new WaitForEndOfFrame();
        }
        onFadedIn?.Invoke();
    }

    IEnumerator FadeOut() {
        if( fadeTime <= 0 ) fader.alpha = 1;
        while ( fader.alpha < 1 ) {
            fader.alpha += Time.deltaTime / fadeTime;
            yield return new WaitForEndOfFrame();
        }
        if(!string.IsNullOrEmpty( targetScene) )
            SceneManager.LoadScene( targetScene );
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        var player = collision.GetComponent<PlayerConrtoller>();
        if ( player == null ) return;
        player.enabled = false;
        player.TargetMovement.ForceStopMovement();
        if ( _interestObjects != null )
            foreach ( var item in _interestObjects )
                item.Hide();
        StartCoroutine( FadeOut() );
    }

}
