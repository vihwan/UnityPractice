using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡의 소스
}

public class SoundManager : MonoBehaviour
{
    //Singleton 기법 사용
    //사운드 매니저는 반드시 '하나만' 존재해야한다.
    public static SoundManager instance;

    //객체 생성시 최초 실행
    #region Singleton
    private void Awake()
    {
        //사운드 매니저가 없다면 새로 만든다
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //자기 자신을 파괴하지 말아라.
        }
        else
        {
            //기존꺼가 존재한다면 파괴시킨다.
            Destroy(this.gameObject);
        }          
    }
    #endregion Singleton

    public AudioSource[] audioSourceEffects; //여러개를 동시에 생성할수도 있으니 배열로
    public AudioSource audioSourceBgm; //배경음악은 하나만 존재해도 되니 배열이 아님

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public string[] playSoundName;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                return;
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되어있지 않습니다.");

    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }
    
    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                break;
            }
        }
        Debug.Log("재생 중안" + name + "사운드가 없습니다.");
    }

}
