using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  public float speed = 40;
  public GameObject bullet;
  public Transform barrel;
  private AudioSource audioSource;
  public AudioClip audioClip;

  private void Start()
  {
      audioSource = GetComponent<AudioSource>();
  }

  public void Fire()
  {
      audioSource.PlayOneShot(audioClip);
      Invoke("Shoot",.1f);
  }

  private void Shoot()
  {
      GameObject spawendBullet = Instantiate(bullet, barrel.position, barrel.rotation);
      spawendBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;
      Destroy(spawendBullet, 2);
  }
}
