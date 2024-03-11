using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraSeeThrough : MonoBehaviour
{
    public GameObject camera;
    public GameObject target;

    bool _foundTarget = false;

    public MeshRenderer _prevMesh;
    public List<MeshRenderer> _prevMeshes = new List<MeshRenderer>();
    public MeshRenderer _currentMesh;

    public Material transWallmat;
    public Material transStairMat;
    public Material transRoofMat;
    
    public Material Wallmat;
    public Material StairMat;
    public Material RoofMat;
    
    private void Start()
    {
        target = GameObject.Find("SphereMask");
    }
    
    void Update()
    {
        if (!_foundTarget)
        {
            try
            {
                target = GameObject.Find("SphereMask");
                _foundTarget = true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to find sphereMask");
            }
        }
        //this.gameObject.transform.LookAt(target.transform);
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.LogWarning("Hit object " + hit.collider.gameObject);
            if (hit.collider.gameObject.tag == Tags.WALL_TAG ||
                hit.collider.gameObject.tag == Tags.FLOOR_TAG ||
                hit.collider.gameObject.tag == Tags.PILLAR_TAG ||
                hit.collider.gameObject.tag == Tags.STAIR_TAG ||
                hit.collider.gameObject.tag == Tags.DOOR_TAG ||
                hit.collider.gameObject.tag == Tags.MIRROR_TAG)
            {
                AssignAlpha(hit.collider.gameObject.GetComponent<MeshRenderer>());
            }
            else if (hit.collider.gameObject.tag == "SphereMask" || hit.collider.gameObject.tag == "Player")
            {
                DeAssignAll();
            }
        }
        if (_prevMesh
            != null)
        {
            Debug.LogError("Prev mesh " + _prevMesh);
        }
        if (_currentMesh != null)
        {
            Debug.LogError("Curr mesh " + _currentMesh);
        }
    }
    private void AssignAlpha(MeshRenderer _obj)
    {
        if (_prevMeshes == null)
        {
            _prevMeshes = new List<MeshRenderer>();
        }
        else
        {
            DeAssignAll();
        }

        // Find all the mesh renderers between the player and the camera
        RaycastHit[] hits = Physics.RaycastAll(target.transform.position, this.transform.position - target.transform.position, Vector3.Distance(this.transform.position, target.transform.position));
        foreach (RaycastHit hit in hits)
        {
            Debug.Log("Hit Object " + hit.collider.gameObject);
            MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                _prevMeshes.Add(meshRenderer);
                SetMaterialProperties(_prevMeshes);
            }
        }
    }

    private void SetMaterialProperties(List<MeshRenderer> meshes )
    {

        foreach (MeshRenderer meshRenderer in meshes)
        {
            meshRenderer.enabled = false;
        }
        /*foreach (Material mat in materials)
        {
            mat.SetFloat("_Mode", 2); // Set the rendering mode to "Fade"
            Color col = mat.color;
            col.a = 0f;
            mat.color = col; // Assign the modified color back to the material
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }*/
    }

    private void DeAssignAll()
    {
        if (_prevMeshes != null)
        {
            foreach (MeshRenderer meshRenderer in _prevMeshes)
            {
                DeAssign(meshRenderer);
            }
            _prevMeshes.Clear();
        }
    }

    private void DeAssign(MeshRenderer mesh)
    {

        if(mesh == null){return;}
        
        try
        {
            mesh.enabled = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        /*foreach (Material mat in materials)
        {
            mat.SetFloat("_Mode", 0); // Reset the rendering mode to "Opaque"
            Color col = mat.color;
            col.a = 1f; // Reset the alpha value to 1
            mat.color = col; // Assign the modified color back to the material
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;
        }*/
    }
}

