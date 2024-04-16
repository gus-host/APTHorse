using Photon.Pun;
using TMPro;
using UnityEngine;

public class OnEnterDisableCollider : MonoBehaviourPunCallbacks, IPunObservable
{
   public GameObject _object;
   public GameObject []_objects;
   public Transform []_nextCheckpoint;
   public bool _turnOnOrOff=false;

   public int checkpointIndex = 0;

    public TMP_Text _text;

    private void Start()
    {
        _text.text = (checkpointIndex + 1).ToString();
        LeanTween.scale(_text.gameObject, new Vector3(0, 0, 0), 2)
            .setEaseInElastic()
            .setLoopPingPong().setOnComplete(() => {
               /* LeanTween.scale(_text.gameObject, new Vector3(1, 1, 1), 2)
            .setEaseInElastic()
            .setLoopPingPong();*/ });
    }

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
    public void UpdateCheckPoint()
    {
        if(checkpointIndex < _nextCheckpoint.Length )
        {
            transform.position = _nextCheckpoint[checkpointIndex++].transform.localPosition;
            _text.text = (checkpointIndex+1).ToString();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Serialize fields to send over the network
            stream.SendNext(_turnOnOrOff);
            stream.SendNext(checkpointIndex);
        }
        else
        {
            // Deserialize received data
            _turnOnOrOff = (bool)stream.ReceiveNext();
            checkpointIndex = (int)stream.ReceiveNext();
        }
    }
}
