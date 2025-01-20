using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.Generic;


public class Interaction : MonoBehaviour
{
    public Camera _camera;
    public Button _button;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TestCoroutine());
    }

    IEnumerator TestCoroutine()
    {
        while (true)
        {
            ShootRaycast();
            yield return new WaitForSeconds(1f);
            //Debug.Log(Time.time.ToString());
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shootPosition = _camera.transform.position;
        var direction = _camera.transform.forward;
        
        Debug.DrawRay(shootPosition, direction * 2f, Color.green);

    }

    

    public void ShootRaycast()
    {
        Vector3 shootPosition = _camera.transform.position;
        var direction = _camera.transform.forward;
        
        //Debug.DrawRay(shootPosition, direction * 2f, Color.green);
        if (Physics.Raycast(shootPosition, direction, out var hitInfo, 2f))   //, layerMask, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
            //Debug.Log("Hit! Object = " + hitInfo.collider.name);

            if (hitInfo.collider.tag == "Button")
            {
                Debug.Log("Кнопка : " + hitInfo.collider.name);

                _button = hitInfo.collider.GetComponent<Button>();
                Debug.Log(_button.name);
                _button.onClick.Invoke();

            }
        }
    }
}
