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
    [SerializeField] private List<AudioSource> _arrAudioSource = new List<AudioSource> { };
    [SerializeField] private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>(); //오디오 클립을 관리할 예정

    public static Manager_Sound instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void AudioVolume(float audioVolume) //설정창에서 사용가능하도록
    {
        foreach (AudioSource source in _arrAudioSource)
            source.volume = audioVolume;
    }
    private void AddClip(string clipName) //클립을 추가시키는 함수
    {
        AudioClip clip = Resources.Load<AudioClip>($"{clipName}");
        _audioClips.Add(clipName, clip);
    }
    private void PlayAudioSource(AudioSource audioSource, AudioClip clip, bool loop = true) //배경음 
    {
        if (audioSource != null)
        {
            if (!_arrAudioSource.Contains(audioSource))
                _arrAudioSource.Add(audioSource);
            audioSource.playOnAwake = loop;
            audioSource.loop = loop;
            audioSource.clip = clip;
            audioSource.spatialBlend = loop == true ? 1f : 0f; //SpatialBlend의 값이 1이면 3D 0이면 2D이다
            audioSource.Play();
        }
    }
    public void AudioPlay(GameObject audioObject, string name, bool isLoop = false) //해당 오브젝트의 오디오 소스를 가지고 시작함
    {
        AudioSource audioSource = AddAudioSource(audioObject);
        if (_audioClips.TryGetValue(name, out AudioClip audioClip))
        {
            PlayAudioSource(audioSource, audioClip, isLoop);
        }
        else
        {
            AddClip(name);
            PlayAudioSource(audioSource, _audioClips[name], isLoop);
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
        foreach (var item in _arrAudioSource)
            item.Stop();
        _arrAudioSource.Clear();
    }
}