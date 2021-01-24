using Nextwin.Client.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogSelection : UIDialog<EDialog>
{
    private Button _buttonCandy;
    private Button _buttonRiceBall;

    [SerializeField]
    private MidasianController _player;

    [SerializeField]
    private GameObject _candy;
    [SerializeField]
    private GameObject _riceBall;

    // Start is called before the first frame update
    void Start()
    {
        _buttonCandy = transform.GetChild(0).GetComponent<Button>();
        _buttonRiceBall = transform.GetChild(1).GetComponent<Button>();

        _buttonCandy.onClick.AddListener(OnClickCandy);
        _buttonRiceBall.onClick.AddListener(OnClickRiceBall);
    }

    private void OnClickCandy()
    {
        GameManager.Instance.CanGoNextFlow = true;
        _player.GrabItem(_candy);
        
        Show(false);
    }

    private void OnClickRiceBall()
    {
        GameManager.Instance.CanGoNextFlow = true;
        _player.GrabItem(_riceBall);

        Show(false);
    }
}
