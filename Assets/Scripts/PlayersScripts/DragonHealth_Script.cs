using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragonHealth_Script : MonoBehaviour
{
    public float Health = 100f;
    public float x_Death = -90f;
  /*  private float death_smooth = 0.9f;
    private float rotate_Time = 0.23f;*/
    private bool playerDied;

    public GameObject GameOver_Panel;
    public GameObject Replay_Btn;
    public GameObject QuitBtn;

    public GameObject DungeonEnvironment;
    public GameObject LevelComplete_Panel;

    public GameObject Dragon;
    public GameObject joystick_Controller;
    public GameObject Attack_Btn;
    public GameObject Dragon_HealthUI;

    // public GameObject BossMonster;

    // public GameObject BossMonster;

    public bool isPlayer;

    [SerializeField]
    private Image health_UI;

    [HideInInspector]
    public bool shieldActivated;

    public GameObject Dead_BloodSplatter;
    public GameObject BloodSplatter_Hit;
    public GameObject PlayerHealth_Canvas;
    public GameObject Player;

    private CharacterSoundFX soundFX;
    public GameObject HealthUI_Canvas;
   

    private Animator _DragonAnim;
    private Animator BloodSplatter_anim;


    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
        soundFX = GetComponentInChildren<CharacterSoundFX>();
        BloodSplatter_anim = BloodSplatter_Hit.GetComponent<Animator>();
        _DragonAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public void ApplyDamage(float Damage)
    {
        //print("Apply Damage,.....");

        if (isPlayer)
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", true);
            Invoke("BloodSplatterOnHit", 0.5f);
        }
        else
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", true);
            Invoke("BloodSplatterOnHit", 0.5f);
        }

        if (shieldActivated)
        {
            return;
        }

        Health = Health - Damage;

        if (health_UI != null)
        {
            health_UI.fillAmount = Health / 100f;
        }

        if (Health <= 0)
        {
            _DragonAnim.SetTrigger("isDead");
            GameManager.instance.sfx.Die();

            //    GetComponent<Animator>().enabled = false;
            //   StartCoroutine(AllowRotate());
            HealthUI_Canvas.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = false;

            if (isPlayer)
            {
                GetComponent<PlayerMovementManager>().enabled = false;
                GetComponent<PlayerAttackScript>().enabled = false;
                GameManager.instance.sfx.Die();
                GameObject.FindGameObjectWithTag("Dragon").GetComponent<DragonController>().enabled = false;
                
                Dead_BloodSplatter.SetActive(true);
                Invoke("ActivateGameOver_Panel", 3f);
            }

            else
            {
                playerDied = true;
               // afterDragon_Death();

                GetComponent<DragonController>().enabled = false;
                Dead_BloodSplatter.SetActive(true);
                GetComponent<NavMeshAgent>().enabled = false;

                Invoke("afterDragon_Death", 3f);    
            }
        }

    }

    void afterDragon_Death()
    {
        DungeonEnvironment.SetActive(false);
        LevelComplete_Panel.SetActive(true);
        Replay_Btn.SetActive(true);
        QuitBtn.SetActive(true);
        Dragon.SetActive(false);
        PlayerHealth_Canvas.SetActive(false);
        Player.SetActive(false);
        joystick_Controller.SetActive(false);
        Attack_Btn.SetActive(false);
        Dragon_HealthUI.SetActive(false);

    }

    void BloodSplatterOnHit()
    {
        if (isPlayer)
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", false);
            //Invoke("BloodSplatterOnHit", 0.5f);
        }
        else
        {
            BloodSplatter_anim.SetBool("isBloodSplatter", false);
            //  Invoke("BloodSplatterOnHit", 0.5f);
        }
    }

    void ActivateGameOver_Panel()
    {
        Dragon.SetActive(false);
        GameOver_Panel.SetActive(true);
        Replay_Btn.SetActive(true);
        QuitBtn.SetActive(true);
        DungeonEnvironment.SetActive(false);
        PlayerHealth_Canvas.SetActive(false);
        Player.SetActive(false);
        joystick_Controller.SetActive(false);
        Attack_Btn.SetActive(false);
        Dragon_HealthUI.SetActive(false);


    }

    /*   void RotateAfterDeath()
         {
             transform.eulerAngles = new Vector3(Mathf.Lerp(transform.eulerAngles.x, x_Death, Time.deltaTime * death_smooth), transform.eulerAngles.y, transform.eulerAngles.z);
         }   
             IEnumerator AllowRotate()
         {
             playerDied = true;
             yield return new WaitForSeconds(rotate_Time);


             playerDied = false;
         }     */
}
