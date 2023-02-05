using System.Collections;
using UnityEngine;

public enum VehicleAction { None, Push }

public class PlayerVehicle : MonoBehaviour, IResetable {

    [SerializeField] string vehicleAnimTag;
    [SerializeField] float moveSpd;
    [SerializeField] VehicleAction vehicleAction;
    [SerializeField] Transform graphics; 
    [SerializeField] ResetAnimation resetAnimation;

    public string VehicleAnimTag => vehicleAnimTag;
    public float MoveSpeed => moveSpd;
    public VehicleAction VehicleAction => vehicleAction;

    Vector2 _startPos;
    Transform _player;
    Collider2D _collder;
    Collider2D[] _childColliders;

    private void Awake() {
        _startPos = transform.position;
        _collder = GetComponent<Collider2D>();
        _childColliders = GetComponentsInChildren<Collider2D>();
    } 

    private void OnTriggerEnter2D( Collider2D collision ) {
        var player = collision.GetComponent<PlayerMovement>();
        player?.SetVehicle( this );
    }

    public void Dismount( Transform player ) {
        _collder.enabled = false;
        EnableChildColliders( false );
        _player = player;
        StartCoroutine( KeepPlayerDistance() );
    }

    void EnableChildColliders( bool enable ) {
        if ( _childColliders != null )
            foreach ( var childCol in _childColliders )
                childCol.enabled = enable;
    }

    public void ResetPosition() => resetAnimation.BeginReset( graphics, () => transform.position = _startPos );

    IEnumerator KeepPlayerDistance() {
        yield return new WaitUntil( () => Vector3.Distance( transform.position, _player.position ) > .5f );
        _player = null;
        _collder.enabled = true;
        EnableChildColliders( true );
    }
}
