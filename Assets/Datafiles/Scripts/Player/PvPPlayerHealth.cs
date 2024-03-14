using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PvPPlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField] public bool _isPlayer;
    [SerializeField] public Image _fill;
    
    [SerializeField] private int maxHealth = 100;

    public int currentHealth;
    public event Action<int, int, PhotonView> ClientOnHealthUpdated;

    public PvPHealthBarFiller _healthBarInstance = null;
    public RectTransform _healthBar;
    public event Action ServerOnDie;

    private void Start()
    {
        HandleHealthUpdated(0, maxHealth);
    }

    #region Server
    
    public void DealDamage(int damageAmount)
    {
        Debug.LogError("Dealing Damage");
        if(currentHealth == 0 ){return;}
        if(photonView.IsMine){return;}
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        float fillAmount = (float)currentHealth / maxHealth;
        HandleHealthUpdated(0, currentHealth);
        Debug.LogError($"current Health {currentHealth} and {fillAmount}");
        if(currentHealth != 0){return;}
        
        ServerOnDie?.Invoke();

        Debug.Log("We Died");
    }
    #endregion


    #region Client
   
    private void HandleHealthUpdated(int oldHealth = 0, int newhealth = 0)
    {
        ClientOnHealthUpdated?.Invoke(newhealth,maxHealth, photonView);
        photonView.RPC("RpcUpdateHealth", RpcTarget.All, newhealth);
    }

    [PunRPC]
    private void RpcUpdateHealth(int val)
    {
        currentHealth = val;
        _fill.fillAmount = (float)val / maxHealth;
    }
    
    #endregion
}
