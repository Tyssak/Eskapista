public class FiltersRiddleButton : BaseButton
{
    public enum FiltersRiddleButtonType
    {
        OnOff,
        Reset,
        Next,
        Prev,
        Reverse,
        Plus,
        Minus
    }
    
    public changePicture pictureScript;
    public FiltersRiddleButtonType buttonType;

    protected override void ButtonAction()
    {
        if (pictureScript != null)
        {
            pictureScript.HandleButtonRelease(buttonType);
        }
    }
}
