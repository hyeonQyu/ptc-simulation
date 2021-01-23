using Nextwin.Client.Util;
using Nextwin.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ENpcType
{
    MaleA,
    MaleB,
    Female,
    Zombie,
    Police
}

/// <summary>
/// 대본
/// </summary>
public enum EScript
{

}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
public class Npc : MonoBehaviour
{
    public delegate void Callback();

    [SerializeField]
    private ENpcType _npcType;
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField]
    private float _rotateSpeed = 15f;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Npc의 대사를 재생
    /// </summary>
    /// <param name="script">재생할 대사</param>
    /// <param name="callback">Npc의 대사가 끝난 후 실행할 내용</param>
    public void Tell(EScript script, Callback callback)
    {
        EAudioClip clip = EnumConverter.ToEnum<EAudioClip>(script.ToString());
        EAudioSource source = EnumConverter.ToEnum<EAudioSource>(_npcType.ToString());

        AudioManager.Instance.PlayAudio(clip, source);

        float playTime = AudioManager.Instance.GetAudioClipLength(clip) / AudioManager.Instance.GetAudioSourcePitch(source);
        ActionManager.Instance.ExecuteWithDelay(() =>
        {
            callback?.Invoke();
        }, playTime);
    }

    /// <summary>
    /// dest로 움직인 후 lookAt을 바라보게 함
    /// </summary>
    /// <param name="dest">움직일 목적지</param>
    /// <param name="lookAt">움직인 후 바라볼 곳</param>
    /// <param name="callback">움직임과 회전이 끝난 후 실행할 내용</param>
    public void Move(Vector3 dest, Vector3 lookAt, Callback callback)
    {
        _agent.SetDestination(dest);
        _animator.SetBool(EAnimState.IsWalk.ToString(), true);
        StartCoroutine(WaitForMoveFinish(lookAt, callback));
    }

    private IEnumerator WaitForMoveFinish(Vector3 lookAt, Callback callback)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.0001f);

        while(_agent.pathPending)
        {
            yield return waitForSeconds;
        }
        while(_agent.remainingDistance > _agent.stoppingDistance)
        {
            yield return waitForSeconds;
        }
        while(_agent.hasPath && _agent.velocity.sqrMagnitude != 0)
        {
            yield return waitForSeconds;
        }

        _animator.SetBool(EAnimState.IsWalk.ToString(), false);
        StartCoroutine(WaitForRotateFinish(lookAt, callback));
    }

    private IEnumerator WaitForRotateFinish(Vector3 lookAt, Callback callback)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while(true)
        {
            float angle = Mathf.Atan2(lookAt.x, lookAt.z) * Mathf.Rad2Deg;
            Quaternion destRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, destRotation, _rotateSpeed * Time.fixedDeltaTime);
            yield return waitForEndOfFrame;

            if(IsSameRotation(destRotation))
            {
                break;
            }
        }

        callback?.Invoke();
    }

    private bool IsSameRotation(Quaternion destRotation)
    {
        Vector3 diff = destRotation.eulerAngles - transform.rotation.eulerAngles;

        float x = Mathf.Abs(diff.x);
        float y = Mathf.Abs(diff.y);
        float z = Mathf.Abs(diff.z);

        if(x + y + z < 0.1f)
        {
            return true;
        }
        return false;
    }
}
