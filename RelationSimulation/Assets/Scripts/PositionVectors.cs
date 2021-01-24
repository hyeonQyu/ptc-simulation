using Nextwin.Client.Util;
using System;
using UnityEngine;

public enum EDestination
{
    Entrance,
    Exit,
    SeatMember,
    SeatDirector,
    SeatLeader1,
    SeatLeader2,
    Player,
    Director,
    LobbyLeader1,
    LobbyLeader2,
}

public enum ELookAt
{
    None,
    Player,
    ComputerMember,
    ComputerDirector,
    SeatLeader1,
    SeatLeader2,
    LobbyLeader1,
    Director,
    Entrance
}

[Serializable]
public class DestinationDictionary : SerializableDictionary<EDestination, Transform> { }

[Serializable]
public class LookAtDictionary : SerializableDictionary<ELookAt, Transform> { }

public class PositionVectors : MonoBehaviour
{
    public static PositionVectors Instance { get; private set; }

    [SerializeField]
    private DestinationDictionary _destinations;

    [SerializeField]
    private LookAtDictionary _lookAts;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetDestination(EDestination dest)
    {
        return _destinations[dest].position;
    }

    public Vector3 GetLookAt(ELookAt lookAt)
    {
        return _lookAts[lookAt].position;
    }
}
