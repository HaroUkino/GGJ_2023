using UnityEngine;

public class BoulderController : CharacterMovement, IInteractable, IResetable {

    [SerializeField] ResetAnimation resetAnimation;

    public string InteractText => "Push?";

    Vector2 _startPosition;

    void Awake() { 
        _startPosition = transform.position;
        raycastLayers = new string[] { "Default", "Obstacle" };
    }

    public void Interact( GameObject other ) {
        var player = other.GetComponent<PlayerMovement>();
        if ( player == null || player.Vehicle == null || player.Vehicle.VehicleAction != VehicleAction.Push ) return;
        var dir = transform.position - other.transform.position;
        Move( dir );
    }

    public void ResetPosition() {
        StopMovement();
        resetAnimation.BeginReset( sprRenderer.transform, () => transform.position = _startPosition ); 
    }
}
