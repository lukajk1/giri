using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADFM : MonoBehaviour
{
    [Header("Unit")]
    [SerializeField] protected AudioClip shieldBreak;
    [SerializeField] protected AudioClip onHitCrit;
    [SerializeField] protected AudioClip onHitNormal;
    [SerializeField] protected AudioClip lifesteal;


    [Header("Player")]
    [SerializeField] protected AudioClip playerAttack;
    [SerializeField] protected AudioClip blink;
    [SerializeField] protected AudioClip playerDeath;

    [Header("Enemy")]
    [SerializeField] protected AudioClip enemyDeath;
    [SerializeField] protected AudioClip enemyCastAttack;

    [Header("UI")]
    [SerializeField] protected AudioClip buttonHover;
    [SerializeField] protected AudioClip cdNotUp;
    [SerializeField] protected AudioClip cdUp;

    [Header("Misc")]
    [SerializeField] protected AudioClip itemDropPickup;

    [Header("Music")]
    [SerializeField] protected AudioSource song;

    public enum Sfx { ShieldBreak=1, OnHitCrit=2, OnHitNormal=3, Lifesteal=4, PlayerAttack=5, Blink=6, EnemyDeath=7, ButtonHover=8, CDNotUp=9, CDUp=10, ItemDropPickup=11, PlayerDeath=12, EnemyCastAttack=13}

    protected AudioSource UI;
    protected List<AudioData> audioDataList = new List<AudioData>();
    protected List<AudioSource> audioSourceList = new List<AudioSource>();
    protected List<AudioSource> audioSourceListMinusUI = new List<AudioSource>();
    protected List<AudioSource> pausedSources = new List<AudioSource>();
    protected void InitializeAudioDataSources()
    {
        AudioSource hits = gameObject.AddComponent<AudioSource>();
        AudioSource playerTakeDamage = gameObject.AddComponent<AudioSource>();
        AudioSource playerSFX = gameObject.AddComponent<AudioSource>();

        AudioSource enemy = gameObject.AddComponent<AudioSource>();
        AudioSource enemyAttack = gameObject.AddComponent<AudioSource>();

        AudioSource game = gameObject.AddComponent<AudioSource>();
        AudioSource playerHealth = gameObject.AddComponent<AudioSource>();
        UI = gameObject.AddComponent<AudioSource>();

        audioSourceList = new List<AudioSource> {
            hits, playerSFX, enemy, enemyAttack, game, playerHealth, UI, playerTakeDamage
        };

        // string, AudioClip, AudioSource
        audioDataList.Add(new AudioData(Sfx.ShieldBreak, shieldBreak, hits));
        audioDataList.Add(new AudioData(Sfx.PlayerAttack, playerAttack, hits));

        audioDataList.Add(new AudioData(Sfx.OnHitCrit, onHitCrit, playerSFX));
        audioDataList.Add(new AudioData(Sfx.OnHitNormal, onHitNormal, playerSFX));

        audioDataList.Add(new AudioData(Sfx.EnemyDeath, enemyDeath, enemy));

        audioDataList.Add(new AudioData(Sfx.EnemyCastAttack, enemyCastAttack, enemyAttack));

        audioDataList.Add(new AudioData(Sfx.Lifesteal, lifesteal, playerHealth));

        audioDataList.Add(new AudioData(Sfx.Blink, blink, game));

        audioDataList.Add(new AudioData(Sfx.ItemDropPickup, itemDropPickup, UI));
        audioDataList.Add(new AudioData(Sfx.CDNotUp, cdNotUp, UI));
        audioDataList.Add(new AudioData(Sfx.ButtonHover, buttonHover, UI));
        audioDataList.Add(new AudioData(Sfx.PlayerDeath, playerDeath, UI));
        audioDataList.Add(new AudioData(Sfx.CDUp, cdUp, UI));
    }
}
