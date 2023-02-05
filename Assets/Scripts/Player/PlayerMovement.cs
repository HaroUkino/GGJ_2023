using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable {

    [SerializeField] float moveSpeed;
    [SerializeField] float rayCastDist = 1;
    [SerializeField] bool directionOverride;
    [SerializeField] SpriteRenderer sprRenderer;

    public float MoveSpeed => _vehicle ? _vehicle.MoveSpeed : moveSpeed;

    bool _moving;
    Vector2 _spawnLocation;
    Vector2Int _dir;
    Vector3 _target;
    PlayerAnimation _anim;
    PlayerVehicle _vehicle;
    bool _lockMovement = false;
    Collider2D _collider;

    void Awake() {
        transform.position = Vector3Int.FloorToInt( transform.position );
        _anim = GetComponent<PlayerAnimation>();
        _spawnLocation = transform.position;
        _collider = GetComponent<Collider2D>();
    }

    void Update() {
        if ( _lockMovement ) return;
        if ( ValidateMovement() && !_moving ) {
            _moving = true;
            _target = transform.position + new Vector3( _dir.x, _dir.y );
        }
        if( _moving ) {
            if ( Vector3.Distance( transform.position, _target ) > .01f )
                transform.position = Vector3.MoveTowards( transform.position, _target, Time.deltaTime * MoveSpeed );
            else
                _moving = false;
        }
    }

    private void OnDrawGizmos() {
        if ( sprRenderer == null ) return;
        var center = sprRenderer.bounds.center;
        Gizmos.color = Color.green;
        Gizmos.DrawRay( center, new Vector2( _dir.x, _dir.y ) * rayCastDist );
    }

    bool ValidateMovement() {
        var center = sprRenderer.bounds.center;
        var rayHit = Physics2D.Raycast( center, _dir, rayCastDist);
        if ( rayHit && rayHit.transform.tag == "Vehicle" ) 
            return _vehicle == null;
        return !rayHit;
    }

    public void Move( Vector2 dir ) {
        Vector2Int newDir = new Vector2Int( Mathf.RoundToInt( dir.x ), Mathf.RoundToInt( dir.y ) );
        if ( Mathf.Abs( newDir.x ) + Mathf.Abs( newDir.y ) > 1 ) {
            if ( directionOverride )
                newDir = new Vector2Int( Mathf.Abs( _dir.x ) == 1 ? 0 : newDir.x, Mathf.Abs( _dir.y ) == 1 ? 0 : newDir.y );
            else
                return;
        }
        _dir = newDir;
        if ( _dir != Vector2.zero )
            PlayCurrentAnim();
    }

    string GetPlayerDirection() {
        string dir = PlayerAnimation.MOVE_DOWN;
        if ( _dir.x > 0 ) dir = PlayerAnimation.MOVE_RIGHT;
        if ( _dir.x < 0 ) dir = PlayerAnimation.MOVE_LEFT;
        if ( _dir.y > 0 ) dir = PlayerAnimation.MOVE_UP;
        return dir;
    }

    public void ReturnToSpawn() {
        transform.position = _spawnLocation;
    }

    public void SetVehicle( PlayerVehicle vehicle ) {
        if ( _vehicle != null ) return;
        _vehicle = vehicle;
        if ( vehicle ) {
            _vehicle.transform.SetParent( transform );
            vehicle.gameObject.SetActive( false );
        }
        PlayCurrentAnim();
    }

    public void Interact() {
        if ( _vehicle == null ) return;
        _vehicle.transform.SetParent( null );
        _vehicle.transform.position = Vector3Int.RoundToInt( transform.position );
        _vehicle.gameObject.SetActive( true );
        _vehicle.Dismount( transform );
        _vehicle = null;
        PlayCurrentAnim();
    }

    void PlayCurrentAnim() => _anim.PlayAnimation( $"{( _vehicle != null ? _vehicle.VehicleAnimTag + " " : "")}{GetPlayerDirection()}" );

    [ContextMenu( "Kill" )]
    public void Kill() {
        if ( _lockMovement ) return;
        Debug.Log( "player diededed" );
        StartCoroutine( Fall() );
        _moving = false;
    }

    IEnumerator Fall() {
        _lockMovement = true;
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
        ReturnToSpawn();
        while ( scale < 1 ) {
            scale = Mathf.Clamp01( scale + Time.deltaTime * 2 );
            sprTransform.localScale = Vector2.one * scale;
            yield return new WaitForEndOfFrame();
        }
        _collider.enabled = true;
        _lockMovement = false;
    }
}
