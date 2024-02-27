using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum E_Sound
{
    Bgm,    //반복재생
    Effect, //짧게 1번만 재생
    MaxCount
}
public class Manager_Sound : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _bgmAudioSource = new List<AudioSource> { };
    [SerializeField] private List<AudioSource> _sfxAudioSource = new List<AudioSource> { };
    [SerializeField] private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); //오디오 클립을 관리할 예정
    public bool IsMute { get; set; }
    public float BGMVolume { get; set; }
    public float SFXVolume { get; set; }

    public static Manager_Sound instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        BGMVolume = 0.4f;
        SFXVolume = 0.4f;
    }
    public void AudioVolume(float audioVolume, int Choice) //설정창에서 사용가능하도록
    {
        switch (Choice)
        {
            case 1: //Bgm 볼륨
                BGMVolume = audioVolume;
                foreach (AudioSource source in _bgmAudioSource)
                    source.volume = BGMVolume;
                break;
            case 2: //Sfx 볼륨
                SFXVolume = audioVolume;
                foreach (AudioSource source in _sfxAudioSource)
                    source.volume = SFXVolume;
                break;
            case 3: //전체 볼륨
                BGMVolume = audioVolume;
                SFXVolume = audioVolume;
                foreach (AudioSource source in _bgmAudioSource)
                    source.volume = BGMVolume; 
                foreach (AudioSource source in _sfxAudioSource)
                    source.volume = SFXVolume;
                break;
        }
    }
    public void AudioMute(bool isMute) //설정창에서 사용가능하도록
    {
        IsMute = isMute;
        foreach (AudioSource source in _bgmAudioSource)
            source.mute = isMute;
        foreach (AudioSource source in _sfxAudioSource)
            source.mute = isMute;
    }
    private void AddClip(string clipName) //클립을 추가시키는 함수
    {
        AudioClip clip = Resources.Load<AudioClip>($"{clipName}");
        _audioClips.Add(clipName, clip);
    }
    private void PlayAudioSource(AudioSource audioSource, AudioClip clip, bool loop, bool BGM) //배경음 
    {
        if (audioSource != null)
        {
            if (!_bgmAudioSource.Contains(audioSource) && BGM)
                _bgmAudioSource.Add(audioSource);
            else if (!_sfxAudioSource.Contains(audioSource) && !BGM)
                _sfxAudioSource.Add(audioSource);
            audioSource.playOnAwake = loop;
            audioSource.loop = loop;
            audioSource.clip = clip;
            audioSource.mute = IsMute;
            audioSource.volume = BGM ? BGMVolume : SFXVolume;
            audioSource.spatialBlend = loop == true ? 1f : 0f; //SpatialBlend의 값이 1이면 3D 0이면 2D이다
            audioSource.Play();
        }
    }
    public void AudioPlay(GameObject audioObject, string name, bool isLoop = false, bool isBGM = false) //해당 오브젝트의 오디오 소스를 가지고 시작함
    {
        AudioSource audioSource = AddAudioSource(audioObject);
        if (_audioClips.TryGetValue(name, out AudioClip audioClip))
        {
            PlayAudioSource(audioSource, audioClip, isLoop, isBGM);
        }
        else
        {
            AddClip(name);
            PlayAudioSource(audioSource, _audioClips[name], isLoop, isBGM);
        }
    }
    private AudioSource AddAudioSource(GameObject audioObject) //오디오가 없으면 추가해주고 있으면 그냥 반환
    {
        AudioSource audio = audioObject.GetComponent<AudioSource>();
        if (audio == null)
        {
            audio = audioObject.AddComponent<AudioSource>();
            return audio;
        }
        else
            return audio;
    }
    public void AudioClear() //좀더 보안해야함, 현재 clip의 값만 초기화를 시켜주는거임
    {
        foreach (var item in _bgmAudioSource)
            item.Stop();
        foreach (var item in _sfxAudioSource)
            item.Stop();
        _bgmAudioSource.Clear();
        _sfxAudioSource.Clear();
    }
}