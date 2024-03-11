using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SeethroughWall : MonoBehaviour
{
    #region cmntcode
    //public Material maskMaterial;
    //public float revealSpeed = 0.1f;
    //private float revealAmount = 0f;

    //private void OnRenderImage(RenderTexture source, RenderTexture destination)
    //{
    //   // Debug.Log("Calling");
    //    Color maskColor = maskMaterial.color;
    //    maskColor.a = revealAmount;
    //    maskMaterial.color = maskColor;


    //    Graphics.Blit(source, destination, maskMaterial);
    //    revealAmount += revealSpeed * Time.deltaTime;
    //    revealAmount = Mathf.Clamp01(revealAmount);
    //}
    #endregion
    public static SeethroughWall Instance;
    public Transform lookPoint;
    public Material normalWall_Mat;
    public Material fadeWall_Mat;
    Vector3 lookDirection;
    RaycastHit hit;
    [SerializeField] List<GameObject> hitobject;
 
    public bool present=false;



    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        Raycastseethrough();
       
    }
   void Raycastseethrough()
    {

        if (lookPoint != null)
        {
            lookDirection = lookPoint.position - transform.position;
            Debug.DrawRay(transform.position, lookDirection, Color.blue, 10);
            if (Physics.Raycast(this.transform.position, lookDirection, out hit))
            {
                if (hit.collider.tag == "Wall")
                {
                    Debug.Log("hitting wall...");
                    Debug.DrawRay(transform.position, lookDirection, Color.blue, 10);
                    Debug.Log("WallName  " + hit.collider.name);
                    hit.collider.GetComponent<Renderer>().material = fadeWall_Mat;


                    foreach (GameObject hitobj in hitobject)
                    {
                        if (hitobj.name == hit.collider.name)
                        {
                            Debug.Log("Already Present....");
                            present = true;
                            break;
                        }
                        else
                        {
                            present = false;
                        }
                    }
                    if (!present)
                    {
                        hitobject.Add(hit.collider.gameObject);

                    }

                }
                else
                {
                    Debug.Log("Not Hitting any wall.");
                    foreach (GameObject hitobj in hitobject)
                    {
                        hitobj.GetComponent<Renderer>().material = normalWall_Mat;

                    }

                    // hitobject.Clear();
                }
            }
        }
    }
   
}
