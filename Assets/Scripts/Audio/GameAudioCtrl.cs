using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音频模块控制器
/// </summary>
public class GameAudioCtrl : AudioBase
{
    public AudioClip deathAudio;
    public AudioClip pickUpArmsAduio;
    public AudioClip pickUpCureAudio;
    public AudioClip pickUpFoodAudio;

   void Awake()
    {
        Bind(AudioEvent.PLAY_DEATH_AUDIO,AudioEvent.PLAY_PICKUP_ARM,
            AudioEvent.PLAY_PICKUP_CURE,AudioEvent.PLAY_PICKUP_FOOD); 
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case AudioEvent.PLAY_DEATH_AUDIO:
                PlayDeathAudio((Vector3)message);
                break;
            case AudioEvent.PLAY_PICKUP_ARM:
                PlayArmsAudio((Vector3)message);
                break;
            case AudioEvent.PLAY_PICKUP_CURE:
                PlayCureAudio((Vector3)message);
                break;
            case AudioEvent.PLAY_PICKUP_FOOD:
                PlayFoodAudio((Vector3)message);
                break;
            default:
                break;
        }
    }

    private void PlayDeathAudio(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(deathAudio, pos);
    }

    private void PlayArmsAudio(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(pickUpArmsAduio, pos);
    }

    private void PlayCureAudio(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(pickUpCureAudio, pos);
    }

    private void PlayFoodAudio(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(pickUpFoodAudio, pos);
    }

}
