using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Sound
{
    Bgm,    //반복재생
    Effect, //짧게 1번만 재생
    MaxCount
}
public class Manager_Sound : MonoBehaviour
{
    Dictionary<string, AudioSource> _audioSource = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); //오디오 클립을 관리할 예정

    public static Manager_Sound instance;
    private void Awake()
    {
        instance = this;
    }
    public void AddClip(string clipName)
    {
        AudioClip clip = Resources.Load($"{clipName}") as AudioClip;
        _audioClips.Add(clipName, clip);
    }
    public AudioSource PlayBGM(string bgmName, bool loop = true)
    {
        if (!_audioSource.TryGetValue(bgmName, out AudioSource source))
        {
            source = gameObject.AddComponent<AudioSource>();
            source.clip = _audioClips[bgmName];
            source.loop = loop;
            _audioSource[bgmName] = source;
        }
        return _audioSource[bgmName];
    }
    public void PlaySFX(AudioSource audioSource, string sfxName) //게임오브젝트의 AudioSource에 접근해서 값 전달하기, 게임오브젝트의 AudiloSource생성하게 하기
    {
        if (_audioClips.TryGetValue(sfxName, out AudioClip source))
        {
            audioSource.clip = source;
            audioSource.loop = false;
        }
        Debug.Log(audioSource.name);
        audioSource.Play();
    }
    
    public void AudioClear() //신전환시 클리어하게 하기
    {
        
    }
}