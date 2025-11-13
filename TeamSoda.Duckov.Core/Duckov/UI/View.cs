using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003C8 RID: 968
	public abstract class View : ManagedUIElement
	{
		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x0007C095 File Offset: 0x0007A295
		// (set) Token: 0x06002351 RID: 9041 RVA: 0x0007C09C File Offset: 0x0007A29C
		public static View ActiveView
		{
			get
			{
				return View._activeView;
			}
			private set
			{
				UnityEngine.Object activeView = View._activeView;
				View._activeView = value;
				if (activeView != View._activeView)
				{
					Action onActiveViewChanged = View.OnActiveViewChanged;
					if (onActiveViewChanged == null)
					{
						return;
					}
					onActiveViewChanged();
				}
			}
		}

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x06002352 RID: 9042 RVA: 0x0007C0C4 File Offset: 0x0007A2C4
		// (remove) Token: 0x06002353 RID: 9043 RVA: 0x0007C0F8 File Offset: 0x0007A2F8
		public static event Action OnActiveViewChanged;

		// Token: 0x06002354 RID: 9044 RVA: 0x0007C12C File Offset: 0x0007A32C
		protected override void Awake()
		{
			base.Awake();
			if (this.exitButton != null)
			{
				this.exitButton.onClick.AddListener(new UnityAction(base.Close));
			}
			UIInputManager.OnNavigate += this.OnNavigate;
			UIInputManager.OnConfirm += this.OnConfirm;
			UIInputManager.OnCancel += this.OnCancel;
			this.viewTabs = base.transform.parent.parent.GetComponent<ViewTabs>();
			if (this.autoClose)
			{
				base.Close();
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0007C1C5 File Offset: 0x0007A3C5
		protected override void OnDestroy()
		{
			base.OnDestroy();
			UIInputManager.OnNavigate -= this.OnNavigate;
			UIInputManager.OnConfirm -= this.OnConfirm;
			UIInputManager.OnCancel -= this.OnCancel;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0007C200 File Offset: 0x0007A400
		protected override void OnOpen()
		{
			this.autoClose = false;
			if (View.ActiveView != null && View.ActiveView != this)
			{
				View.ActiveView.Close();
			}
			View.ActiveView = this;
			ItemUIUtilities.Select(null);
			if (this.viewTabs != null)
			{
				this.viewTabs.Show();
			}
			if (base.gameObject == null)
			{
				Debug.LogError("GameObject不存在", base.gameObject);
			}
			InputManager.DisableInput(base.gameObject);
			AudioManager.Post(this.sfx_Open);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0007C292 File Offset: 0x0007A492
		protected override void OnClose()
		{
			if (View.ActiveView == this)
			{
				View.ActiveView = null;
			}
			InputManager.ActiveInput(base.gameObject);
			AudioManager.Post(this.sfx_Close);
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0007C2BE File Offset: 0x0007A4BE
		internal virtual void TryQuit()
		{
			base.Close();
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0007C2C6 File Offset: 0x0007A4C6
		public void OnNavigate(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnNavigate(eventData.vector);
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0007C2EB File Offset: 0x0007A4EB
		public void OnConfirm(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView != this)
			{
				return;
			}
			this.OnConfirm();
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0007C30A File Offset: 0x0007A50A
		public void OnCancel(UIInputEventData eventData)
		{
			if (eventData.Used)
			{
				return;
			}
			if (View.ActiveView == null || View.ActiveView != this)
			{
				return;
			}
			this.OnCancel();
			if (!eventData.Used)
			{
				this.TryQuit();
				eventData.Use();
			}
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0007C34A File Offset: 0x0007A54A
		protected virtual void OnNavigate(Vector2 vector)
		{
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0007C34C File Offset: 0x0007A54C
		protected virtual void OnConfirm()
		{
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0007C34E File Offset: 0x0007A54E
		protected virtual void OnCancel()
		{
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007C350 File Offset: 0x0007A550
		protected static T GetViewInstance<T>() where T : View
		{
			return GameplayUIManager.GetViewInstance<T>();
		}

		// Token: 0x040017FB RID: 6139
		[HideInInspector]
		private static View _activeView;

		// Token: 0x040017FD RID: 6141
		[SerializeField]
		private ViewTabs viewTabs;

		// Token: 0x040017FE RID: 6142
		[SerializeField]
		private Button exitButton;

		// Token: 0x040017FF RID: 6143
		[SerializeField]
		private string sfx_Open;

		// Token: 0x04001800 RID: 6144
		[SerializeField]
		private string sfx_Close;

		// Token: 0x04001801 RID: 6145
		private bool autoClose = true;
	}
}
