using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003CE RID: 974
	public class KontextMenu : MonoBehaviour
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x0007CDD8 File Offset: 0x0007AFD8
		private Transform ContentRoot
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x0007CDE0 File Offset: 0x0007AFE0
		private PrefabPool<KontextMenuEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<KontextMenuEntry>(this.entryPrefab, this.ContentRoot, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0007CE1E File Offset: 0x0007B01E
		private void Awake()
		{
			if (KontextMenu.instance == null)
			{
				KontextMenu.instance = this;
			}
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0007CE44 File Offset: 0x0007B044
		private void OnDestroy()
		{
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x0007CE48 File Offset: 0x0007B048
		private void Update()
		{
			if (this.watchRectTransform)
			{
				if ((this.cachedTransformPosition - this.watchRectTransform.position).magnitude > this.positionMoveCloseThreshold)
				{
					KontextMenu.Hide(null);
					return;
				}
			}
			else if (this.isWatchingRectTransform)
			{
				KontextMenu.Hide(null);
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x0007CEA0 File Offset: 0x0007B0A0
		public void InstanceShow(object target, RectTransform targetRectTransform, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = targetRectTransform;
			this.isWatchingRectTransform = true;
			this.cachedTransformPosition = this.watchRectTransform.position;
			Vector3[] array = new Vector3[4];
			targetRectTransform.GetWorldCorners(array);
			float num = Mathf.Min(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num2 = Mathf.Max(new float[]
			{
				array[0].x,
				array[1].x,
				array[2].x,
				array[3].x
			});
			float num3 = Mathf.Min(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num4 = Mathf.Max(new float[]
			{
				array[0].y,
				array[1].y,
				array[2].y,
				array[3].y
			});
			float num5 = num;
			float num6 = (float)Screen.width - num2;
			float num7 = num3;
			float num8 = (float)Screen.height - num4;
			float x = (num5 > num6) ? num : num2;
			float y = (num7 > num8) ? num3 : num4;
			Vector2 vector = new Vector2(x, y);
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector2 = new Vector2(vector.x / (float)Screen.width, vector.y / (float)Screen.height);
			float x2 = (float)((vector2.x < 0.5f) ? 0 : 1);
			float y2 = (float)((vector2.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(x2, y2);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = vector;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x0007D0E4 File Offset: 0x0007B2E4
		public void InstanceShow(object target, Vector2 screenPoint, params KontextMenuDataEntry[] entries)
		{
			this.target = target;
			this.watchRectTransform = null;
			this.isWatchingRectTransform = false;
			if (entries.Length < 1)
			{
				this.InstanceHide();
				return;
			}
			Vector2 vector = new Vector2(screenPoint.x / (float)Screen.width, screenPoint.y / (float)Screen.height);
			float x = (float)((vector.x < 0.5f) ? 0 : 1);
			float y = (float)((vector.y < 0.5f) ? 0 : 1);
			this.rectTransform.pivot = new Vector2(x, y);
			base.gameObject.SetActive(true);
			this.fadeGroup.SkipHide();
			this.Setup(entries);
			this.fadeGroup.Show();
			base.transform.position = screenPoint;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0007D1A4 File Offset: 0x0007B3A4
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.ContentRoot.childCount; i++)
			{
				Transform child = this.ContentRoot.GetChild(i);
				if (child.gameObject.activeSelf)
				{
					list.Add(child.gameObject);
				}
			}
			foreach (GameObject obj in list)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0007D23C File Offset: 0x0007B43C
		private void Setup(IEnumerable<KontextMenuDataEntry> entries)
		{
			this.Clear();
			int num = 0;
			foreach (KontextMenuDataEntry kontextMenuDataEntry in entries)
			{
				if (kontextMenuDataEntry != null)
				{
					KontextMenuEntry kontextMenuEntry = this.EntryPool.Get(this.ContentRoot);
					num++;
					kontextMenuEntry.Setup(this, num, kontextMenuDataEntry);
					kontextMenuEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x0007D2B0 File Offset: 0x0007B4B0
		public void InstanceHide()
		{
			this.target = null;
			this.watchRectTransform = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0007D2CB File Offset: 0x0007B4CB
		public static void Show(object target, RectTransform watchRectTransform, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, watchRectTransform, entries);
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0007D2E8 File Offset: 0x0007B4E8
		public static void Show(object target, Vector2 position, params KontextMenuDataEntry[] entries)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			KontextMenu.instance.InstanceShow(target, position, entries);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x0007D305 File Offset: 0x0007B505
		public static void Hide(object target)
		{
			if (KontextMenu.instance == null)
			{
				return;
			}
			if (target != null && target != KontextMenu.instance.target)
			{
				return;
			}
			if (KontextMenu.instance.fadeGroup.IsHidingInProgress)
			{
				return;
			}
			KontextMenu.instance.InstanceHide();
		}

		// Token: 0x04001828 RID: 6184
		private static KontextMenu instance;

		// Token: 0x04001829 RID: 6185
		private RectTransform rectTransform;

		// Token: 0x0400182A RID: 6186
		[SerializeField]
		private KontextMenuEntry entryPrefab;

		// Token: 0x0400182B RID: 6187
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400182C RID: 6188
		[SerializeField]
		private float positionMoveCloseThreshold = 10f;

		// Token: 0x0400182D RID: 6189
		private object target;

		// Token: 0x0400182E RID: 6190
		private bool isWatchingRectTransform;

		// Token: 0x0400182F RID: 6191
		private RectTransform watchRectTransform;

		// Token: 0x04001830 RID: 6192
		private Vector3 cachedTransformPosition;

		// Token: 0x04001831 RID: 6193
		private PrefabPool<KontextMenuEntry> _entryPool;
	}
}
