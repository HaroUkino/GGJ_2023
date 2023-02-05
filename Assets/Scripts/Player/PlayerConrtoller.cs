using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConrtoller : MonoBehaviour {

    GameInput _input;

    PlayerMovement _target;

    public PlayerMovement TargetMovement => _target;

    private void OnEnable() => _input?.Enable();
    private void OnDisable() => _input?.Disable();

    private void Awake() {
        _target = GetComponent<PlayerMovement>();
        RegisterCallbacks();
        _input.Enable();
    }

    void RegisterCallbacks() {
        _input = new GameInput();
        _input.Movement.Movement.performed += ctx => Move( ctx.ReadValue<Vector2>() );
        _input.Movement.Movement.canceled += ctx => Move( new( 0, 0 ) );
        _input.Movement.Interaction.performed += ctx => Interact();
        _input.Movement.Reset.performed += ctx => Restart();
        _input.Movement.Menu.performed += ctx => Menu();
    }

    void Move( Vector2 dir ) => _target?.Move( dir );
    void Interact() => _target?.Interact();
    void Restart() => _target?.Kill();
    void Menu() { }
}
