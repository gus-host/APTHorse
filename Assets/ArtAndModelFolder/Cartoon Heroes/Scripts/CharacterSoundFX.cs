using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFX : MonoBehaviour
{
    private AudioSource SoundFX;

    [SerializeField]
    private AudioClip attackSound_1, attackSound_2, die_Sound; 
    //private void Awake()
    //{
    //}
    // Start is called before the first frame update
    void Start()
    {
        try{
            SoundFX = GameManager.instance.sfx.GetComponent<AudioSource>();
        }
        catch{
        
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack_1()
    {
        SoundFX.clip = attackSound_1;   
        SoundFX.Play();
    }

    public void Attack_2()
    {
        SoundFX.clip = attackSound_2;
        SoundFX.Play();
    }

    public void Die()
    {
        SoundFX.clip = die_Sound;
        SoundFX.Play();
    }
}
