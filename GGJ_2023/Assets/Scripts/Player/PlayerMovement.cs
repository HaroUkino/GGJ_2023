using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IMovable {

    [SerializeField] float moveSpeed;
    [SerializeField] float rayCastDist = .25f;

    public float MoveSpeed => moveSpeed;

    Vector2 _dir;
    SpriteRenderer _sprRenderer;

    void Awake() {
        _sprRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if( ValidateMovement() )
            transform.position += new Vector3( _dir.x, _dir.y, 0 ) * Time.deltaTime * moveSpeed;
    }

    bool ValidateMovement() {
        var extend = _sprRenderer.bounds.extents;
        var rayHit_topLeft = Physics2D.Raycast( new( transform.position.x - extend.x, transform.position.y + extend.y ), _dir, rayCastDist);
        var rayHit_topRight = Physics2D.Raycast( new( transform.position.x + extend.x, transform.position.y + extend.y ), _dir, rayCastDist );
        var rayHit_botLeft = Physics2D.Raycast( new( transform.position.x - extend.x, transform.position.y - extend.y ), _dir, rayCastDist );
        var rayHit_botRight = Physics2D.Raycast( new( transform.position.x + extend.x, transform.position.y - extend.y ), _dir, rayCastDist );
        return !(rayHit_botLeft || rayHit_botRight || rayHit_topRight || rayHit_topLeft);
    }

    private void OnDrawGizmos() {
        if ( _sprRenderer == null ) return;
        var extend = _sprRenderer.bounds.extents;
        Gizmos.color = Color.green;
        Gizmos.DrawRay( new Vector3( transform.position.x - extend.x, transform.position.y + extend.y ), _dir * rayCastDist );
        Gizmos.DrawRay( new Vector3( transform.position.x + extend.x, transform.position.y + extend.y ), _dir * rayCastDist );
        Gizmos.DrawRay( new Vector3( transform.position.x - extend.x, transform.position.y - extend.y ), _dir * rayCastDist );
        Gizmos.DrawRay( new Vector3( transform.position.x + extend.x, transform.position.y - extend.y ), _dir * rayCastDist );
    }

    public void Move( Vector2 dir ) {
        _dir = dir;
    }
}
