using Nextwin.Client.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogReport:UIDialog<EDialog>
{
    [SerializeField]
    private Button[] _buttons;
    [SerializeField]
    private GameObject[] _reports;

    private MidasianController _player;

    private int _selectedCount;

    private void Start()
    {
        _player = FindObjectOfType<MidasianController>();

        foreach(var button in _buttons)
        {
            button.onClick.AddListener(OnClickReport);
        }
    }

    private void OnClickReport()
    {
        _selectedCount++;

        if(_selectedCount == 3)
        {
            _selectedCount = 0;
            GameManager.Instance.CanGoNextFlow = true;
            _player.GrabItems(_reports);

            Show(false);
        }
    }
}
