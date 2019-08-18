using System;
using System.Collections.Generic;

public class VariantParameter : TestParameter
{
    public override ParameterType Type {
        get { return ParameterType.SelectVariant; }
    }


    public List<object> Variants;
    public int CurrentIndex;

    public Action<object> OnChanged;


    public override void ExecuteChangedCallback() {
        OnChanged(Variants[CurrentIndex]);
    }

}