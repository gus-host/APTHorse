using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerRoomProfile : MonoBehaviour
{
        public TMP_Text _name;

        public void SetName(string val)
        {
                _name.text = val;
        }
}
