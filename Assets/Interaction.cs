using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace StarterAssets
{

    public class Interaction : MonoBehaviour
    {
        public Camera _camera;
        public Animator Animator;
        [SerializeField] private Button _button;
        public TVButtonClick TVButtonClick;
        public DoorButtonClick DoorButtonClick;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartCoroutine(FindButton());
        }

        IEnumerator FindButton()
        {
            while (true)
            {
                if (!Animator.GetBool("isArmed"))
                {
                    ShootRaycast();
                }
                yield return new WaitForSeconds(0.3f);  // счетчик таймера поиска кнопки
                                                        //Debug.Log(Time.time.ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 shootPosition = _camera.transform.position;
            //var direction = _camera.transform.forward;

            //Debug.DrawRay(shootPosition, direction * 2f, Color.green);
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

                if (hitInfo.collider.tag == "TVButton")
                {
                    //Debug.Log(" нопка : " + hitInfo.collider.name);
                    TVButtonClick = hitInfo.collider.GetComponent<TVButtonClick>();
                    TVButtonClick.CheckButtonEnabled();
                }

                if (hitInfo.collider.tag == "DoorButton")
                {
                    //Debug.Log(" нопка : " + hitInfo.collider.name);
                    DoorButtonClick = hitInfo.collider.GetComponent<DoorButtonClick>();
                    DoorButtonClick.CheckButtonEnabled();
                }
            }
        }

        public void OnActivate(InputValue value)
        {
            Debug.Log("кнопка активации нажата");
            Vector3 shootPosition = _camera.transform.position;
            var direction = _camera.transform.forward;

            //Debug.DrawRay(shootPosition, direction * 2f, Color.green);
            if (Physics.Raycast(shootPosition, direction, out var hitInfo, 2f))   //, layerMask, QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawRay(SearchPoint, _targetDirection * _fireDistans, Color.red);
                //Debug.Log("Hit! Object = " + hitInfo.collider.name);

                if (hitInfo.collider.tag == "TVButton")
                {
                    //Debug.Log(" нопка : " + hitInfo.collider.name);
                    TVButtonClick = hitInfo.collider.GetComponent<TVButtonClick>();
                    //Debug.Log("кнопка активации нажата2");
                    TVButtonClick.Click();
                }

                if (hitInfo.collider.tag == "DoorButton")
                {
                    Debug.Log("кнопка пульта двери нажата");
                    //Debug.Log(" нопка : " + hitInfo.collider.name);
                    DoorButtonClick = hitInfo.collider.GetComponent<DoorButtonClick>();
                    //Debug.Log("кнопка активации нажата2");
                    DoorButtonClick.Click();
                }
            }
        }
    }
}
