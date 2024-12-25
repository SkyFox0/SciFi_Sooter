using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{

    public class EnimeAnimatorController : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public Animator Animator;
        //public FirstPersonController FirstPersonController;
        //public My_Weapon_Controller My_Weapon_Controller;
        public SoundController SoundController;

        private void Start()
        {
            Animator.SetBool("isMove", true);
            Animator.SetBool("isGround", true);
            //My_Weapon_Controller = GetComponent<My_Weapon_Controller>();
            //FirstPersonController = GetComponent<FirstPersonController>();
            //Animator = GetComponent<Animator>();
        }

        //public void ActivateMoveAnimation()
        //{
        //    Animator.SetBool("isMove", true);
        //}


        //public void DeactivateMoveAnimation()
        //{
        //    Animator.SetBool("isMove", false);
        //}


        public void JumpBegin(string s)
        {
            Debug.Log("���������� � ������ ���������!");
            Animator.SetBool("isJump", true);
            Animator.SetBool("isGround", false);            
            //FirstPersonController.Jump();
            SoundController.JumpSound();
        }
        public void JumpEnd(string s)
        {
            Debug.Log("������ ��������!");
            Animator.SetBool("isJump", false);
            Animator.SetBool("isGround", true);
            SoundController.LandingSound();
        }

        public void StartReload(string s)
        {
            Animator.SetBool("isReloading", true);

            Debug.Log("����������� ������!");
        }

        public void EndReload(string s)
        {
           // My_Weapon_Controller.EndReload();
            Animator.SetBool("isReloading", false);
            Animator.SetBool("isNeedToReload", false);
            //Animator.SetBool("isMove", true);
            Debug.Log("����������� ���������!");
        }        

        public void DamageBegin(string s)
        {            
            Animator.SetBool("isDamage", true);
            Debug.Log("������ �����");
        }

        public void DamageEnd(string s)
        {
            Animator.SetBool("isDamage", false);
            Debug.Log("����� �����");
        }

        public void Step_1(string s)
        {
            SoundController.StepSound_1();
        }

        public void Step_2(string s)
        {
            SoundController.StepSound_2();
        }
    }
}
