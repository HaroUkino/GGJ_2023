using System.Collections;
using UnityEngine;

public class PlayerMovement : CharacterMovement, IResetable {

    [SerializeField] GameController gameController;

    Vector2 _startPosition;
    
    PlayerAnimation _anim;
    Collider2D _collider;

    void Awake() {
        transform.position = Vector3Int.FloorToInt( transform.position );
        _anim = GetComponent<PlayerAnimation>();
        _startPosition = transform.position;
        _collider = GetComponent<Collider2D>();
        onDirectionChanged += PlayCurrentAnim;
    }

    string GetPlayerDirection() {
        string dir = PlayerAnimation.MOVE_DOWN;
        if ( direction.x > 0 ) dir = PlayerAnimation.MOVE_RIGHT;
        if ( direction.x < 0 ) dir = PlayerAnimation.MOVE_LEFT;
        if ( direction.y > 0 ) dir = PlayerAnimation.MOVE_UP;
        return dir;
    }

    public void SetVehicle( PlayerVehicle vehicle ) {
        if ( Vehicle != null ) return;
        Vehicle = vehicle;
        if ( vehicle ) {
            Vehicle.transform.SetParent( transform );
            vehicle.gameObject.SetActive( false );
        }
        PlayCurrentAnim();
    }

    void DismountVehicle() {
        Vehicle.transform.SetParent( null );
        Vehicle.transform.position = Vector3Int.RoundToInt( transform.position );
        Vehicle.gameObject.SetActive( true );
        Vehicle.Dismount( transform );
        Vehicle = null;
        PlayCurrentAnim();
    }

    public void Interact() {
        if ( Vehicle == null ) return;
        DismountVehicle();
    }

    void PlayCurrentAnim() => _anim.PlayAnimation( $"{( Vehicle != null ? Vehicle.VehicleAnimTag + " " : "")}{GetPlayerDirection()}" );

    public void ForceStopMovement() => StopMovement();

    [ContextMenu( "Kill" )]
    public void Kill() {
        if ( lockMovement ) return;
        Debug.Log( "player diededed" );
        StopMovement();
        Interact();
        gameController?.ReestAllResetables();
    }

    IEnumerator Fall() {
        lockMovement = true;
        _collider.enabled = false;
        float scale = 1;
        var sprTransform = sprRenderer.transform;
        while ( scale > 0 ) {
            scale = Mathf.Clamp01( scale - Time.deltaTime );
            sprTransform.localScale = new(scale,scale);
            sprTransform.Rotate( Vector3.forward, -90 * Time.deltaTime );
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds( .5f );
        sprTransform.eulerAngles = new( 0, 0, 0 );
        transform.position = _startPosition;
        while ( scale < 1 ) {
            scale = Mathf.Clamp01( scale + Time.deltaTime * 2 );
            sprTransform.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
        _collider.enabled = true;
        lockMovement = false;
    }

    public void ResetPosition() {
        StartCoroutine( Fall() );
    }
}
