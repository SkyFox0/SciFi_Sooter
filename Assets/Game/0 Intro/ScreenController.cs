
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
   

    void Start()
    {
        StartCoroutine(StartVideo());        
    } 

    IEnumerator StartVideo()
    {
        yield return new WaitForSeconds(248.0f);
        //Application.LoadLevel("Playground");  // Устарело
        SceneManager.LoadScene("Playground");      
    }  
    
    public void OnShoot(InputValue value)
    {
        //ShootInput(value.isPressed);
        SceneManager.LoadScene("Playground");
    }

    public void OnAnyClick(InputValue value)
    {
        //ShootInput(value.isPressed);
        SceneManager.LoadScene("Playground");
    }

    //public void OnExit(InputValue value)
    /*{
        SceneManager.LoadScene("Playground");
        Debug.Log("Exit");
        Application.Quit();    // закрыть приложение
    }*/


}
