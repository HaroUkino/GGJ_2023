using UnityEngine;

public interface IMovable {

    float MoveSpeed { get; }

    void Move( Vector2 dir );
}
