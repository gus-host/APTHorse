using UnityEngine;

namespace SciFiArsenal
{
public class SciFiLoadSceneOnClick : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	public bool GUIHide4 = false;
	
    public void LoadSceneSciFiProjectiles()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_projectiles");
	}
    public void LoadSceneSciFiBeamup()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_beamup");
	}
    public void LoadSceneSciFiBuff()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_buff");
	}
    public void LoadSceneSciFiFlamethrowers2()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_flamethrowers");
	}
    public void LoadSceneSciFiQuestZone()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_hexagonzone");
	}
    public void LoadSceneSciFiLightjump()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_lightjump");
	}
    public void LoadSceneSciFiLoot()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_loot");
	}
    public void LoadSceneSciFiBeams()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_beams");
    }
    public void LoadSceneSciFiPortals()  {
        UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_portals");
    }
    public void LoadSceneSciFiRegenerate() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_regenerate");
    }
    public void LoadSceneSciFiShields() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_shields");
    }
    public void LoadSceneSciFiSwirlyAura() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_swirlyaura");
    }
    public void LoadSceneSciFiWarpgates() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_warpgates");
    }
    public void LoadSceneSciFiJetflame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_jetflame");
    }
    public void LoadSceneSciFiUltimateNova(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_ultimatenova");
    }
	public void LoadSceneSciFiFire(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("scifi_fire");
    }
	public void LoadSceneSciFiUpdate6()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_6");
	}
	public void LoadSceneSciFiUpdate7()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_7");
	}
	public void LoadSceneSciFiUpdate8()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_8");
	}
	public void LoadSceneSciFiUpdate9()  {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_9");
	}
	public void LoadSceneSciFiUpdate10() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_10");
	}
	public void LoadSceneSciFiUpdate11() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_11");
	}
	public void LoadSceneSciFiUpdate12() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("update_scifi_12");
	}
	public void LoadSceneSciFiBlood() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_blood");
	}
	public void LoadSceneSciFiRoundZone() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scifi_roundzone");
	}



	void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("SciFiSceneSelectNew").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("SciFiSceneSelectNew").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("SciFiProjectileCanvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("SciFiProjectileCanvas").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.H))
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
		if(Input.GetKeyDown(KeyCode.K))
	 {
         GUIHide4 = !GUIHide4;
     
         if (GUIHide3)
		 {
             GameObject.Find("SciFiBeamCanvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("SciFiBeamCanvas").GetComponent<Canvas> ().enabled = true;
         }
     }
	}
}
}