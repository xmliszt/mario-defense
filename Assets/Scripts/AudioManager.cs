using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource mainCamSource;
    public AudioSource marioSource;
    public AudioSource enemySource;

    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip bounceSound;
    public AudioClip destroySound;

    public AudioClip enemyDieSound;

    public AudioClip theme;
    public AudioClip bossSound;
    public AudioClip winSound;
    public AudioClip gameoverSound;


    public void PlayOnShoot()
    {
        marioSource.PlayOneShot(shootSound);
    }

    public void PlayOnHit()
    {
        marioSource.PlayOneShot(hitSound);
    }

    public void PlayOnBounce()
    {
        marioSource.PlayOneShot(bounceSound);
    }

    public void PlayOnDestroy()
    {
        marioSource.PlayOneShot(destroySound);
    }

    public void PlayOnDie()
    {
        enemySource.PlayOneShot(enemyDieSound);
    }

    public void PlayOnBoss()
    {
        mainCamSource.Pause();
        mainCamSource.clip = bossSound;
        mainCamSource.Play();
    }

    public void PlayWin()
    {
        mainCamSource.Pause();
        mainCamSource.clip = winSound;
        mainCamSource.loop = false;
        mainCamSource.Play();
    }

    public void PlayGameover()
    {
        mainCamSource.Pause();
        mainCamSource.clip = gameoverSound;
        mainCamSource.loop = false;
        mainCamSource.Play();
    }

    public void PlayTheme()
    {
        mainCamSource.Pause();
        mainCamSource.clip = theme;
        mainCamSource.loop = true;
        mainCamSource.Play();
    }
}
