using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable {

    [SerializeField] float moveSpeed;
    [SerializeField] float rayCastDist = 1;
    [SerializeField] bool directionOverride;

    public float MoveSpeed => moveSpeed;

    Vector2Int _dir;
    Vector3Int _target;
    bool _moving;

    void Awake() {
        transform.position = new Vector2( Mathf.RoundToInt( transform.position.x ), Mathf.RoundToInt( transform.position.y ) );
    }

    void Update() {
        if ( ValidateMovement() && !_moving ) {
            _moving = true;
            _target =  Vector3Int.RoundToInt( transform.position ) + new Vector3Int( _dir.x, _dir.y );
        }
        if( _moving ) {
            if ( Vector3.Distance( transform.position, _target ) > .01f )
                transform.position = Vector3.MoveTowards( transform.position, _target, Time.deltaTime * moveSpeed );
            else {
                transform.position = _target;
                _moving = false;
            }
        }
    }

    bool ValidateMovement() {
        var rayHit = Physics2D.Raycast( transform.position, _dir, rayCastDist);
        return !rayHit;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay( transform.position, new Vector2( _dir.x, _dir.y ) * rayCastDist );
    }

    public void Move( Vector2 dir ) {
        Vector2Int newDir = Vector2Int.RoundToInt( dir );
        if ( Mathf.Abs( newDir.x ) + Mathf.Abs( newDir.y ) > 1 ) {
            if ( directionOverride )
                newDir = new Vector2Int( Mathf.Abs( _dir.x ) == 1 ? 0 : newDir.x, Mathf.Abs( _dir.y ) == 1 ? 0 : newDir.y );
            else
                return;
        }
        _dir = newDir;
    }
}
