using Nextwin.Client.Game;
using Nextwin.Client.Util;
using System.Collections;
using UnityEngine;

public enum EAnimState
{
    Idle,
    IsWalk,
    IsAttack
}

public class MidasianController : PlayerController
{
    public bool IsGrabbing
    {
        get
        {
            return _item != null;
        }
    }
    
    [SerializeField]
    private Transform _hand;
    private GameObject _item;

    [SerializeField]
    private Transform _director;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        OnInputAttack();
    }

    public void GrabItem(GameObject item)
    {
        item.transform.parent = _hand;
        item.transform.localPosition = new Vector3(-0.0539f, 0.0135f, 0.1544f);

        _item = item;
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

            if(GameManager.Instance.Flow == EFlow.GiveRiceBall || GameManager.Instance.Flow == EFlow.GiveCandy)
            {
                ReleaseItem();
            }
        }
    }

    /// <summary>
    /// 부장님에게 아이템 전달
    /// </summary>
    private void ReleaseItem()
    {
        StartCoroutine(MoveItem());
    }

    private IEnumerator MoveItem()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.0001f);

        while(true)
        {
            _item.transform.position = Vector3.Lerp(_item.transform.position, _director.position, Time.deltaTime);

            if(IsItemReached())
            {
                break;
            }

            yield return waitForSeconds;
        }

        _item = null;
    }

    private bool IsItemReached()
    {
        Vector3 diffVector = _item.transform.position - _director.position;
        if(Mathf.Abs(diffVector.x) + Mathf.Abs(diffVector.y) + Mathf.Abs(diffVector.z) < 0.01f)
        {
            return true;
        }
        return false;
    }
}