using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
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
		public bool ActiveObject;
		public bool Action;
		public bool LockLocomotion;
		public float ScrollHotbar;
		public bool OpenInventory;
        public bool ItemShortcut;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = false;
		public bool cursorInputForLook = false;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
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
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnActive(InputValue value)
		{
            ActiveObjectInput(value.isPressed);
		}
        public void OnAction(InputValue value)
        {
            ActionInput(value.isPressed);
        }
        public void OnLockMode(InputValue value)
		{
            LockLocomotionInput(value.isPressed);
		}

		public void OnScrollHotbar(InputValue value)
		{
			ScrollHotbarInput(value.Get<float>());
		}

        public void OnOpenInventory(InputValue value)
        {
            OpenInvenInput(value.isPressed);
        }
        public void OnInventoryShortcut(InputValue value)
		{
			InvenShortInput(value.isPressed);
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

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		public void ActiveObjectInput(bool newActiveObject)
		{
            ActiveObject = newActiveObject;
		}
        public void ActionInput(bool newAction)
        {
            Action = newAction;
        }
        public void LockLocomotionInput(bool newLockLocomotion)
		{
            LockLocomotion = newLockLocomotion;
		}

        public void ScrollHotbarInput(float newScrollHotbar)
        {
            ScrollHotbar = newScrollHotbar;
        }
		public void OpenInvenInput(bool newOpenInvenState)
		{
			OpenInventory = newOpenInvenState;
		}
		private void OnApplicationFocus(bool hasFocus)
        {
            //SetCursorState(cursorLocked);
        }
		public void InvenShortInput(bool invenShortInput)
		{
            ItemShortcut = invenShortInput;
		}
  //      private void SetCursorState(bool newState)
		//{
		//	//Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		//	//Debug.Log(newState);
		//}
	}
}