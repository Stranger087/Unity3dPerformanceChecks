using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.Parameters;
using UnityEngine;


public class DrawCallTestManager : BaseTestManager
{
    [SerializeField] private GameObject Template1;
    [SerializeField] private GameObject Template2;

    [SerializeField] private List<RenderTexture> _RenderTextures;

    private enum DrawModes
    {
        DrawDifferent,
        DrawIdentical,
        DrawInSingleMesh
    }

    private enum TestSubjects
    {
        Texture,
        Shader,
        Mesh,
        ShaderParameters,
        RenderTarget
    }

    private Dictionary<TestSubjects, int> _VariantsCountMax;

    private DrawModes _DrawMode;

    private Mesh _OneDrawCallMesh;

    private List<Material> _SampleMaterials = new List<Material>();
    private List<Material> _Materials = new List<Material>();
    private bool _UseSameMaterial;

    private int _TextureSetIndex;
    private string _TextureSizeKey;
    private string _ShaderKey;
    private bool _ZcullingOn;
    private int _VariantsCount;


    private DrawModes _PrevDrawMode;
    private int _PrevTextureSetIndex;
    private string _PrevTextureSizeKey;
    private string _PrevShaderKey;
    private bool _PrevZcullingOn;
    private TestSubjects _TestSubject;

    private NumberParameter _VariantsCountParameter;
    private Camera _Camera;
    private string _MeshKey;

    private List<Mesh> _Meshes = new List<Mesh>();

    // Start is called before the first frame update
    protected override void OnEnable() {
        _VariantsCountMax = new Dictionary<TestSubjects, int>() {
            {TestSubjects.Mesh, 5},
            {TestSubjects.Shader, 10},
            {TestSubjects.Texture, 10},
            {TestSubjects.ShaderParameters, 100},
            {TestSubjects.RenderTarget, 5}
        };


        for (int i = 0; i < _VariantsCountMax[TestSubjects.Mesh]; i++) {
            _Meshes.Add(null);
        }

        base.OnEnable();

        Template1.SetActive(false);
        Template2.SetActive(false);

        _Camera = Camera.main;


//        ChooseVariantWidget.Instance.RegisterVariation(OnTextSelected, "Texture Variants", _TextureVariants.Select(_ => _.Name).ToArray());
//        ChooseVariantWidget.Instance.RegisterVariation(OnShader1Selected, "Shader1", _AvailableShaders);
//        ChooseVariantWidget.Instance.RegisterVariation(OnShader2Selected, "Shader2", _AvailableShaders);
    }

    protected override void SetupParameters() {
        Parameters.Add(new NumberParameter() {
            Name = "DrawCalls Count",
            Min = 1,
            Max = 5000,
            Value = 50,
            Precision = 0.1f,
            OnChanged = Handler_DrawcallsCountChanged
        });

        _VariantsCountParameter = new NumberParameter() {
            Name = "Variants Count",
            Min = 1,
            Max = 10,
            Value = 1,
            Precision = 1f,
            OnChanged = Handler_VariantsCountChanged
        };
        Parameters.Add(_VariantsCountParameter);

        Parameters.Add(new VariantParameter() {
            Name = "Draw Mode",
            Variants = ((DrawModes[]) Enum.GetValues(typeof(DrawModes))).Select(enumValue => (object) enumValue).ToList(),
            OnChanged = Handler_DrawModeChanged
        });

        Parameters.Add(new VariantParameter() {
            Name = "TestSubject",
            Variants = VariantParameter.ParseEnum<TestSubjects>(),
            OnChanged = Handler_TestSubjectChanged
        });

        Parameters.Add(new FlagParameter() {
            Name = "Z Culling",
            Checked = true,
            OnChanged = Handler_ZCullingChanged
        });

        Parameters.Add(new VariantParameter() {
            Name = "Shader",
            Variants = new List<object>() {
                "Unlit",
                "Standard"
            },
            OnChanged = Handler_ShaderChanged
        });

        Parameters.Add(new VariantParameter() {
            Name = "Texture Size",
            Variants = new List<object>() {
                "256",
                "2048",
                "4096"
            },
            CurrentIndex = 1,
            OnChanged = Handler_TextureSizeChanged
        });

        Parameters.Add(new VariantParameter() {
            Name = "Mesh",
            Variants = new List<object>() {
                "quad_2",
                "quad_512",
                "quad_2048",
                "quad_32768"
            },
            CurrentIndex = 0,
            OnChanged = Handler_MeshSizeChanged
        });

        foreach (var testParameter in Parameters) {
            testParameter.ExecuteChangedCallback();
        }

        Initialized = true;
        UpdateMaterials();
    }

    private void Handler_MeshSizeChanged(object obj) {
        _MeshKey = (string) obj;
        UpdateMaterials();
    }

    private void Handler_TestSubjectChanged(object obj) {
        _TestSubject = (TestSubjects) obj;
        _VariantsCountParameter.Max = _VariantsCountMax[_TestSubject];
        if (_VariantsCountParameter.OnSettingsChange != null)
            _VariantsCountParameter.OnSettingsChange.Invoke();
        UpdateMaterials();
    }

    private void Handler_VariantsCountChanged(float val) {
        _VariantsCount = (int) val;
        UpdateMaterials();
    }

    private void Handler_DrawModeChanged(object drawMode) {
        _DrawMode = (DrawModes) drawMode;
        UpdateMaterials();
    }

    private void Handler_TextureSizeChanged(object size) {
        _TextureSizeKey = (string) size;
        UpdateMaterials();
    }

    private void Handler_ShaderChanged(object shader) {
        _ShaderKey = (string) shader;
        UpdateMaterials();
    }

    private void Handler_ZCullingChanged(bool zCulling) {
        _ZcullingOn = zCulling;
        UpdateMaterials();
    }

    private void Handler_DrawcallsCountChanged(float val) {
        DrawsCount = (int) val;
        UpdateMaterials();
    }


    private void UpdateMaterials() {
        //setup material samples

        if (!Initialized)
            return;

        var baseMeshPath = "Meshes/" + _MeshKey;

        _Meshes[0] = Resources.Load<Mesh>(baseMeshPath + "_0");

        string baseShaderPath = _ShaderKey + "/" + (_ZcullingOn ? "" : "_ZWriteOff");

        var defaultShader = Shader.Find(baseShaderPath + "_0");
        var defaultTexture = Resources.Load<Texture>("DrawCallsTest/Textures/tex_" + _TextureSizeKey + "_0");

        for (int i = 0; i < _VariantsCount; i++) {
            if (_SampleMaterials.Count > i) {
                Destroy(_SampleMaterials[i]);
            }
            else {
                _SampleMaterials.Add(null);
            }

            _SampleMaterials[i] = new Material(defaultShader);
            _SampleMaterials[i].mainTexture = defaultTexture;

            switch (_TestSubject) {
                case TestSubjects.Texture:
                    string path = "DrawCallsTest/Textures/tex_" + _TextureSizeKey + "_" + i;
                    var texture = Resources.Load<Texture>(path);
                    Debug.Log(path + "  " + texture);
                    _SampleMaterials[i].mainTexture = texture;
                    break;
                case TestSubjects.Shader:
                    _SampleMaterials[i].shader = Shader.Find(baseShaderPath + "_" + i);
                    break;

                case TestSubjects.Mesh:
                    _Meshes[i] = Resources.Load<Mesh>(baseMeshPath + "_" + i);
                    break;
                case TestSubjects.RenderTarget:
                    if(_RenderTextures.Count<=i)
                        _RenderTextures.Add(null);
                    if(_RenderTextures[i]==null)
                        _RenderTextures[i] = new RenderTexture((int) (Screen.width * 0.75f), (int) (Screen.height * 0.75f), 0, RenderTextureFormat.ARGB32);
                    
                    break;
            }
        }

        //setup material clones
        switch (_DrawMode) {
            case DrawModes.DrawDifferent:
            case DrawModes.DrawIdentical:


                bool identical = _DrawMode == DrawModes.DrawIdentical;

                for (int i = 0; i < DrawsCount; i++) {
                    int variantIndex = i % _VariantsCount;
                    if (identical)
                        variantIndex = 0;
                    var mat = _SampleMaterials[variantIndex];

                    if (_Materials.Count == i) {
                        _Materials.Add(null);
                    }
                    else {
                        Destroy(_Materials[i]);
                    }

                    _Materials[i] = Instantiate(mat);
                    _Materials[i].renderQueue = i;
                }

                break;
            case DrawModes.DrawInSingleMesh:

                RecreateSingleMesh();

                break;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!Initialized)
            return;

        Quaternion rotation = Quaternion.identity;
        Vector3 position = Vector3.zero;

        var mesh = _Meshes[0];
        var material = _Materials[0];

        switch (_DrawMode) {
            case DrawModes.DrawInSingleMesh:

                Graphics.DrawMesh(_OneDrawCallMesh, position, rotation, _SampleMaterials[0], 0, _Camera, 0);

                break;

//            case States.DrawSameMaterial:
//
//                for (int i = 0; i < DrawsCount; i++) {
//                    position.z += 0.01f;
//                    position.x = 0.5f - (float) i / DrawsCount;
//                    Graphics.DrawMesh(_Mesh, position, rotation, _Material1, 0, camera, 0);
//                }
//
//                break;

            case DrawModes.DrawDifferent:
            case DrawModes.DrawIdentical:


                for (int i = 0; i < DrawsCount; i++) {
                    if (_DrawMode == DrawModes.DrawDifferent) {
                        switch (_TestSubject) {
                            case TestSubjects.Mesh:
                                mesh = _Meshes[i % _VariantsCount];
                                break;
                            case TestSubjects.RenderTarget:
                                int textureIndex = i % (_VariantsCount + 1);
                                _Camera.targetTexture = textureIndex == 0 ? null : _RenderTextures[textureIndex - 1];
                                break;
                            default:
                                material = _Materials[i];
                                break;
                        }
                    }

                    position.z += 0.01f;
                    position.x = 0.5f - (float) i / DrawsCount;
                    Graphics.DrawMesh(mesh, position, rotation, material, 0, _Camera, 0);
                }

                break;
        }
    }


    private void RecreateSingleMesh() {
        _OneDrawCallMesh = new Mesh();

        var mesh = _Meshes[0];

        var triangles = mesh.triangles.ToList();
        var vertices = mesh.vertices.ToList();
        var uvs = mesh.uv.ToList();
        var normals = mesh.normals.ToList();

        var rTriangles = new List<int>(DrawsCount * 2);
        var rVertices = new List<Vector3>(DrawsCount * 4);
        var rNormals = new List<Vector3>(DrawsCount * 4);
        var rUVs = new List<Vector2>(DrawsCount * 4);

        var iVertices = new List<Vector3>(vertices);
        var iTriangles = new List<int>(triangles);

        for (int i = 0; i < DrawsCount; i++) {
            for (int j = 0; j < iTriangles.Count; j++) {
                iTriangles[j] = triangles[j] + rVertices.Count;
            }

            for (int j = 0; j < iVertices.Count; j++) {
                //move vertices
                iVertices[j] = vertices[j] + new Vector3(0.5f - (float) i / DrawsCount, 0, i * 0.01f);
            }

            rVertices.AddRange(iVertices);
            rTriangles.AddRange(iTriangles);
            rUVs.AddRange(uvs);
            rNormals.AddRange(normals);
        }

        _OneDrawCallMesh.SetVertices(rVertices);
        _OneDrawCallMesh.SetTriangles(rTriangles, 0);
        _OneDrawCallMesh.SetNormals(rNormals);
        _OneDrawCallMesh.SetUVs(0, rUVs);
        _OneDrawCallMesh.RecalculateTangents();
        _OneDrawCallMesh.RecalculateBounds();
    }
}