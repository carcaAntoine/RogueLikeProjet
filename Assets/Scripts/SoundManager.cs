using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
        public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
        public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.				
        public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

        void Awake ()
		{
			//Check if there is already an instance of SoundManager
			if (instance == null)
				
				instance = this;
			//If instance already exists:
			else if (instance != this)
				Destroy (gameObject);

			DontDestroyOnLoad (gameObject);
		}

        //Used to play single sound clips.
		public void PlaySingle(AudioClip clip)
		{
			efxSource.clip = clip;
			efxSource.Play ();
		}
		
		
		//RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
		public void RandomizeSfx (params AudioClip[] clips)
		{
			int randomIndex = Random.Range(0, clips.Length);
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			efxSource.pitch = randomPitch;
			efxSource.clip = clips[randomIndex];
			efxSource.Play();
		}
    }
}