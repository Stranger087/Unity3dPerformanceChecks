using System;
using System.Collections.Generic;

public class VariantParameter<T> : TestParameter
{
    public override ParameterType Type {
        get { return ParameterType.SelectVariant; }
    }

    public List<T> Variants;
    public int CurrentIndex;

    public Action<T> OnChanged;
}