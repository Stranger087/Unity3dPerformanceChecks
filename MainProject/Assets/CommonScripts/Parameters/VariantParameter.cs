using System;
using System.Collections.Generic;
using System.Linq;

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

    public static List<object> ParseEnum<T>() {
        var arr = (T[]) Enum.GetValues(typeof(T));
        return arr.Select(_ => (object) _).ToList();
    }
}