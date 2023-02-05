using UnityEngine;

public class VineController : MonoBehaviour {

    [SerializeField] GameObject vinePrefab;
    [SerializeField] byte width, height;
    [SerializeField] Vector2 origin;

    [ContextMenu("Create tiles")]
    void CreateTiles() {
        ClearChildren();
        int tileNumber = width * height;
        Vector2 tileOrigin = new( width * origin.x, height * origin.y );
        for ( int i = 0; i < tileNumber; i++ ) {
            var go = Instantiate( vinePrefab, transform );
            go.transform.localPosition = new Vector2( i%width - ( width/2 ), Mathf.FloorToInt( (float)i/width ) - ( height / 2 ) ) + tileOrigin;
        }
    }

    [ContextMenu("Clear child")]
    void ClearChildren() {
        for ( int i = transform.childCount - 1; i >= 0; i-- )
#if UNITY_EDITOR
            DestroyImmediate( transform.GetChild( i ).gameObject );
#else
            Destroy( transform.GetChild( i ).gameObject );
#endif
    }
}
