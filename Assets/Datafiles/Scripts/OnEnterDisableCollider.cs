using UnityEngine;

public class OnEnterDisableCollider : MonoBehaviour
{
   public GameObject _object;
   public GameObject []_objects;
   public bool _turnOnOrOff=false;

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag(Tags.PLAYER_TAG))
      {
         if(_object != null)
         {
            Debug.LogError("Enabling");
            _object.SetActive(true);
         }
         if(_objects.Length > 0 )
         {
             foreach (var obj in _objects)
             {
                    obj.SetActive(_turnOnOrOff);
             }
         }
         this.gameObject.SetActive(false);
      }
      if(other.gameObject.CompareTag(MapPointsWeekThree.PlayersClaw))
      {
         Debug.LogError("Enabling");
            if (_object != null)
            {
                Debug.LogError("Enabling");
                _object.SetActive(true);
            }
            if (_objects.Length > 0)
            {
                foreach (var obj in _objects)
                {
                    obj.SetActive(_turnOnOrOff);
                }
            }
            gameObject.SetActive(false);
      }
   }
   private void OnCollisionEnter(Collision collision)
   {
       if (collision.gameObject.CompareTag(Tags.PLAYER_TAG))
       {
           if (_object != null)
           {
               Debug.LogError("Enabling");
               _object.SetActive(true);
           }
           if (_objects.Length > 0 )
           {
               foreach (var obj in _objects)
               {
                   _object.SetActive(_turnOnOrOff);
               }
           }
           this.gameObject.SetActive(false);
       }
   }
}
