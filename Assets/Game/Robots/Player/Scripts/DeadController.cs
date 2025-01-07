using StarterAssets;
using UnityEngine;
using UnityEngine.UIElements;

public class DeadController : MonoBehaviour
{
    public bool _isDead;
    public Camera MainCamera;
    public Camera DeadCamera;
    public Animator DeadCameraAnimator;
    public Canvas ImageX;
    public Canvas Dead;
    public Canvas Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayerIsDead()
    {
        _isDead = true;
        DeadCamera.enabled = true;
        MainCamera.enabled = false;        
        DeadCameraAnimator.SetBool("isDead", true);
        ImageX.enabled = false;
        Dead.enabled = true;
        Score.worldCamera = DeadCamera;        
    }
    private void Update()
    {
        if (_isDead)
        {
            if (DeadCamera.fieldOfView > 40f)
            {
                DeadCamera.fieldOfView -= Time.deltaTime * 5f;
            }
        }
    }
}
