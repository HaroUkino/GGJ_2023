using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConrtoller : MonoBehaviour {

    GameInput _input;

    IMovable target;

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Awake() {
        target = GetComponent<IMovable>();
        RegisterCallbacks();
    }

    void RegisterCallbacks() {
        _input = new GameInput();
        _input.Movement.Movement.performed += ctx => Move( ctx.ReadValue<Vector2>() );
        _input.Movement.Movement.canceled += ctx => Move( new( 0, 0 ) );
        _input.Movement.Interaction.performed += ctx => Interact();
    }

    void Move( Vector2 dir ) {
        target?.Move( dir );
    }

    void Interact() {
        target?.Interact();
    }

}
