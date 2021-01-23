using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionVectors))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Npc[] _npcs;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            _npcs[0].Move(EDestination.Default, ELookAt.Default, () =>
            {
                UIManager.Instance.GetFrame(EFrame.Subtitle).Show();
                _npcs[0].Tell(EScript.GoodMorning, () =>
                {
                    _npcs[0].Tell(EScript.OffWork, () =>
                    {
                        UIManager.Instance.GetFrame(EFrame.Subtitle).Show(false);

                        _npcs[0].Move(EDestination.Out, ELookAt.Default);
                    });
                });
            });
        }
    }
}
