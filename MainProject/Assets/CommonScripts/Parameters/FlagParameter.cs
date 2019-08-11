using System;

public class FlagParameter : TestParameter
{
    public override ParameterType Type {
        get { return ParameterType.Flag; }
    }

    public bool Checked;
    public Action<bool> OnChanged;
}