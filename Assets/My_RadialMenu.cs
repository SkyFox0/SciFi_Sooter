using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace StarterAssets
{

    public class My_RadialMenu : MonoBehaviour
    {
        //private Vector2 _menuCenter;
        public bool _radialMenuIsOpened;
        public Canvas RadialMenu;
        public Canvas X_Point;
        public StarterAssetsInputs MouseInput;
        public float MousePosition;
        public GameObject Cursor_0;
        public GameObject Cursor_1;
        public AudioSource SoundSelect;

        public enum Weapons
        {
            Rifle = 0,
            PlasmaGun = 1
        }
        public Weapons SelectedWeapons;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //_menuCenter = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
            SelectedWeapons = Weapons.Rifle;
            _radialMenuIsOpened = false;
            RadialMenu.enabled = false;
            StartCoroutine(FindMouse());
            if (SelectedWeapons == Weapons.Rifle)
            {
                Cursor_0.SetActive(true);
                Cursor_1.SetActive(false);
            }
            else
            {
                Cursor_0.SetActive(false);
                Cursor_1.SetActive(true);
            }
        }

        IEnumerator FindMouse() // проверяет где находится мышь
        {
            while (true)
            {
                if (_radialMenuIsOpened)
                {
                    //float mouseDistanceFromCenter = Vector2.Distance(_menuCenter, Input.mousePosition);
                    //float mouseDistanceFromCenter = _menuCenter.x - Input.mousePosition.x;
                    //Debug.Log("Вектор мыши относительно центра экрана = " + mouseDistanceFromCenter.ToString());

                    //Debug.Log("Вектор мыши относительно центра экрана = " + MousePosition.ToString()); 
                    
                }
                
                //float mouseDistanceFromCenter = Vector2.Distance(_menuCenter, Input.mousePosition);
                //Debug.Log("Вектор мыши относительно центра экрана = " + mouseDistanceFromCenter.ToString());

                if (SelectedWeapons == Weapons.Rifle && MousePosition > 1f)
                {
                    SelectedWeapons = Weapons.PlasmaGun;
                    SoundSelect.Play();
                }
                else
                {
                    if (SelectedWeapons == Weapons.PlasmaGun && MousePosition < -1f)
                    {
                        SelectedWeapons = Weapons.Rifle;
                        SoundSelect.Play();
                    }
                }
                if (SelectedWeapons == Weapons.Rifle)
                {
                    Cursor_0.SetActive(true);
                    Cursor_1.SetActive(false);
                }
                else
                {
                    Cursor_0.SetActive(false);
                    Cursor_1.SetActive(true);
                }


                yield return new WaitForSeconds(0.25f);
            }
        }



        // Update is called once per frame
        void Update()
        {
            if (_radialMenuIsOpened)
            {
                if (MouseInput.look.x > 0f && MousePosition < 3f)
                {
                    MousePosition = MousePosition + MouseInput.look.x * Time.deltaTime * 20;
                }
                else
                {
                    if (MouseInput.look.x < 0f && MousePosition > -3f)
                    {
                        MousePosition = MousePosition + MouseInput.look.x * Time.deltaTime * 20;
                    }                    
                }                
            }            
        }

        public void Open()
        {
            MousePosition = 0f;
            //_menuCenter = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
            _radialMenuIsOpened = true;
            RadialMenu.enabled = true;
            X_Point.enabled = false;
            //Включить и спрятать курсор
            //Cursor.lockState = CursorLockMode.Locked;

            //Cursor.visible = true;
            //transform.localScale = (OpenAnimation == AnimationType.zoomIn) ? Vector3.zero : Vector3.one * 10;
            if (SelectedWeapons == Weapons.Rifle)
            {
                Cursor_0.SetActive(true);
                Cursor_1.SetActive(false);
            }
            else
            {
                Cursor_0.SetActive(false);
                Cursor_1.SetActive(true);
            }
        }

        public void Close()
        {
            _radialMenuIsOpened = false;
            RadialMenu.enabled = false;
            X_Point.enabled = true;
            //Отключить и спрятать курсор
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        
    }
}
