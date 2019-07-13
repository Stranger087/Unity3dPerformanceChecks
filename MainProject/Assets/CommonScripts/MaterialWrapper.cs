using System;
using UnityEngine;

[Serializable]
public class MaterialWrapper
{
    public Material Material;
    private Texture _Texture;

    public Texture Texture {
        get { return _Texture; }
        private set {
            _Texture = value;
            Material.SetTexture(0, value);
        }
    }
}