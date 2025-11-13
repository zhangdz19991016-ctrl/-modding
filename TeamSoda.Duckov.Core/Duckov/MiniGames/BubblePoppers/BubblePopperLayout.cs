using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002E0 RID: 736
	public class BubblePopperLayout : MiniGameBehaviour
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x000573BA File Offset: 0x000555BA
		public Vector2 XPositionBorder
		{
			get
			{
				return new Vector2((float)this.xBorder.x * this.BubbleRadius * 2f - this.BubbleRadius, (float)this.xBorder.y * this.BubbleRadius * 2f);
			}
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000573FC File Offset: 0x000555FC
		public Vector2 CoordToLocalPosition(Vector2Int coord)
		{
			float bubbleRadius = this.BubbleRadius;
			return new Vector2(((coord.y % 2 != 0) ? bubbleRadius : 0f) + (float)coord.x * bubbleRadius * 2f, (float)coord.y * bubbleRadius * BubblePopperLayout.YOffsetFactor);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x0005744C File Offset: 0x0005564C
		public Vector2Int LocalPositionToCoord(Vector2 localPosition)
		{
			float bubbleRadius = this.BubbleRadius;
			int num = Mathf.RoundToInt(localPosition.y / bubbleRadius / BubblePopperLayout.YOffsetFactor);
			float num2 = (num % 2 != 0) ? bubbleRadius : 0f;
			return new Vector2Int(Mathf.RoundToInt((localPosition.x - num2) / bubbleRadius / 2f), num);
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000574A0 File Offset: 0x000556A0
		public Vector2Int WorldPositionToCoord(Vector2 position)
		{
			Vector3 v = base.transform.worldToLocalMatrix.MultiplyPoint(position);
			return this.LocalPositionToCoord(v);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000574D4 File Offset: 0x000556D4
		public Vector2Int[] GetAllNeighbourCoords(Vector2Int center, bool includeCenter)
		{
			int num = (center.y % 2 != 0) ? 0 : -1;
			Vector2Int[] result;
			if (includeCenter)
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					center,
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			else
			{
				result = new Vector2Int[]
				{
					new Vector2Int(center.x + num, center.y + 1),
					new Vector2Int(center.x + num + 1, center.y + 1),
					new Vector2Int(center.x - 1, center.y),
					new Vector2Int(center.x + 1, center.y),
					new Vector2Int(center.x + num, center.y - 1),
					new Vector2Int(center.x + num + 1, center.y - 1)
				};
			}
			return result;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00057680 File Offset: 0x00055880
		public List<Vector2Int> GetAllPassingCoords(Vector2 localOrigin, Vector2 direction, float length)
		{
			float num = this.BubbleRadius * 2f;
			List<Vector2Int> list = new List<Vector2Int>
			{
				this.LocalPositionToCoord(localOrigin)
			};
			if (num > 0f)
			{
				float num2 = -num;
				while (num2 < length)
				{
					num2 += num;
					Vector2 localPosition = localOrigin + num2 * direction;
					Vector2Int center = this.LocalPositionToCoord(localPosition);
					list.AddRange(this.GetAllNeighbourCoords(center, true));
				}
			}
			return list;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000576EC File Offset: 0x000558EC
		private void OnDrawGizmos()
		{
			float bubbleRadius = this.BubbleRadius;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.x, 0f), new Vector3(this.XPositionBorder.x, -100f));
			Gizmos.DrawLine(new Vector3(this.XPositionBorder.y, 0f), new Vector3(this.XPositionBorder.y, -100f));
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00057778 File Offset: 0x00055978
		public void GizmosDrawCoord(Vector2Int coord, float ratio)
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawSphere(this.CoordToLocalPosition(coord), this.BubbleRadius * ratio);
			Gizmos.matrix = matrix;
		}

		// Token: 0x04001146 RID: 4422
		[SerializeField]
		private Vector2Int xBorder;

		// Token: 0x04001147 RID: 4423
		public Vector2Int XCoordBorder;

		// Token: 0x04001148 RID: 4424
		public float BubbleRadius = 8f;

		// Token: 0x04001149 RID: 4425
		public static readonly float YOffsetFactor = Mathf.Tan(1.0471976f);

		// Token: 0x0400114A RID: 4426
		[SerializeField]
		private Transform tester;

		// Token: 0x0400114B RID: 4427
		[SerializeField]
		private float distance = 10f;

		// Token: 0x0400114C RID: 4428
		[SerializeField]
		private Vector2Int min;

		// Token: 0x0400114D RID: 4429
		[SerializeField]
		private Vector2Int max;
	}
}
