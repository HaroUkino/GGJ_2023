using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable {

    float MoveSpeed { get; }

    void Move( Vector2 dir );
    void Kill();
    void Interact();
}
