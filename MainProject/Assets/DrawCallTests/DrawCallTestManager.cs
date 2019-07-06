using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class DrawCallTestManager : BaseTestManager
{
    [SerializeField] private GameObject Template1;
    [SerializeField] private GameObject Template2;

    private enum States
    {
        DrawDifferent,
        DrawSame,
        DrawInSingleMesh
    }

    private States _State;

    private Mesh _Mesh;
    private Mesh _OneDrawCallMesh;
    private Material _Material1;
    private Material _Material2;

    private Material[] _Materials;
    private bool _UseSameMaterial;

    // Start is called before the first frame update
    protected override void OnEnable() {
        base.OnEnable();

        _Mesh = Template1.GetComponent<MeshFilter>().sharedMesh;
        _Material1 = Template1.GetComponent<MeshRenderer>().material;
        _Material2 = Template2.GetComponent<MeshRenderer>().material;
        Template1.SetActive(false);
        Template2.SetActive(false);

        SetState(States.DrawDifferent);
    }

    protected override void OnDrawsCountChanged() {
        _Materials = new Material[DrawsCount];
        SetState(_State);
    }

    // Update is called once per frame
    void Update() {
        Quaternion rotation = Quaternion.identity;
        Vector3 position = Vector3.zero;
        Camera camera = Camera.main;

        switch (_State) {
            case States.DrawInSingleMesh:

                Graphics.DrawMesh(_OneDrawCallMesh, position, rotation, _Material1, 0, camera, 0);

                break;

            case States.DrawDifferent:
            case States.DrawSame:

                for (int i = 0; i < DrawsCount; i++) {
                    position.z += 0.01f;
                    position.x = 0.5f - (float) i / DrawsCount;
                    Graphics.DrawMesh(_Mesh, position, rotation, _Materials[i], 0, camera, 0);
                }


                break;
        }
    }

    protected override void Handler_SwitchTriggered() {
        SetState(_State.Next());
    }

    private void SetState(States state) {
        _State = state;
        switch (_State) {
            case States.DrawSame:

                SetupMaterials(true);

                break;
            case States.DrawDifferent:

                SetupMaterials(false);

                break;

            case States.DrawInSingleMesh:


                _OneDrawCallMesh = new Mesh();

                var triangles = _Mesh.triangles.ToList();
                var vertices = _Mesh.vertices.ToList();
                var uvs = _Mesh.uv.ToList();
                var normals = _Mesh.normals.ToList();

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

                break;
        }

        base.Handler_SwitchTriggered();
        SetSwitchName(_State.ToString());
    }

    private void SetupMaterials(bool useSameMaterial) {

        if (_Materials == null || _Materials.Length < DrawsCount) {
            _Materials=new Material[DrawsCount];
        }
        
        for (int i = 0; i < DrawsCount; i++) {
            var mat = (!useSameMaterial && i % 2 == 0) ? _Material1 : _Material2;
            if (_Materials[i] != null) {
                Destroy(_Materials[i]);
            }

            _Materials[i] = Instantiate(mat);
            _Materials[i].renderQueue = i;
        }
    }
}