using Nextwin.Client.UI;

public enum EFrame
{
    Subtitle,
    Guage,
    Shade
}

public enum EDialog
{
    Selection,
    Report
}

public class UIManager : UIManagerBase<UIManager, EFrame, EDialog>
{
}