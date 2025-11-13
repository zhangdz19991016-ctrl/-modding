using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.MiniGames
{
	// Token: 0x02000282 RID: 642
	public class VirtualCursor : MiniGameBehaviour
	{
		// Token: 0x06001486 RID: 5254 RVA: 0x0004C604 File Offset: 0x0004A804
		private void Awake()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = (base.transform as RectTransform);
			}
			if (this.moveArea == null)
			{
				this.moveArea = (this.rectTransform.parent as RectTransform);
			}
			if (this.canvas == null)
			{
				this.canvas = base.GetComponentInParent<Canvas>();
			}
			this.canvasRectTransform = (this.canvas.transform as RectTransform);
			if (this.raycaster == null)
			{
				this.raycaster = base.GetComponentInParent<GraphicRaycaster>();
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0004C6A0 File Offset: 0x0004A8A0
		private void Update()
		{
			if (base.Game == null)
			{
				return;
			}
			if (base.Game.Console && !base.Game.Console.Interacting)
			{
				return;
			}
			Vector2 mouseDelta = UIInputManager.MouseDelta;
			Vector3 vector = this.rectTransform.localPosition + mouseDelta * this.sensitivity;
			Rect rect = this.moveArea.rect;
			vector.x = Mathf.Clamp(vector.x, rect.min.x, rect.max.x);
			vector.y = Mathf.Clamp(vector.y, rect.min.y, rect.max.y);
			this.rectTransform.localPosition = vector;
			List<RaycastResult> list = new List<RaycastResult>();
			this.Raycast(list);
			RaycastResult raycastResult = VirtualCursor.FindFirstRaycast(list);
			if (raycastResult.gameObject != VirtualCursor.raycastGO)
			{
				VirtualCursorTarget virtualCursorTarget = VirtualCursor.target;
				VirtualCursorTarget virtualCursorTarget2;
				if (raycastResult.gameObject != null)
				{
					virtualCursorTarget2 = raycastResult.gameObject.GetComponent<VirtualCursorTarget>();
				}
				else
				{
					virtualCursorTarget2 = null;
				}
				if (virtualCursorTarget2 != virtualCursorTarget)
				{
					VirtualCursor.target = virtualCursorTarget2;
					this.OnChange(virtualCursorTarget2, virtualCursorTarget);
				}
			}
			if (UIInputManager.WasClickedThisFrame && VirtualCursor.target != null)
			{
				VirtualCursor.target.OnClick();
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x0004C800 File Offset: 0x0004AA00
		private void OnChange(VirtualCursorTarget newTarget, VirtualCursorTarget oldTarget)
		{
			if (newTarget != null)
			{
				newTarget.OnCursorEnter();
			}
			if (oldTarget != null)
			{
				oldTarget.OnCursorExit();
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0004C820 File Offset: 0x0004AA20
		private void Raycast(List<RaycastResult> resultAppendList)
		{
			if (this.canvas == null)
			{
				return;
			}
			IList<Graphic> raycastableGraphicsForCanvas = GraphicRegistry.GetRaycastableGraphicsForCanvas(this.canvas);
			VirtualCursor.s_canvasGraphics.Clear();
			if (raycastableGraphicsForCanvas == null || raycastableGraphicsForCanvas.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < raycastableGraphicsForCanvas.Count; i++)
			{
				VirtualCursor.s_canvasGraphics.Add(raycastableGraphicsForCanvas[i]);
			}
			Camera eventCamera = this.raycaster.eventCamera;
			Vector3 vector = eventCamera.WorldToScreenPoint(base.transform.position);
			vector.z = 0f;
			this.eventPositionWatch = vector;
			this.m_RaycastResults.Clear();
			VirtualCursor.Raycast(this.canvas, eventCamera, vector, raycastableGraphicsForCanvas, this.m_RaycastResults);
			int count = this.m_RaycastResults.Count;
			for (int j = 0; j < count; j++)
			{
				GameObject gameObject = this.m_RaycastResults[j].gameObject;
				float distance = 0f;
				Vector3 forward = gameObject.transform.forward;
				RaycastResult item = new RaycastResult
				{
					gameObject = gameObject,
					module = this.raycaster,
					distance = distance,
					screenPosition = vector,
					displayIndex = 0,
					index = (float)resultAppendList.Count,
					depth = this.m_RaycastResults[j].depth,
					sortingLayer = this.canvas.sortingLayerID,
					sortingOrder = this.canvas.sortingOrder,
					worldPosition = vector,
					worldNormal = -forward
				};
				resultAppendList.Add(item);
			}
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x0004C9D0 File Offset: 0x0004ABD0
		private static void Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, List<Graphic> results)
		{
			int count = foundGraphics.Count;
			for (int i = 0; i < count; i++)
			{
				Graphic graphic = foundGraphics[i];
				if (graphic.raycastTarget && !graphic.canvasRenderer.cull && graphic.depth != -1 && RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera, graphic.raycastPadding) && (!(eventCamera != null) || eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z <= eventCamera.farClipPlane) && graphic.Raycast(pointerPosition, eventCamera))
				{
					VirtualCursor.s_SortedGraphics.Add(graphic);
				}
			}
			VirtualCursor.s_SortedGraphics.Sort((Graphic g1, Graphic g2) => g2.depth.CompareTo(g1.depth));
			count = VirtualCursor.s_SortedGraphics.Count;
			for (int j = 0; j < count; j++)
			{
				results.Add(VirtualCursor.s_SortedGraphics[j]);
			}
			VirtualCursor.s_SortedGraphics.Clear();
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x0004CAC4 File Offset: 0x0004ACC4
		private static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			int count = candidates.Count;
			for (int i = 0; i < count; i++)
			{
				if (!(candidates[i].gameObject == null))
				{
					return candidates[i];
				}
			}
			return default(RaycastResult);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0004CB0C File Offset: 0x0004AD0C
		internal static bool IsHovering(VirtualCursorTarget virtualCursorTarget)
		{
			return virtualCursorTarget == VirtualCursor.target;
		}

		// Token: 0x04000F05 RID: 3845
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x04000F06 RID: 3846
		[SerializeField]
		private RectTransform moveArea;

		// Token: 0x04000F07 RID: 3847
		[SerializeField]
		private Canvas canvas;

		// Token: 0x04000F08 RID: 3848
		private RectTransform canvasRectTransform;

		// Token: 0x04000F09 RID: 3849
		[SerializeField]
		private GraphicRaycaster raycaster;

		// Token: 0x04000F0A RID: 3850
		[SerializeField]
		private float sensitivity = 0.5f;

		// Token: 0x04000F0B RID: 3851
		private static GameObject raycastGO;

		// Token: 0x04000F0C RID: 3852
		private static VirtualCursorTarget target;

		// Token: 0x04000F0D RID: 3853
		[NonSerialized]
		private List<Graphic> m_RaycastResults = new List<Graphic>();

		// Token: 0x04000F0E RID: 3854
		private Vector3 eventPositionWatch;

		// Token: 0x04000F0F RID: 3855
		[NonSerialized]
		private static List<Graphic> s_canvasGraphics = new List<Graphic>();

		// Token: 0x04000F10 RID: 3856
		[NonSerialized]
		private static readonly List<Graphic> s_SortedGraphics = new List<Graphic>();
	}
}
