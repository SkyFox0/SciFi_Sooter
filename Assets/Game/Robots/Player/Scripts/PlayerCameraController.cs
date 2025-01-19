using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
//using Unity.Mathematics;

namespace StarterAssets
{  

    public class PlayerCameraController : MonoBehaviour
    {
        public Animator Animator;
        public FirstPersonController FirstPersonController;
        public My_Weapon_Controller My_Weapon_Controller;
        public Ui_Control Ui_Control;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        private Transform CameraTargetPosition;
        public Transform IdleCameraPoint;
        public Transform GuardIdleCameraPoint;
        public Transform GuardMoveCameraPoint;
        public Transform GuardRunCameraPoint;

        public Transform SightPoint;
        public Transform SightPointShoot;
        public CinemachineVirtualCamera Camera;
        public float _sigthFocus = 30;
        public float _normalFocus = 65;

        [Header("UI")]
        public Image _imageX;
        public Image _image0;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            CameraTargetPosition = IdleCameraPoint;
        }

        // Update is called once per frame
        void Update()
        {
            if (CinemachineCameraTarget.transform.position != CameraTargetPosition.position) 
            {
                //CinemachineCameraTarget.transform.position = CameraTargetPosition.position;
                //CinemachineCameraTarget.transform.position = math.lerp(CinemachineCameraTarget.transform.position, CameraTargetPosition.position, 1f * Time.deltaTime);
                CinemachineCameraTarget.transform.position = Vector3.Lerp(CinemachineCameraTarget.transform.position, CameraTargetPosition.position, 1f * Time.deltaTime);
            }
        }
    }
}
