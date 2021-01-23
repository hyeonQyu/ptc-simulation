using Nextwin.Client.Game;
using UnityEngine;

public enum EAnimState
{
    Idle,
    IsWalk
}

public class MidasianController : PlayerController
{
    protected override void OnMoveStart()
    {
        _animator.SetBool(EAnimState.IsWalk.ToString(), true);
    }

    protected override void OnMoveFinish()
    {
        _animator.SetBool(EAnimState.IsWalk.ToString(), false);
    }
}