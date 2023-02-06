using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : CharacterMovement
{
    Vector2 _startPosition;

        void Awake() { 
        _startPosition = transform.position;
        raycastLayers = new string[] { "Default", "Obstacle" };
    }

    public void PushNode ( GameObject other ) {
        var player = other.GetComponent<PlayerMovement>();
        if ( player != null) return;
        var dir = transform.position - other.transform.position;
        Move( dir );
    }
}
