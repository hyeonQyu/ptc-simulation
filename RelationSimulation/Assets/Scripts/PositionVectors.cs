using Nextwin.Client.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDestination
{
    Default,
    Out
}

public enum ELookAt
{
    Default
}

[Serializable]
public class DestinationDictionary : SerializableDictionary<EDestination, Transform> { }

[Serializable]
public class LookAtDictionary : SerializableDictionary<ELookAt, Transform> { }

public class PositionVectors : MonoBehaviour
{
    [SerializeField]
    private DestinationDictionary _destinations;

    [SerializeField]
    private LookAtDictionary _lookAts;

    public Vector3 GetDestination(EDestination dest)
    {
        return _destinations[dest].position;
    }

    public Vector3 GetLookAt(ELookAt lookAt)
    {
        return _lookAts[lookAt].position;
    }
}
