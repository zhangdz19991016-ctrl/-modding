using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using NodeCanvas.Framework;
using TMPro;
using UI_Spline_Renderer;
using UnityEngine;
using UnityEngine.Splines;

namespace Duckov.UI
{
	// Token: 0x020003C5 RID: 965
	public class PerkTreeView : View, ISingleSelectionMenu<PerkEntry>
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x0007B384 File Offset: 0x00079584
		public static PerkTreeView Instance
		{
			get
			{
				return View.GetViewInstance<PerkTreeView>();
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x0007B38C File Offset: 0x0007958C
		private PrefabPool<PerkEntry> PerkEntryPool
		{
			get
			{
				if (this._perkEntryPool == null)
				{
					this._perkEntryPool = new PrefabPool<PerkEntry>(this.perkEntryPrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkEntryPool;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0007B3CC File Offset: 0x000795CC
		private PrefabPool<PerkLineEntry> PerkLinePool
		{
			get
			{
				if (this._perkLinePool == null)
				{
					this._perkLinePool = new PrefabPool<PerkLineEntry>(this.perkLinePrefab, this.contentParent, null, null, null, true, 10, 10000, null);
				}
				return this._perkLinePool;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x0007B40A File Offset: 0x0007960A
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06002329 RID: 9001 RVA: 0x0007B410 File Offset: 0x00079610
		// (remove) Token: 0x0600232A RID: 9002 RVA: 0x0007B448 File Offset: 0x00079648
		internal event Action<PerkEntry> onSelectionChanged;

		// Token: 0x0600232B RID: 9003 RVA: 0x0007B480 File Offset: 0x00079680
		private void PopulatePerks()
		{
			this.contentParent.ForceUpdateRectTransforms();
			this.PerkEntryPool.ReleaseAll();
			this.PerkLinePool.ReleaseAll();
			bool isDemo = GameMetaData.Instance.IsDemo;
			foreach (Perk perk in this.target.Perks)
			{
				if ((!isDemo || !perk.LockInDemo) && this.target.RelationGraphOwner.GetRelatedNode(perk) != null)
				{
					this.PerkEntryPool.Get(this.contentParent).Setup(this, perk);
				}
			}
			foreach (PerkLevelLineNode cur in this.target.RelationGraphOwner.graph.GetAllNodesOfType<PerkLevelLineNode>())
			{
				this.PerkLinePool.Get(this.contentParent).Setup(this, cur);
			}
			this.FitChildren();
			this.RefreshConnections();
		}

		// Token: 0x0600232C RID: 9004 RVA: 0x0007B598 File Offset: 0x00079798
		private void RefreshConnections()
		{
			bool isDemo = GameMetaData.Instance.IsDemo;
			this.activeConnectionsRenderer.enabled = false;
			this.inactiveConnectionsRenderer.enabled = false;
			SplineContainer splineContainer = this.activeConnectionsRenderer.splineContainer;
			SplineContainer splineContainer2 = this.inactiveConnectionsRenderer.splineContainer;
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer);
			PerkTreeView.<RefreshConnections>g__ClearSplines|27_0(splineContainer2);
			PerkTreeView.<>c__DisplayClass27_0 CS$<>8__locals1;
			CS$<>8__locals1.horizontal = this.target.Horizontal;
			CS$<>8__locals1.splineTangentVector = (CS$<>8__locals1.horizontal ? Vector3.left : Vector3.up) * this.splineTangent;
			foreach (Perk perk in this.target.Perks)
			{
				if (!isDemo || !perk.LockInDemo)
				{
					PerkRelationNode relatedNode = this.target.RelationGraphOwner.GetRelatedNode(perk);
					PerkEntry perkEntry = this.GetPerkEntry(perk);
					if (!(perkEntry == null) && relatedNode != null)
					{
						SplineContainer container = perk.Unlocked ? splineContainer : splineContainer2;
						foreach (Connection connection in relatedNode.outConnections)
						{
							PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
							Perk relatedNode2 = perkRelationNode.relatedNode;
							if (relatedNode2 == null)
							{
								Debug.Log(string.Concat(new string[]
								{
									"Target Perk is Null (Connection from ",
									relatedNode.name,
									" to ",
									perkRelationNode.name,
									")"
								}));
							}
							else if (!isDemo || !relatedNode2.LockInDemo)
							{
								PerkEntry perkEntry2 = this.GetPerkEntry(relatedNode2);
								if (perkEntry2 == null)
								{
									Debug.Log(string.Concat(new string[]
									{
										"Target Perk Entry is Null (Connection from ",
										relatedNode.name,
										" to ",
										perkRelationNode.name,
										")"
									}));
								}
								else
								{
									PerkTreeView.<RefreshConnections>g__AddConnection|27_1(container, perkEntry.transform.localPosition, perkEntry2.transform.localPosition, ref CS$<>8__locals1);
								}
							}
						}
					}
				}
			}
			this.activeConnectionsRenderer.enabled = true;
			this.inactiveConnectionsRenderer.enabled = true;
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x0007B81C File Offset: 0x00079A1C
		private PerkEntry GetPerkEntry(Perk ofPerk)
		{
			return this.PerkEntryPool.ActiveEntries.FirstOrDefault((PerkEntry e) => e != null && e.Target == ofPerk);
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x0007B854 File Offset: 0x00079A54
		private void FitChildren()
		{
			this.contentParent.ForceUpdateRectTransforms();
			ReadOnlyCollection<PerkEntry> activeEntries = this.PerkEntryPool.ActiveEntries;
			float num2;
			float num = num2 = float.MaxValue;
			float num4;
			float num3 = num4 = float.MinValue;
			foreach (PerkEntry perkEntry in activeEntries)
			{
				RectTransform rectTransform = perkEntry.RectTransform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.zero;
				Vector2 layoutPosition = perkEntry.GetLayoutPosition();
				layoutPosition.y *= -1f;
				Vector2 vector = layoutPosition * this.layoutFactor;
				rectTransform.anchoredPosition = vector;
				if (vector.x < num2)
				{
					num2 = vector.x;
				}
				if (vector.y < num)
				{
					num = vector.y;
				}
				if (vector.x > num4)
				{
					num4 = vector.x;
				}
				if (vector.y > num3)
				{
					num3 = vector.y;
				}
			}
			float num5 = num4 - num2;
			float num6 = num3 - num;
			Vector2 b = -new Vector2(num2, num);
			RectTransform rectTransform2 = this.contentParent;
			Vector2 sizeDelta = rectTransform2.sizeDelta;
			sizeDelta.y = num6 + this.padding.y * 2f;
			rectTransform2.sizeDelta = sizeDelta;
			foreach (PerkEntry perkEntry2 in activeEntries)
			{
				RectTransform rectTransform3 = perkEntry2.RectTransform;
				Vector2 vector2 = rectTransform3.anchoredPosition + b;
				if (num5 == 0f)
				{
					vector2.x = (rectTransform2.rect.width - this.padding.x * 2f) / 2f;
				}
				else
				{
					float num7 = (rectTransform2.rect.width - this.padding.x * 2f) / num5;
					vector2.x *= num7;
				}
				vector2 += this.padding;
				rectTransform3.anchoredPosition = vector2;
			}
			foreach (PerkLineEntry perkLineEntry in this.PerkLinePool.ActiveEntries)
			{
				RectTransform rectTransform4 = perkLineEntry.RectTransform;
				Vector2 layoutPosition2 = perkLineEntry.GetLayoutPosition();
				layoutPosition2.y *= -1f;
				Vector2 vector3 = layoutPosition2 * this.layoutFactor;
				vector3 += this.padding;
				vector3.x = rectTransform4.anchoredPosition.x;
				rectTransform4.anchoredPosition = vector3;
				rectTransform4.SetAsFirstSibling();
			}
			this.contentParent.anchoredPosition = Vector2.zero;
		}

		// Token: 0x0600232F RID: 9007 RVA: 0x0007BB34 File Offset: 0x00079D34
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0007BB47 File Offset: 0x00079D47
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0007BB5A File Offset: 0x00079D5A
		public PerkEntry GetSelection()
		{
			return this.selectedPerkEntry;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0007BB62 File Offset: 0x00079D62
		public bool SetSelection(PerkEntry selection)
		{
			this.selectedPerkEntry = selection;
			this.OnSelectionChanged();
			return true;
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x0007BB72 File Offset: 0x00079D72
		private void OnSelectionChanged()
		{
			Action<PerkEntry> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this.selectedPerkEntry);
			}
			this.RefreshDetails();
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0007BB91 File Offset: 0x00079D91
		private void RefreshDetails()
		{
			PerkDetails perkDetails = this.details;
			PerkEntry perkEntry = this.selectedPerkEntry;
			perkDetails.Setup((perkEntry != null) ? perkEntry.Target : null, true);
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x0007BBB1 File Offset: 0x00079DB1
		private void Show_Local(PerkTree target)
		{
			this.UnregisterEvents();
			this.SetSelection(null);
			this.target = target;
			this.title.text = target.DisplayName;
			this.ShowTask().Forget();
			this.RegisterEvents();
		}

		// Token: 0x06002336 RID: 9014 RVA: 0x0007BBEA File Offset: 0x00079DEA
		public static void Show(PerkTree target)
		{
			if (PerkTreeView.Instance == null)
			{
				return;
			}
			PerkTreeView.Instance.Show_Local(target);
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0007BC05 File Offset: 0x00079E05
		private void RegisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged += this.Refresh;
			}
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0007BC2C File Offset: 0x00079E2C
		private void UnregisterEvents()
		{
			if (this.target != null)
			{
				this.target.onPerkTreeStatusChanged -= this.Refresh;
			}
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0007BC53 File Offset: 0x00079E53
		private void Refresh(PerkTree tree)
		{
			this.RefreshConnections();
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0007BC5C File Offset: 0x00079E5C
		private UniTask ShowTask()
		{
			PerkTreeView.<ShowTask>d__41 <ShowTask>d__;
			<ShowTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowTask>d__.<>4__this = this;
			<ShowTask>d__.<>1__state = -1;
			<ShowTask>d__.<>t__builder.Start<PerkTreeView.<ShowTask>d__41>(ref <ShowTask>d__);
			return <ShowTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0007BC9F File Offset: 0x00079E9F
		public void Hide()
		{
			base.Close();
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0007BCA7 File Offset: 0x00079EA7
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0007BCD8 File Offset: 0x00079ED8
		[CompilerGenerated]
		internal static void <RefreshConnections>g__ClearSplines|27_0(SplineContainer splineContainer)
		{
			while (splineContainer.Splines.Count > 0)
			{
				splineContainer.RemoveSplineAt(0);
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0007BCF4 File Offset: 0x00079EF4
		[CompilerGenerated]
		internal static void <RefreshConnections>g__AddConnection|27_1(SplineContainer container, Vector2 from, Vector2 to, ref PerkTreeView.<>c__DisplayClass27_0 A_3)
		{
			if (A_3.horizontal)
			{
				container.AddSpline(new Spline(new BezierKnot[]
				{
					new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(new Vector3(from.x, to.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
					new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
				}, false));
				return;
			}
			container.AddSpline(new Spline(new BezierKnot[]
			{
				new BezierKnot(from, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(from - A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(new Vector3(to.x, from.y) - 2f * A_3.splineTangentVector, A_3.splineTangentVector, -A_3.splineTangentVector),
				new BezierKnot(to, A_3.splineTangentVector, -A_3.splineTangentVector)
			}, false));
		}

		// Token: 0x040017E7 RID: 6119
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x040017E8 RID: 6120
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017E9 RID: 6121
		[SerializeField]
		private RectTransform contentParent;

		// Token: 0x040017EA RID: 6122
		[SerializeField]
		private PerkDetails details;

		// Token: 0x040017EB RID: 6123
		[SerializeField]
		private PerkEntry perkEntryPrefab;

		// Token: 0x040017EC RID: 6124
		[SerializeField]
		private PerkLineEntry perkLinePrefab;

		// Token: 0x040017ED RID: 6125
		[SerializeField]
		private UISplineRenderer activeConnectionsRenderer;

		// Token: 0x040017EE RID: 6126
		[SerializeField]
		private UISplineRenderer inactiveConnectionsRenderer;

		// Token: 0x040017EF RID: 6127
		[SerializeField]
		private float splineTangent = 100f;

		// Token: 0x040017F0 RID: 6128
		[SerializeField]
		private PerkTree target;

		// Token: 0x040017F1 RID: 6129
		private PrefabPool<PerkEntry> _perkEntryPool;

		// Token: 0x040017F2 RID: 6130
		private PrefabPool<PerkLineEntry> _perkLinePool;

		// Token: 0x040017F3 RID: 6131
		private PerkEntry selectedPerkEntry;

		// Token: 0x040017F4 RID: 6132
		[SerializeField]
		private float layoutFactor = 10f;

		// Token: 0x040017F5 RID: 6133
		[SerializeField]
		private Vector2 padding = Vector2.one;
	}
}
