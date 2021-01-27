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
    public bool IsGrabbing { get; private set; }
    
    [SerializeField]
    private Transform _hand;
    private GameObject _item;
    private GameObject[] _items;

    [SerializeField]
    private Transform _directorTransform;
    private DirectorNpc _director;

    protected override void Start()
    {
        base.Start();
        _items = new GameObject[3];
        _director = _directorTransform.GetComponent<DirectorNpc>();
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
        IsGrabbing = true;
    }

    public void GrabItems(GameObject[] items)
    {
        for(int i = 0; i < items.Length; i++)
        {
            items[i].transform.parent = _hand;
            items[i].transform.localPosition = new Vector3(-0.0539f, 0.0135f, 0.1544f);

            _items[i] = items[i];
        }
        IsGrabbing = true;
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
        if(Input.GetKeyDown(KeyCode.F))
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
            else if(GameManager.Instance.Flow == EFlow.GiveReport1 || GameManager.Instance.Flow == EFlow.GiveReport2)
            {
                ReleaseItems();
            }
        }
    }

    /// <summary>
    /// 부장님에게 아이템 전달
    /// </summary>
    private void ReleaseItem()
    {
        StartCoroutine(MoveItem(_item));
    }

    private void ReleaseItems()
    {
        for(int i = 0; i < _items.Length; i++)
        {
            StartCoroutine(MoveItem(_items[i], i));
        }
    }

    private IEnumerator MoveItem(GameObject item, int index = -1)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.0001f);

        int i = 0;
        while(true)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, _directorTransform.position, Time.deltaTime);

            i++;
            if(IsItemReached(item))
            {
                break;
            }
            else if(i > 140)
            {
                break;
            }

            yield return waitForSeconds;
        }

        if(index == -1)
        {
            _director.Item = item;
        }
        else
        {
            _director.Items[index] = item;
        }
        IsGrabbing = false;
    }

    private bool IsItemReached(GameObject item)
    {
        Vector3 diffVector = item.transform.position - _directorTransform.position;
        float diff = Mathf.Abs(diffVector.x) + Mathf.Abs(diffVector.y) + Mathf.Abs(diffVector.z);

        if(diff < 0.8f)
        {
            return true;
        }
        return false;
    }
}