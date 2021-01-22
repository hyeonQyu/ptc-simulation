using Nextwin.Client.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    Idle,
    IsWalk
}

public class MidasianController : PlayerController
{
    protected override void OnMoveStart()
    {
        _animator.SetBool(AnimState.IsWalk.ToString(), true);
        Debug.Log("Move Start");
    }

    protected override void OnMoveFinish()
    {
        _animator.SetBool(AnimState.IsWalk.ToString(), false);
        Debug.Log("Move Finish");
    }
}