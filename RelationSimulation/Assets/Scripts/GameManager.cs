using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionVectors))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Npc[] _npcs;
    private PositionVectors _vectors;

    // Start is called before the first frame update
    void Start()
    {
        _vectors = GetComponent<PositionVectors>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _npcs[0].Move(_vectors.GetDestination(EDestination.Default), _vectors.GetLookAt(ELookAt.Default), () =>
            {
                Debug.Log("움직임 끝남");
            });
        }
    }
}
