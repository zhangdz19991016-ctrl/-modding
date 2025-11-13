using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003B0 RID: 944
	public class GameplayUIManager : MonoBehaviour
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x00076BAB File Offset: 0x00074DAB
		public static GameplayUIManager Instance
		{
			get
			{
				return GameplayUIManager.instance;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x00076BB2 File Offset: 0x00074DB2
		public View ActiveView
		{
			get
			{
				return View.ActiveView;
			}
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00076BBC File Offset: 0x00074DBC
		public static T GetViewInstance<T>() where T : View
		{
			if (GameplayUIManager.Instance == null)
			{
				return default(T);
			}
			View view;
			if (GameplayUIManager.Instance.viewDic.TryGetValue(typeof(T), out view))
			{
				return view as T;
			}
			View view2 = GameplayUIManager.Instance.views.Find((View e) => e is T);
			if (view2 == null)
			{
				return default(T);
			}
			GameplayUIManager.Instance.viewDic[typeof(T)] = view2;
			return view2 as T;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x00076C70 File Offset: 0x00074E70
		private void Awake()
		{
			if (GameplayUIManager.instance == null)
			{
				GameplayUIManager.instance = this;
			}
			else
			{
				Debug.LogWarning("Duplicate Gameplay UI Manager detected!");
			}
			foreach (View view in this.views)
			{
				view.gameObject.SetActive(true);
			}
			foreach (GameObject gameObject in this.setActiveOnAwake)
			{
				if (!(gameObject == null))
				{
					gameObject.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x060021EB RID: 8683 RVA: 0x00076D38 File Offset: 0x00074F38
		public PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this.itemDisplayPool == null)
				{
					this.itemDisplayPool = new PrefabPool<ItemDisplay>(GameplayDataSettings.UIPrefabs.ItemDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.itemDisplayPool;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x00076D7C File Offset: 0x00074F7C
		public PrefabPool<SlotDisplay> SlotDisplayPool
		{
			get
			{
				if (this.slotDisplayPool == null)
				{
					this.slotDisplayPool = new PrefabPool<SlotDisplay>(GameplayDataSettings.UIPrefabs.SlotDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.slotDisplayPool;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x00076DC0 File Offset: 0x00074FC0
		public PrefabPool<InventoryEntry> InventoryEntryPool
		{
			get
			{
				if (this.inventoryEntryPool == null)
				{
					this.inventoryEntryPool = new PrefabPool<InventoryEntry>(GameplayDataSettings.UIPrefabs.InventoryEntry, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.inventoryEntryPool;
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x00076E02 File Offset: 0x00075002
		public SplitDialogue SplitDialogue
		{
			get
			{
				return this._splitDialogue;
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x00076E0A File Offset: 0x0007500A
		public static UniTask TemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = false;
			return GameplayUIManager.Instance.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00076E3E File Offset: 0x0007503E
		public static UniTask ReverseTemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = true;
			return GameplayUIManager.Instance.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x040016EC RID: 5868
		private static GameplayUIManager instance;

		// Token: 0x040016ED RID: 5869
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040016EE RID: 5870
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016EF RID: 5871
		[SerializeField]
		private List<View> views = new List<View>();

		// Token: 0x040016F0 RID: 5872
		[SerializeField]
		private List<GameObject> setActiveOnAwake;

		// Token: 0x040016F1 RID: 5873
		private Dictionary<Type, View> viewDic = new Dictionary<Type, View>();

		// Token: 0x040016F2 RID: 5874
		private PrefabPool<ItemDisplay> itemDisplayPool;

		// Token: 0x040016F3 RID: 5875
		private PrefabPool<SlotDisplay> slotDisplayPool;

		// Token: 0x040016F4 RID: 5876
		private PrefabPool<InventoryEntry> inventoryEntryPool;

		// Token: 0x040016F5 RID: 5877
		[SerializeField]
		private SplitDialogue _splitDialogue;
	}
}
