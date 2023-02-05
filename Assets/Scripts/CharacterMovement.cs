using UnityEngine;

public class CharacterMovement : MonoBehaviour, IMovable {

    public PlayerVehicle Vehicle { get; protected set; }

    [SerializeField] float moveSpeed;
    [SerializeField] float rayCastDist = 1, rayCastCenterOffset;
    [SerializeField] bool directionOverride;
    [SerializeField] protected SpriteRenderer sprRenderer;

    public float MoveSpeed => Vehicle != null ? Vehicle.MoveSpeed : moveSpeed ;

    protected bool lockMovement = false;
    protected event System.Action onDirectionChanged;
    protected Vector2Int direction => _dir;
    protected string[] raycastLayers = { "Default" };

    bool _moving;
    Vector2Int _dir;
    Vector3 _target;

    void Update() {
        if ( lockMovement ) return;
        if ( !_moving && ValidateMovement() ) {
            _moving = true;
            _target = transform.position + new Vector3( _dir.x, _dir.y );
            onDirectionChanged?.Invoke();
        }
        if ( _moving ) {
            if ( Vector3.Distance( transform.position, _target ) > .01f )
                transform.position = Vector3.MoveTowards( transform.position, _target, Time.deltaTime * MoveSpeed );
            else
                _moving = false;
        }
    }

    private void OnDrawGizmos() {
        if ( sprRenderer == null ) return;
        var center = sprRenderer.bounds.center + ( new Vector3( _dir.x, _dir.y ) * rayCastCenterOffset );
        Gizmos.color = Color.green;
        Gizmos.DrawRay( center, new Vector2( _dir.x, _dir.y ) * ( rayCastDist - rayCastCenterOffset ) );
    }

    bool ValidateMovement() {
        var center = sprRenderer.bounds.center + ( new Vector3( _dir.x, _dir.y ) * rayCastCenterOffset );
        var rayHit = Physics2D.Raycast( center, _dir, rayCastDist - rayCastCenterOffset, LayerMask.GetMask( raycastLayers ) );
        if ( rayHit ) {
            var interactable = rayHit.transform.GetComponent<IInteractable>();
            if ( interactable != null ) interactable.Interact( gameObject );
            if ( rayHit.transform.CompareTag( "Vehicle" ) ) return Vehicle == null;
        }
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
    }

    protected void StopMovement() {
        _dir = Vector2Int.zero;
        _moving = false; 
    }
}
