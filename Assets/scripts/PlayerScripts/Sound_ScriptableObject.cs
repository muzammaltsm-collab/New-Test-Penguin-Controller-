
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(fileName = "Sound Data", menuName = "ScriptableObjects/Sound/Sound Data")]
    public class Sound_ScriptableObject : ScriptableObject
    {
        [SerializeField] AudioClip _GoldenFishCollectSound;
        [SerializeField] AudioClip _JumpSound;
        [SerializeField] AudioClip _PlayerDeathSound;
        [SerializeField] AudioClip[] _CollectBlueFishSounds;
       
        [SerializeField] AudioClip _CoinsRewardSound;
        [SerializeField] AudioClip _UpgradeSound;
        [SerializeField] AudioClip _GemsCollectSound;
        [SerializeField] AudioClip _WinSound;
        [SerializeField] AudioClip _PortalInSound;
        [SerializeField] AudioClip _PortalOutSound;
        [SerializeField] AudioClip _BoxHitSound;
        [SerializeField] AudioClip _BarrelHitSound;
        [SerializeField] AudioClip _WoodenLogHitSound;
        [SerializeField] AudioClip _FishHitSound;
        public void PlaySound(AudioSource AS, AudioClip ac)
        {
            if (AS == null)
                return;
            if (ac == null)
                return;
            AS.clip = ac;
            AS.Play();
        }
      
        public void Play_GoldenFishCollectSound(AudioSource AS)
        {
            PlaySound(AS, _GoldenFishCollectSound);
        }
        public void Play_JumpSound(AudioSource AS)
        {
            PlaySound(AS, _JumpSound);
        }
        public void Play_FishHitSound(AudioSource AS)
        {
            PlaySound(AS, _FishHitSound);
        }
        public void Play_BoxHitSound(AudioSource AS)
        {
            PlaySound(AS, _BoxHitSound);
        }
        public void Play_WoodenLogHitSound(AudioSource AS)
        {
            PlaySound(AS, _WoodenLogHitSound);
        }
        public void Play_BarrelHitSound(AudioSource AS)
        {
            PlaySound(AS, _BarrelHitSound);
        }
        public void Play_PlayerDeathSound(AudioSource AS)
        {
            PlaySound(AS, _PlayerDeathSound);
        }
        public void Play_CollectBlueFishSounds(AudioSource AS)
        {
            PlaySound(AS, _CollectBlueFishSounds[UnityEngine.Random.Range(0,
                _CollectBlueFishSounds.Length)]);
        }
       
        public void Play_CoinsRewardSound(AudioSource AS)
        {
            PlaySound(AS, _CoinsRewardSound);
        }
        public void Play_UpgradeSound(AudioSource AS)
        {
            PlaySound(AS, _UpgradeSound);
        }
      
        public void Play_GemsCollectSound(AudioSource AS)
        {
            PlaySound(AS, _GemsCollectSound);
        }
        public void Play__WinSound(AudioSource AS)
        {
            PlaySound(AS, _WinSound);
        } 
        public void Play__PortalInSound(AudioSource AS)
        {
            PlaySound(AS, _PortalInSound);
        }
        public void Play__PortalOutSound(AudioSource AS)
        {
            PlaySound(AS, _PortalOutSound);
        }
        
    }
}