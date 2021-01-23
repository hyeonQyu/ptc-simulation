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

    private Text _text;

    private void Start()
    {
        _text = GetComponentInChildren<Text>();
    }

    public void ShowScript(EScript script)
    {
        _text.text = _scripts[script];
    }
}
