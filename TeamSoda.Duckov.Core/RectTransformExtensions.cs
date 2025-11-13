using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public static class RectTransformExtensions
{
	// Token: 0x06000535 RID: 1333 RVA: 0x00017986 File Offset: 0x00015B86
	public static Camera GetUICamera()
	{
		return null;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001798C File Offset: 0x00015B8C
	public static bool MatchWorldPosition(this RectTransform rectTransform, Vector3 worldPosition, Vector3 worldSpaceOffset = default(Vector3))
	{
		RectTransform rectTransform2 = rectTransform.parent as RectTransform;
		if (rectTransform2 == null)
		{
			return false;
		}
		worldPosition += worldSpaceOffset;
		Camera main = Camera.main;
		if (main == null)
		{
			return false;
		}
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(main, worldPosition);
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform2, screenPoint, RectTransformExtensions.GetUICamera(), out v);
		Vector3 rhs = worldPosition - main.transform.position;
		bool result = Vector3.Dot(main.transform.forward, rhs) > 0f;
		rectTransform.localPosition = v;
		return result;
	}
}
