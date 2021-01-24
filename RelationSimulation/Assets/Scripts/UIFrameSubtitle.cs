using Nextwin.Client.UI;
using Nextwin.Client.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SubtitleDictionary : SerializableDictionary<EScript, string> { }

public class UIFrameSubtitle : UIFrame<EFrame>
{
    [SerializeField]
    private SubtitleDictionary _scripts;

    [SerializeField]
    private Text _teller;
    [SerializeField]
    private Text _script;

    public override void Show(bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }

    public void ShowScript(EScript script, ENpcType npcType)
    {
        _teller.text = GetTeller(npcType);
        _script.text = _scripts[script];
    }

    private string GetTeller(ENpcType npcType)
    {
        string teller = "";

        switch(npcType)
        {
            case ENpcType.Director:
                teller = "박부장님";
                break;

            case ENpcType.GirlFriend:
                teller = "여자친구";
                break;

            case ENpcType.Leader1:
                teller = "오과장님";
                break;

            case ENpcType.Leader2:
                teller = "김과장님";
                break;

            case ENpcType.Member:
                teller = "윤대리님";
                break;

            case ENpcType.Player:
                teller = "나";
                break;
        }

        return teller;
    }
}
