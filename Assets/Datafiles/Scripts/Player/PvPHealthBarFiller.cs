using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PvPHealthBarFiller : MonoBehaviourPunCallbacks
{
   [SerializeField] private PvPPlayerHealth health = null;
   [SerializeField] private GameObject healthBarParent = null;
   [SerializeField] private Image healthBarImage = null;

   private void Awake()
   {
      health.ClientOnHealthUpdated += HandleHealthUpdated;
   }

   private void OnDestroy()
   {
      health.ClientOnHealthUpdated -= HandleHealthUpdated;
   }

   /*private void HandleHealthUpdated(int currenthealth, int maxHealth, PhotonView view)
   {
     float amount = (float)currenthealth / maxHealth;
     view.RPC("RPCUpdateFill", RpcTarget.All,amount);
   }*/

   private void HandleHealthUpdated(int currenthealth, int maxHealth, PhotonView view)
   {
      float amount = (float)currenthealth / maxHealth;
      if (view.IsMine)
      {
         view.RPC("RPCUpdateFill", RpcTarget.All, amount, maxHealth);
      }
      Debug.LogError("RPCUpdateFill called with amount: " + amount);
   }
   
   [PunRPC]
   private void RPCUpdateFill(int amount, int maxHealth)
   {
      healthBarImage.fillAmount = (float)amount / maxHealth;;
   }
}
