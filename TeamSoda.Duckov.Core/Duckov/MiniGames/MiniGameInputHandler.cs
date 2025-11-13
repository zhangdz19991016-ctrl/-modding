using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Duckov.MiniGames
{
	// Token: 0x02000289 RID: 649
	public class MiniGameInputHandler : MonoBehaviour
	{
		// Token: 0x060014E2 RID: 5346 RVA: 0x0004DAE8 File Offset: 0x0004BCE8
		private void Awake()
		{
			InputActionAsset actions = GameManager.MainPlayerInput.actions;
			this.inputActionMove = actions["MoveAxis"];
			this.inputActionButtonA = actions["MiniGameA"];
			this.inputActionButtonB = actions["MiniGameB"];
			this.inputActionSelect = actions["MiniGameSelect"];
			this.inputActionStart = actions["MiniGameStart"];
			this.inputActionMouseDelta = actions["MouseDelta"];
			this.inputActionButtonA.actionMap.Enable();
			this.Bind(this.inputActionMove, new Action<InputAction.CallbackContext>(this.OnMove));
			this.Bind(this.inputActionButtonA, new Action<InputAction.CallbackContext>(this.OnButtonA));
			this.Bind(this.inputActionButtonB, new Action<InputAction.CallbackContext>(this.OnButtonB));
			this.Bind(this.inputActionSelect, new Action<InputAction.CallbackContext>(this.OnSelect));
			this.Bind(this.inputActionStart, new Action<InputAction.CallbackContext>(this.OnStart));
			this.Bind(this.inputActionMouseDelta, new Action<InputAction.CallbackContext>(this.OnMouseDelta));
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x0004DC06 File Offset: 0x0004BE06
		private void OnMouseDelta(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 1);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0004DC33 File Offset: 0x0004BE33
		public void ClearInput()
		{
			MiniGame miniGame = this.game;
			if (miniGame == null)
			{
				return;
			}
			miniGame.ClearInput();
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0004DC45 File Offset: 0x0004BE45
		private void OnDisable()
		{
			this.ClearInput();
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x0004DC4D File Offset: 0x0004BE4D
		private void SetGameButtonByContext(MiniGame.Button button, InputAction.CallbackContext context)
		{
			if (context.started)
			{
				this.game.SetButton(button, true);
				return;
			}
			if (context.canceled)
			{
				this.game.SetButton(button, false);
			}
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0004DC7C File Offset: 0x0004BE7C
		private void OnStart(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Start, context);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0004DC9E File Offset: 0x0004BE9E
		private void OnSelect(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.Select, context);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0004DCC0 File Offset: 0x0004BEC0
		private void OnButtonB(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.B, context);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0004DCE2 File Offset: 0x0004BEE2
		private void OnButtonA(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.SetGameButtonByContext(MiniGame.Button.A, context);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0004DD04 File Offset: 0x0004BF04
		private void OnMove(InputAction.CallbackContext context)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.game == null)
			{
				return;
			}
			this.game.SetInputAxis(context.ReadValue<Vector2>(), 0);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0004DD34 File Offset: 0x0004BF34
		private void OnDestroy()
		{
			foreach (Action action in this.unbindCommands)
			{
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0004DD8C File Offset: 0x0004BF8C
		private void Bind(InputAction inputAction, Action<InputAction.CallbackContext> action)
		{
			inputAction.Enable();
			inputAction.started += action;
			inputAction.performed += action;
			inputAction.canceled += action;
			this.unbindCommands.Add(delegate
			{
				inputAction.started -= action;
				inputAction.performed -= action;
				inputAction.canceled -= action;
			});
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0004DE02 File Offset: 0x0004C002
		internal void SetGame(MiniGame game)
		{
			this.game = game;
		}

		// Token: 0x04000F3F RID: 3903
		[SerializeField]
		private MiniGame game;

		// Token: 0x04000F40 RID: 3904
		private InputAction inputActionMove;

		// Token: 0x04000F41 RID: 3905
		private InputAction inputActionButtonA;

		// Token: 0x04000F42 RID: 3906
		private InputAction inputActionButtonB;

		// Token: 0x04000F43 RID: 3907
		private InputAction inputActionSelect;

		// Token: 0x04000F44 RID: 3908
		private InputAction inputActionStart;

		// Token: 0x04000F45 RID: 3909
		private InputAction inputActionMouseDelta;

		// Token: 0x04000F46 RID: 3910
		private List<Action> unbindCommands = new List<Action>();
	}
}
