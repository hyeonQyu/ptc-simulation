using Nextwin.Client.Game;
using Nextwin.Client.Util;
using UnityEngine;

public enum EAnimState
{
    Idle,
    IsWalk,
    IsAttack
}

public class MidasianController : PlayerController
{
    protected override void Update()
    {
        base.Update();
        OnInputAttack();
    }

    protected override void OnMoveStart()
    {
        _animator.SetBool(EAnimState.IsWalk.ToString(), true);
    }

    protected override void OnMoveFinish()
    {
        _animator.SetBool(EAnimState.IsWalk.ToString(), false);
    }

    private void OnInputAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetBool(EAnimState.IsAttack.ToString(), true);
            ActionManager.Instance.ExecuteWithDelay(() =>
            {
                _animator.SetBool(EAnimState.IsAttack.ToString(), false);
            }, 0.5f);
        }
    }
}