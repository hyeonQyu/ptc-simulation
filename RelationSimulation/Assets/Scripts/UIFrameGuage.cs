using Nextwin.Client.UI;
using UnityEngine;

public class UIFrameGuage : UIFrame<EFrame>
{
    [SerializeField]
    private RectTransform _guage;

    private bool _isRising;

    private float _width = 16f;
    private float _height = 0f;

    private void Start()
    {
        _guage.sizeDelta = new Vector2(_width, _height);
    }

    private void Update()
    {
        if(!_isRising)
        {
            return;
        }

        _height += 0.013f;
        _guage.sizeDelta = new Vector2(_width, _height);
    }

    public override void Show(bool isActive = true)
    {
        base.Show(isActive);

        _isRising = isActive;
    }

    public void RaiseGuage()
    {
        _height += 14f;
    }
}
