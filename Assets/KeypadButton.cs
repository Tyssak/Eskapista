using System;

public class KeypadButton : BaseButton
{
    public enum KeypadButtonType
    {
        Digit,
        Back,
        Clear
    }
    public KeypadButtonType buttonType;
    public char digit;
    public KeypadManager keypadManager;
    

    protected override void ButtonAction()
    {
        switch (buttonType)
        {
            case KeypadButtonType.Digit:
                keypadManager.AppendDigit(digit);
                break;
            case KeypadButtonType.Back:
                keypadManager.PopDigit();
                break;
            case KeypadButtonType.Clear:
                keypadManager.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
