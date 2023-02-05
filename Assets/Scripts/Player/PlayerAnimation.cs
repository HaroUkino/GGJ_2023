using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    string _player => "player ";

    public static string MOVE_UP => "back";
    public static string MOVE_DOWN => "front";
    public static string MOVE_LEFT => "left";
    public static string MOVE_RIGHT => "right";

    [SerializeField] Animator animator;

    public void PlayAnimation( string anim ) {
        animator.Play( $"{_player}{anim}", 0 );
    }

}
