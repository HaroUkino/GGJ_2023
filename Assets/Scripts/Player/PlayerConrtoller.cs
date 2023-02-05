using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConrtoller : MonoBehaviour {

    GameInput _input;

    PlayerMovement target;

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Awake() {
        target = GetComponent<PlayerMovement>();
        RegisterCallbacks();
    }

    void RegisterCallbacks() {
        _input = new GameInput();
        _input.Movement.Movement.performed += ctx => Move( ctx.ReadValue<Vector2>() );
        _input.Movement.Movement.canceled += ctx => Move( new( 0, 0 ) );
        _input.Movement.Interaction.performed += ctx => Interact();
        _input.Movement.Reset.performed += ctx => Restart();
    }

    void Move( Vector2 dir ) {
        target?.Move( dir );
    }

    void Interact() {
        target?.Interact();
    }

    void Restart() => target.Kill();
}
