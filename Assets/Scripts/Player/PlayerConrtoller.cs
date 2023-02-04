using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConrtoller : MonoBehaviour {

    GameInput _input;

    [SerializeField] PlayerMovement target;

    private void OnEnable() => _input.Enable();
    private void OnDisable() => _input.Disable();

    private void Awake() {
        RegisterCallbacks();
    }

    void RegisterCallbacks() {
        _input = new GameInput();
        _input.Movement.Movement.performed += ctx => Move( ctx.ReadValue<Vector2>() );
        _input.Movement.Movement.canceled += ctx => Move( new( 0, 0 ) );
    }

    void Move( Vector2 dir ) {
        target?.Move( dir );
    }

}
