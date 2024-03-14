using System.Collections.Generic;
using UnityEngine;

public class PlayerSeeThroughEffectManager : MonoBehaviour
{
   public Camera camera;
   public GameObject target;
   public GameObject lookAtCamera;
   
   public MeshRenderer _prevMesh;
   public List<MeshRenderer> _prevMeshes = new List<MeshRenderer>();
   List<MeshRenderer> hitMeshRenderers = new List<MeshRenderer>();
   public MeshRenderer _currentMesh;

   public Material transRoofMat;
   public Material transWallMat;
   public Material transStairMat;
   public Material transPillarMat;
   
   public Material WallMat;
   public Material RoofMat;
   public Material StairMat;
   public Material PillarMat;

   private void Awake()
   {
      target = GameObject.Find("CameraMask");
      camera = Camera.main;
   }

   private void Update()
   {
      if (!target)
      {
         try
         {
            target = GameObject.Find("CameraMask"); 
         }
         catch (System.Exception e)
         {
            Debug.LogError("Failed to find sphereMask");
         }
      }
   }

   private void FixedUpdate()
   {
       
       lookAtCamera.transform.LookAt(target.transform);
      //this.gameObject.transform.LookAt(target.transform);
      Ray ray = new Ray(transform.position, lookAtCamera.transform.forward);
      RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
      hitMeshRenderers.Clear();
      foreach (RaycastHit hit in hits)
      {
          Debug.DrawLine(transform.position, hit.point, Color.magenta);

          if (hit.collider.gameObject.CompareTag(Tags.WALL_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.FLOOR_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.PILLAR_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.STAIR_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.DOOR_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.MIRROR_TAG) ||
              hit.collider.gameObject.CompareTag(Tags.Maze_TAG))
          {
              hitMeshRenderers.Add(hit.collider.gameObject.GetComponent<MeshRenderer>()); // Add the MeshRenderer to the list
              break;
          }
          else if (hit.collider.gameObject.CompareTag("CameraMask") && hitMeshRenderers.Count<1 )
          {
              DeAssignAll();
              break;
          }
      }
      AssignAlpha(hitMeshRenderers);
      if (_prevMesh
          != null)
      {
         //Debug.LogError("Prev mesh " + _prevMesh);
      }
      if (_currentMesh != null)
      {
         //Debug.LogError("Curr mesh " + _currentMesh);
      }
   }
   
   
     private void AssignAlpha( List<MeshRenderer> _obj)
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
            //Debug.Log("Hit Object " + hit.collider.gameObject);
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
            Material []_materials = meshRenderer.materials;
            int index = 0;
            foreach (Material mat in _materials)
            {
                string tempRoofMat = "";
                string tempWallMat = "";
                string tempStairMat = "";
                string tempPillarMat = "";
                if (RoofMat!=null)
                { 
                     tempRoofMat = RoofMat.name + " (Instance)";
                }
                if (WallMat!=null)
                { 
                     tempWallMat = WallMat.name + " (Instance)";
                }
                if (StairMat!=null)
                {
                     tempStairMat = StairMat.name + " (Instance)";
                }
                if (PillarMat!=null)
                {
                     tempPillarMat = PillarMat.name + " (Instance)";
                }
                
                if (mat.name == tempRoofMat && transRoofMat != null)
                {
                    _materials[index] = transRoofMat;
                }else if (mat.name == tempWallMat && transWallMat != null)
                {
                    _materials[index] = transWallMat;
                }else if (mat.name == tempStairMat && transStairMat != null)
                {
                    _materials[index] = transStairMat;
                }else if (mat.name == tempPillarMat && transPillarMat !=null)
                {
                    _materials[index] = transPillarMat;
                }
                ++index;
            }
            meshRenderer.materials = _materials;
        }
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

    private void DeAssign(MeshRenderer meshRenderer)
    {
        if(meshRenderer == null){return;}
        Material []_materials = meshRenderer.materials;
        int index = 0;
        foreach(Material mat in _materials)
        {
            string tempRoofMat = "";
            string tempWallMat = "";
            string tempStairMat = "";
            string tempPillarMat = "";
                
            if (transRoofMat!=null)
            { 
                tempRoofMat = transRoofMat.name + " (Instance)";
            }
            if (transWallMat!=null)
            { 
                 tempWallMat = transWallMat.name + " (Instance)";
            }if (transStairMat!=null)
            {
                 tempStairMat = transStairMat.name + " (Instance)";
            }if (transPillarMat!=null)
            {
                 tempPillarMat = transPillarMat.name + " (Instance)";
            }
            if (mat.name == tempRoofMat && RoofMat != null)
            {
                _materials[index] = RoofMat;
            }else if (mat.name == tempWallMat && WallMat != null)
            {
                _materials[index] = WallMat;
            }else if (mat.name == tempStairMat && StairMat != null)
            {
                _materials[index] = StairMat;
            }else if (mat.name == tempPillarMat && PillarMat != null)
            {
                _materials[index] = PillarMat;
            }
            ++index;
        }
        meshRenderer.materials = _materials;
    }
}
