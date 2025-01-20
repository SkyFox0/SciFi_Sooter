using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool shoot;
		public bool sight;
        public bool reload;
        public bool reloadHold;
        public bool grenade;
        public bool light;
		public bool activate;


        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM


    public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (!jump)
			{
				//Debug.Log("Ïðûæîê!");
				jump = true;
				Invoke("JumpOff", 0.2f);
			}
				//JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnSight(InputValue value)
        {
            SightInput(value.isPressed);
        }

        public void OnSightOff(InputValue value)
        {
            SightOffInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
			if (!reload)
			{
				reload = true;
				Invoke("ReloadOff", 0.2f);
			}			    
        }
        public void OnReloadHold(InputValue value)
        {
            if (!reloadHold)
            {
                reloadHold = true;
                Invoke("ReloadHoldOff", 0.4f);
            }
        }

        public void OnGrenade(InputValue value)
        {
			if (!grenade)
			{
                grenade = true;
				Invoke("GrenadeOff", 0.2f);
			}
        }

		public void OnLight(InputValue value)
		{
			if (!light)
			{
				light = true;
			}
			else
			{
				light = false;
			}
		}
		public void OnActivate(InputValue value)
		{
			if (!activate)
			{				
				activate = true;
				Invoke("ActivateOff", 0.2f);
			}
			/*else
			{
                Activate = false;
            }*/
		}



#endif

    public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
        {
			look = newLookDirection;
		}
        public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;

        }

        public void JumpOff()
		{
			jump = false;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        public void SightInput(bool newSightState)
        {
            sight = true;
        }
        public void SightOffInput(bool newSightState)
        {
            sight = false;
        }

        //public void ReloadInput(bool newReloadState)
        //{
        //    reload = newReloadState;
        //}

        //public void ReloadStart()
		//{
		//	reload = true;
        //}

        public void ReloadOff()
        {
            reload = false;
        }

        public void ReloadHoldOff()
        {
            reloadHold = false;
        }

        public void GrenadeOff()
        {
            grenade = false;
        }

        public void ActivateOff()
        {
            activate = false;
        }
        




        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}