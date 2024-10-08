using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private List<AudioSource> audioSources_2D;

	private GameObject audioSourcesObject;

	public static AudioManager This { get; private set; }

	private void Awake()
	{
		This = this;
	}

	private void Start()
	{
		audioSources_2D = new List<AudioSource>();
		audioSourcesObject = new GameObject("AudioSources");
	}

	public virtual void PlaySound_2D(AudioClip _clip)
	{
		AudioSource audioSource = null;
		for (int i = 0; i < audioSources_2D.Count; i++)
		{
			if (!audioSources_2D[i].isPlaying)
			{
				audioSource = audioSources_2D[i];
				break;
			}
		}
		if (audioSource == null)
		{
			audioSource = audioSourcesObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSources_2D.Add(audioSource);
		}
		audioSource.clip = _clip;
		audioSource.Play();
	}

	public virtual void PlaySound_2D_Loop(AudioClip _clip)
	{
		AudioSource audioSource = audioSourcesObject.AddComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = _clip;
		audioSource.Play();
	}

	public AudioClip GetClip()
	{
		return null;
	}
}
