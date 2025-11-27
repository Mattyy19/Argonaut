using UnityEngine;
using System.Collections.Generic;
using System;


// Found @ https://www.youtube.com/watch?v=2ldKKV1h5Ww
public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        BatBat_Screech, //Done  //Complete
        BatBat_Swing,   //Done
        BatBat_Hit,     //Done
        BatBat_Hurt,    //Done

        StabSq_Squish, //Done  //Complete
        StabSq_Stab,   //Done
        StabSq_Hit,    //Done
        StabSq_Hurt,   //Done

        ShiSnl_Guard,   //Done  //Complete
        ShiSnl_Hurt,    //Done
        ShiSnl_Deflect, //Done
        ShiSnl_Death,   //Done

        PistPn_Quack, 
        PistPn_Shoot, //Done
        PistPn_Hurt,  //Done

        SawSnk_Hiss, //Done  //Complete
        SawSnk_Roll, //Done
        SawSnk_Hit,  //Done
        SawSnk_Hurt, //Done

        ThundT_Charge,    //Done  //Complete
        ThundT_Summon,    //Done
        ThundT_Lightning, //Done
        ThundT_Hurt,      //Done
        ThundT_Death,     //Done

        ScimSl_Squish, //Done  //Complete
        ScimSl_Attack, //Done
        ScimSl_Hurt,   //Done

        MorStS_Squish, //Done  //Complete
        MorStS_Attack,  //Done
        MorStS_Hurt,    //Done

        KingGG_Squish, //Done  //Complete
        KingGG_Shoot,  //Done
        KingGG_Summon, //Done
        KingGG_Hurt,   //Done

        // Add more sound types

        none
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;
    }

    //Singleton
    public static AudioManager Instance;

    //All sounds and their associated type - Set these in the inspector
    public Sound[] AllSounds;

    //Runtime collections
    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();
    private AudioSource _musicSource;

    private void Awake()
    {
        //Assign singleton
        Instance = this;

        //Set up sounds
        foreach (var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }
    }



    //Call this method to play a sound
    public void Play(SoundType type)
    {
        //Make sure there's a sound assigned to your specified type
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        //Creates a new sound object
        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        //Assigns your sound properties
        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;

        //Play the sound
        audioSrc.Play();

        //Destroy the object
        Destroy(soundObj, s.Clip.length);
    }

    //Call this method to change music tracks
    public void ChangeMusic(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }

        if (_musicSource == null)
        {
            var container = new GameObject("SoundTrackObj");
            _musicSource = container.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        _musicSource.clip = track.Clip;
        _musicSource.Play();
    }

    //Personal method
    //Method to get sound type for scripts used in multiple spots
    public static SoundType getSound(string sound)
    {
        if (sound != "" && sound != null)
        {
            return (SoundType)Enum.Parse(typeof(SoundType), sound);
        } 
        else
        {
            return SoundType.none;
        }
    }
}