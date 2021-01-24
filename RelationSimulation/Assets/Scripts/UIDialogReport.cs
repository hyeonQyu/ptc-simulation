using Nextwin.Client.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogReport:UIDialog<EDialog>
{
    [SerializeField]
    private Button[] _buttons;

    private int _selectedCount;

    private void Start()
    {
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
            Show(false);
        }
    }
}
