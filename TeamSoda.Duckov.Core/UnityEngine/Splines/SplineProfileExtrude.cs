using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200020E RID: 526
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Profile Extrude")]
	public class SplineProfileExtrude : MonoBehaviour
	{
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0003D59B File Offset: 0x0003B79B
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0003D5A3 File Offset: 0x0003B7A3
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x0003D5AB File Offset: 0x0003B7AB
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000F83 RID: 3971 RVA: 0x0003D5B4 File Offset: 0x0003B7B4
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x0003D5BC File Offset: 0x0003B7BC
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x0003D5C4 File Offset: 0x0003B7C4
		public bool RebuildOnSplineChange
		{
			get
			{
				return this.m_RebuildOnSplineChange;
			}
			set
			{
				this.m_RebuildOnSplineChange = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0003D5CD File Offset: 0x0003B7CD
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x0003D5D5 File Offset: 0x0003B7D5
		public int RebuildFrequency
		{
			get
			{
				return this.m_RebuildFrequency;
			}
			set
			{
				this.m_RebuildFrequency = Mathf.Max(value, 1);
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0003D5E4 File Offset: 0x0003B7E4
		// (set) Token: 0x06000F89 RID: 3977 RVA: 0x0003D5EC File Offset: 0x0003B7EC
		public float SegmentsPerUnit
		{
			get
			{
				return this.m_SegmentsPerUnit;
			}
			set
			{
				this.m_SegmentsPerUnit = Mathf.Max(value, 0.0001f);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0003D5FF File Offset: 0x0003B7FF
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x0003D607 File Offset: 0x0003B807
		public float Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = Mathf.Max(value, 1E-05f);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x0003D61A File Offset: 0x0003B81A
		public int ProfileSeg
		{
			get
			{
				return this.profile.Length;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000F8D RID: 3981 RVA: 0x0003D624 File Offset: 0x0003B824
		// (set) Token: 0x06000F8E RID: 3982 RVA: 0x0003D62C File Offset: 0x0003B82C
		public float Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x0003D635 File Offset: 0x0003B835
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0003D63D File Offset: 0x0003B83D
		public Vector2 Range
		{
			get
			{
				return this.m_Range;
			}
			set
			{
				this.m_Range = new Vector2(Mathf.Min(value.x, value.y), Mathf.Max(value.x, value.y));
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000F91 RID: 3985 RVA: 0x0003D66C File Offset: 0x0003B86C
		public Spline Spline
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Spline;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x0003D67F File Offset: 0x0003B87F
		public IReadOnlyList<Spline> Splines
		{
			get
			{
				SplineContainer container = this.m_Container;
				if (container == null)
				{
					return null;
				}
				return container.Splines;
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0003D694 File Offset: 0x0003B894
		internal void Reset()
		{
			base.TryGetComponent<SplineContainer>(out this.m_Container);
			MeshFilter meshFilter;
			if (base.TryGetComponent<MeshFilter>(out meshFilter))
			{
				meshFilter.sharedMesh = (this.m_Mesh = this.CreateMeshAsset());
			}
			MeshRenderer meshRenderer;
			if (base.TryGetComponent<MeshRenderer>(out meshRenderer) && meshRenderer.sharedMaterial == null)
			{
				GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				Material sharedMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
				Object.DestroyImmediate(gameObject);
				meshRenderer.sharedMaterial = sharedMaterial;
			}
			this.Rebuild();
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0003D70C File Offset: 0x0003B90C
		private void Start()
		{
			if (this.m_Container == null || this.m_Container.Spline == null)
			{
				Debug.LogError("Spline Extrude does not have a valid SplineContainer set.");
				return;
			}
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				Debug.LogError("SplineExtrude.createMeshInstance is disabled, but there is no valid mesh assigned. Please create or assign a writable mesh asset.");
			}
			this.Rebuild();
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0003D76B File Offset: 0x0003B96B
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0003D77E File Offset: 0x0003B97E
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0003D791 File Offset: 0x0003B991
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0003D7BE File Offset: 0x0003B9BE
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0003D7DC File Offset: 0x0003B9DC
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.profile, this.m_Mesh, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0003D850 File Offset: 0x0003BA50
		private void Extrude<T>(T spline, SplineProfileExtrude.Vertex[] profile, Mesh mesh, float segmentsPerUnit, float2 range) where T : ISpline
		{
			int num = profile.Length;
			if (num < 2)
			{
				return;
			}
			float num2 = Mathf.Abs(range.y - range.x);
			int num3 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num2 * segmentsPerUnit), 1);
			float num4 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 b = Vector3.zero;
			for (int i = 0; i < num3; i++)
			{
				float num5 = math.lerp(range.x, range.y, (float)i / ((float)num3 - 1f));
				if (num5 > 1f)
				{
					num5 = 1f;
				}
				if (num5 < 1E-07f)
				{
					num5 = 1E-07f;
				}
				float3 v;
				float3 v2;
				float3 v3;
				spline.Evaluate(num5, out v, out v2, out v3);
				Vector3 normalized = v2.normalized;
				Vector3 normalized2 = v3.normalized;
				Vector3 a = Vector3.Cross(normalized, normalized2);
				float num6 = 1f / (float)(num - 1);
				if (i > 0)
				{
					num4 += (v - b).magnitude;
				}
				for (int j = 0; j < num; j++)
				{
					SplineProfileExtrude.Vertex vertex = profile[j];
					float u = vertex.u;
					float y = vertex.position.y;
					float x = vertex.position.x;
					float z = vertex.position.z;
					Vector3 item = Quaternion.FromToRotation(Vector3.up, normalized2) * vertex.normal;
					Vector3 item2 = v + x * a + y * normalized2 + z * normalized;
					list.Add(item2);
					list3.Add(new Vector2(u * this.uFactor, num4 * this.vFactor));
					list2.Add(item);
				}
				b = v;
			}
			SplineProfileExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num3 - 1; k++)
			{
				int num7 = k * num;
				for (int l = 0; l < num - 1; l++)
				{
					int num8 = num7 + l;
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8,
						num8 + 1,
						num8 + num
					}, ref CS$<>8__locals1);
					SplineProfileExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num8 + 1,
						num8 + 1 + num,
						num8 + num
					}, ref CS$<>8__locals1);
				}
			}
			mesh.Clear();
			mesh.vertices = list.ToArray();
			mesh.uv = list3.ToArray();
			mesh.triangles = CS$<>8__locals1.triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003DB15 File Offset: 0x0003BD15
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x0003DB1D File Offset: 0x0003BD1D
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0003DB30 File Offset: 0x0003BD30
		private void FlattenSpline()
		{
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003DBA2 File Offset: 0x0003BDA2
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineProfileExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000CA0 RID: 3232
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000CA1 RID: 3233
		[SerializeField]
		private SplineProfileExtrude.Vertex[] profile;

		// Token: 0x04000CA2 RID: 3234
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000CA3 RID: 3235
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000CA4 RID: 3236
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000CA5 RID: 3237
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000CA6 RID: 3238
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000CA7 RID: 3239
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000CA8 RID: 3240
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 1f);

		// Token: 0x04000CA9 RID: 3241
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000CAA RID: 3242
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000CAB RID: 3243
		private Mesh m_Mesh;

		// Token: 0x04000CAC RID: 3244
		private bool m_RebuildRequested;

		// Token: 0x04000CAD RID: 3245
		private float m_NextScheduledRebuild;

		// Token: 0x020004F2 RID: 1266
		[Serializable]
		private struct Vertex
		{
			// Token: 0x04001D91 RID: 7569
			public Vector3 position;

			// Token: 0x04001D92 RID: 7570
			public Vector3 normal;

			// Token: 0x04001D93 RID: 7571
			public float u;
		}
	}
}
