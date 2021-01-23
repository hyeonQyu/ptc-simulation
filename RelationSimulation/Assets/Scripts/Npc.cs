﻿using Nextwin.Client.Util;
using Nextwin.Util;
using System.Collections;
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
    GoodMorning,
    OffWork
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(PositionVectors))]
public class Npc : MonoBehaviour
{
    public delegate void Callback();

    [SerializeField]
    private ENpcType _npcType;
    private NavMeshAgent _agent;
    private Animator _animator;
    private PositionVectors _vectors;

    [SerializeField]
    private float _rotateSpeed = 15f;

    private UIFrameSubtitle _uiSubtitle;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _vectors = GetComponent<PositionVectors>();
    }

    /// <summary>
    /// Npc의 대사를 재생
    /// </summary>
    /// <param name="script">재생할 대사</param>
    /// <param name="callback">Npc의 대사가 끝난 후 실행할 내용</param>
    public void Tell(EScript script, Callback callback = null)
    {
        EAudioClip clip = EnumConverter.ToEnum<EAudioClip>(script.ToString());
        EAudioSource source = EnumConverter.ToEnum<EAudioSource>(_npcType.ToString());

        AudioManager.Instance.PlayAudio(clip, source);

        if(_uiSubtitle == null)
        {
            _uiSubtitle = UIManager.Instance.GetFrame(EFrame.Subtitle) as UIFrameSubtitle;
        }
        _uiSubtitle.ShowScript(script);

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
    public void Move(EDestination dest, ELookAt lookAt, Callback callback = null)
    {
        _agent.SetDestination(_vectors.GetDestination(dest));
        _animator.SetBool(EAnimState.IsWalk.ToString(), true);
        StartCoroutine(WaitForMoveFinish(_vectors.GetLookAt(lookAt), callback));
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
            Quaternion lookRotation = Quaternion.LookRotation(lookAt - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _rotateSpeed);

            yield return waitForEndOfFrame;

            if(IsSameRotation(lookRotation))
            {
                break;
            }
        }

        callback?.Invoke();
    }

    private bool IsSameRotation(Quaternion destRotation)
    {
        float diff = Mathf.Abs(destRotation.eulerAngles.y - transform.rotation.eulerAngles.y);

        if(diff < 0.001f)
        {
            return true;
        }
        return false;
    }
}
