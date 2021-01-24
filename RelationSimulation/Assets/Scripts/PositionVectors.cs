using Nextwin.Client.Util;
using System;
using UnityEngine;

public enum EDestination
{
    Entrance,
    Exit,
    SeatMember,
    SeatDirector,
    Player,
    Director
}

public enum ELookAt
{
    None,
    Player,
    ComputerMember,
    ComputerDirector,
    Director
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
