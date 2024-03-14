using UnityEngine;

namespace MagicArsenal
{

public class MagicLoadSceneOnClick : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	
    public void LoadSceneProjectiles()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_projectiles");
    }
    public void LoadSceneSprays()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_sprays");
    }
    public void LoadSceneAura()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_aura");
    }
    public void LoadSceneModular()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_modular");
    }
    public void LoadSceneShields2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_domes");
    }
    public void LoadSceneShields()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_shields");
    }
    public void LoadSceneSphereBlast()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_sphereblast");
    }
    public void LoadSceneEnchant()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_enchant");
    }
    public void LoadSceneSlash()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_slash");
    }
    public void LoadSceneCharge()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_charge");
    }
    public void LoadSceneCleave()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_cleave");
    }
    public void LoadSceneAura2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_aura2");
    }
    public void LoadSceneWalls()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_walls");
    }
	public void LoadSceneBeams()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_beams");
    }
	public void LoadSceneMeshGlow()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_meshglow");
    }
	public void LoadScenePillarBlast()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_pillarblast");
    }
	public void LoadSceneAura3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_aura3");
    }
	public void LoadSceneAuraCast()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_auracast");
    }
	public void LoadSceneRain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_rain");
    }
	public void LoadSceneAOE()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_aoe");
    }
	public void LoadSceneNova()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_nova");
    }
	public void LoadSceneFlame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_flame");
    }
	public void LoadSceneAuraCast2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_auracast2");
    }
	public void LoadSceneCurse()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_curse");
    }
	public void LoadSceneBeamBlast()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_beamblast");
    }
	public void LoadSceneOrbitSphere()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_orbitsphere");
    }
	public void LoadSceneDot()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("magic_dot");
    }

	
	void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.K))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = true;
         }
     }
	 }
}
}