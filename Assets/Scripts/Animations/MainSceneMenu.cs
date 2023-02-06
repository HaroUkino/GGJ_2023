using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainSceneMenu : MonoBehaviour {

    [SerializeField] Image targetImage;
    [SerializeField] float fadeTime = 1;
    [SerializeField] GameObject animationGO;
    [SerializeField] string targetScene;

    bool _triggered;

    void Update() {
        if ( _triggered ) return;
        if ( Keyboard.current.anyKey.wasPressedThisFrame ) {
            TriggerAnimation();
        }
    }

    void TriggerAnimation() {
        _triggered = true;
        StartCoroutine( FadeImage() );
    }

    public void ChangeScene() => SceneManager.LoadScene( targetScene );

    IEnumerator FadeImage() {
        if ( fadeTime <= 0 ) targetImage.color = new Color( targetImage.color.r, targetImage.color.g, targetImage.color.b, 0 );
        while ( targetImage.color.a > 0 ) {
            var c = targetImage.color;
            targetImage.color = new Color( c.r, c.g, c.b, c.a - Time.deltaTime / fadeTime );
            yield return new WaitForEndOfFrame();
        }
        animationGO.SetActive( true );
    }
}
