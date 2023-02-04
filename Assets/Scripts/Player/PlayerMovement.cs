using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable {

    [SerializeField] float moveSpeed;
    [SerializeField] float rayCastDist = 1;
    [SerializeField] bool directionOverride;
    private float positionX;
    private float positionY; 

    public float MoveSpeed => moveSpeed;

    Vector2Int _dir;
    SpriteRenderer _sprRenderer;
    Vector3 _target;
    bool _moving;

    void Awake() {
        positionX = 2.5f;
        positionY = 2.5f;
        transform.position = new Vector2(positionX, positionY);
        _sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if ( ValidateMovement() && !_moving ) {
            _moving = true;
            _target = transform.position + new Vector3( _dir.x, _dir.y );
        }
        if( _moving ) {
            if ( Vector3.Distance( transform.position, _target ) > .01f )
                transform.position = Vector3.MoveTowards( transform.position, _target, Time.deltaTime * moveSpeed );
            else
                _moving = false;
        }
    }

    bool ValidateMovement() {
        var rayHit = Physics2D.Raycast( transform.position, _dir, rayCastDist);
        return !rayHit;
    }

    private void OnDrawGizmos() {
        if ( _sprRenderer == null ) return;
        var extend = _sprRenderer.bounds.extents;
        Gizmos.color = Color.green;
        Gizmos.DrawRay( transform.position, new Vector2( _dir.x, _dir.y ) * rayCastDist );
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
}
