using System.Collections;
using UnityEngine;

public class DirectorNpc : Npc
{
    public GameObject Item { get; set; }
    public GameObject[] Items { get; set; }

    [SerializeField]
    private Transform _throwingPosition;

    protected override void Start()
    {
        base.Start();
        Items = new GameObject[3];
    }

    public void ThrowItem()
    {
        StartCoroutine(MoveItem(Item));
        AudioManager.Instance.PlayAudio(EAudioClip.Fist, EAudioSource.Paper1);
    }

    public void ThrowItems()
    {
        for(int i = 0; i < Items.Length; i++)
        {
            StartCoroutine(MoveItem(Items[i], true));
            MakeSound(i);
        }
    }

    private void MakeSound(int index)
    {
        switch(index)
        {
            case 0:
                AudioManager.Instance.PlayAudio(EAudioClip.Paper, EAudioSource.Paper1);
                break;

            case 1:
                AudioManager.Instance.PlayAudio(EAudioClip.Paper, EAudioSource.Paper2);
                break;

            case 2:
                AudioManager.Instance.PlayAudio(EAudioClip.Paper, EAudioSource.Paper3);
                break;
        }
    }

    private IEnumerator MoveItem(GameObject item, bool isActive = false)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.0001f);

        while(true)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, _throwingPosition.transform.position, Time.deltaTime * 2f);

            if(IsItemReached(item))
            {
                break;
            }

            yield return waitForSeconds;
        }

        item.SetActive(isActive);
    }

    private bool IsItemReached(GameObject item)
    {
        Vector3 diffVector = item.transform.position - _throwingPosition.transform.position;
        if(Mathf.Abs(diffVector.x) + Mathf.Abs(diffVector.y) + Mathf.Abs(diffVector.z) < 1f)
        {
            return true;
        }
        return false;
    }
}
