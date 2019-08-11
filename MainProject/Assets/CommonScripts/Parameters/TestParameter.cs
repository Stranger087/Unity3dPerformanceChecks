using System;

public enum ParameterType
{
    SelectVariant,
    Slider,
    Flag
}

public abstract class TestParameter
{
    public string Name;
    public abstract ParameterType Type { get;}
}