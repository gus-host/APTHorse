using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stamina_Script : MonoBehaviour
{
    public Image staminaImage; // the UI image that displays the player's stamina
    public TextMeshProUGUI staminatxt; // the UI image that displays the player's stamina
    public Image HealthImage;

    public IntrantThirdPersonController _playerController;
    
    public float TotalStamina = 50;
    public float maxStamina = 50f; // the maximum amount of stamina the player can have
    public float currentStamina; // the current amount of stamina the player has
    public float cautionStamina = 25; // the current amount of stamina the player has
    public float staminaIncreaseRate = 1f; // the rate at which stamina decreases when moving
    public float staminaDecreaseRate = 1f; // the rate at which stamina decreases when moving
    public float staminaReplenishRate = 0.2f; // the rate at which stamina replenishes when standing still
    public float distanceTravelled = 0f; // the distance the player has traveled

    private float timeSinceLastMovement = 0f; // the time since the player last moved
    private float timeSinceLastIdle = 0f; // the time since the player last stood still




    // Health variables
    private float TotalHealth = 50;
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float healthDecreaseRate = 1.0f;
    public float healthDecreaseInterval = 5.0f;
    private float timeSinceLastHealthDecrease = 0.0f;



    [Header("...........Stamina and Health Decrease Rate.............")]
    public float StaminadownRate = .1f;
    public float StaminaCallRate = .2f;
    public float MaxtravelDisInoneTime;
    [Header("...........Stamina fill Rate.............")]
    public float StaminafillRate = .1f;
    public float StaminafillCallRate = .2f;


    BarFilling BarFilling = new BarFilling();

    [Header("Bools")]
    public bool _isIncreasing = false;
    // public float MaxtravelDisInoneTime;
    void Start()
    {
        //Getting Saved Stamina
        /*maxStamina = PlayerPrefs.GetInt("Stamina");*/
        
        Debug.Log("MaxStamina " + maxStamina);
        staminatxt.text = "SP ";        
        staminaImage.fillAmount = maxStamina;
        currentStamina = maxStamina;
    }

    void Update()
    {
        Stamina();
    }

    void Stamina()
    {
        StaminaHandling();

        VFXHandling();

        SpeedHandling();
    }

    private void SpeedHandling()
    {
        if (maxStamina >= 50)
        {
            switch (currentStamina)
            {
                case <= 15:
                    _playerController.movement_Speed = 2;
                    break;
                case <= 35:
                    _playerController.movement_Speed = 3;
                    break;
                case >= 50:
                    _playerController.movement_Speed = 4;
                    break;
            }
        }
        else if(maxStamina <= 50)
        {
            switch (currentStamina)
            {
                case <= 5:
                    _playerController.movement_Speed = 2;
                    break;
                case <= 25:
                    _playerController.movement_Speed = 3;
                    break;
                case >= 40:
                    _playerController.movement_Speed = 4;
                    break;
            }
        }
        
    }

    private void StaminaHandling()
    {
        if (_playerController.input != Vector3.zero && currentStamina >= 0 && _playerController.isSprinting)
        {
            DecreaseStamina();
        }
        else if (currentStamina <= maxStamina && _playerController.input == Vector3.zero)
        {
            IncreaseStamina();
        }
    }

    private void VFXHandling()
    {
        if (currentStamina < cautionStamina && !_isIncreasing)
        {
            _playerController._debuff.SetActive(true);
            _playerController._buff.SetActive(false);
        }
        if (currentStamina == maxStamina)
        {
            _playerController._debuff.SetActive(false);
            _playerController._buff.SetActive(false);
        }
    }

    private void IncreaseStamina()
    {
        if (currentStamina > 25)
        {
            GetComponent<IntrantThirdPersonController>().canSprint = true;
            GetComponent<PvPPlayerUI>()._sprint.interactable = true;
        }
        //Calculate Factor for increasing
        float staminaDelta = staminaIncreaseRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina + staminaDelta, 0f, maxStamina);

        // Calculate the normalized stamina value
        float normalizedStamina = currentStamina / maxStamina;

        // Show the stamina value
        staminatxt.text = "SP ";
        staminaImage.fillAmount = normalizedStamina;

        _playerController._debuff.SetActive(false);
        _playerController._buff.SetActive(true);
        
        _isIncreasing = true;
    }

    private void DecreaseStamina()
    {
        if (currentStamina < 25)
        {
            GetComponent<IntrantThirdPersonController>().canSprint = false;
            GetComponent<PvPPlayerUI>()._sprint.interactable = false;
        }
        
        //Calculate factor for decreasing
        float staminaDelta = staminaDecreaseRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina - staminaDelta, 0f, maxStamina);
        float _normalizedStamina = currentStamina / maxStamina;
        
        //Show value
        staminatxt.text = "SP ";
        staminaImage.fillAmount = _normalizedStamina;

        _isIncreasing = false;

        _playerController._buff.SetActive(false);
    }

    public void ReduceStamina(int val)
    {
        if(currentStamina > val)
        {
            currentStamina = currentStamina - val;
        }
        else
        {
            GetComponent<IntrantPlayerHealthManager>().ReduceHealth(val);
        }
    }
}
