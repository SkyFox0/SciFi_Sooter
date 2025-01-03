using UnityEngine;

public class ImpactSound : MonoBehaviour
{
    public AudioSource Down;

  
  public void OnTriggerEnter(Collider other)
    {
        Down.Play();
    }
}
