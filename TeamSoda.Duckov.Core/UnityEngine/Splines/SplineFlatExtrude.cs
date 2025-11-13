using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200020D RID: 525
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Flat Extrude")]
	public class SplineFlatExtrude : MonoBehaviour
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0003CFB6 File Offset: 0x0003B1B6
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0003CFBE File Offset: 0x0003B1BE
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0003CFC6 File Offset: 0x0003B1C6
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

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0003CFCF File Offset: 0x0003B1CF
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0003CFD7 File Offset: 0x0003B1D7
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0003CFDF File Offset: 0x0003B1DF
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

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0003CFE8 File Offset: 0x0003B1E8
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0003CFF0 File Offset: 0x0003B1F0
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

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0003CFFF File Offset: 0x0003B1FF
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0003D007 File Offset: 0x0003B207
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

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0003D01A File Offset: 0x0003B21A
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x0003D022 File Offset: 0x0003B222
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

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000F6B RID: 3947 RVA: 0x0003D035 File Offset: 0x0003B235
		// (set) Token: 0x06000F6C RID: 3948 RVA: 0x0003D03D File Offset: 0x0003B23D
		public int ProfileSeg
		{
			get
			{
				return this.m_ProfileSeg;
			}
			set
			{
				this.m_ProfileSeg = value;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000F6D RID: 3949 RVA: 0x0003D046 File Offset: 0x0003B246
		// (set) Token: 0x06000F6E RID: 3950 RVA: 0x0003D04E File Offset: 0x0003B24E
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

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0003D057 File Offset: 0x0003B257
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0003D05F File Offset: 0x0003B25F
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

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0003D08E File Offset: 0x0003B28E
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

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000F72 RID: 3954 RVA: 0x0003D0A1 File Offset: 0x0003B2A1
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

		// Token: 0x06000F73 RID: 3955 RVA: 0x0003D0B4 File Offset: 0x0003B2B4
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

		// Token: 0x06000F74 RID: 3956 RVA: 0x0003D12C File Offset: 0x0003B32C
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

		// Token: 0x06000F75 RID: 3957 RVA: 0x0003D18B File Offset: 0x0003B38B
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0003D19E File Offset: 0x0003B39E
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0003D1B1 File Offset: 0x0003B3B1
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0003D1DE File Offset: 0x0003B3DE
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0003D1FC File Offset: 0x0003B3FC
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.m_Mesh, this.m_Width, this.m_ProfileSeg, this.m_Height, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0003D27C File Offset: 0x0003B47C
		private void Extrude<T>(T spline, Mesh mesh, float width, int profileSegments, float height, float segmentsPerUnit, float2 range) where T : ISpline
		{
			if (profileSegments < 2)
			{
				return;
			}
			float num = Mathf.Abs(range.y - range.x);
			int num2 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num * segmentsPerUnit), 1);
			float num3 = 0f;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			Vector3 b = Vector3.zero;
			for (int i = 0; i < num2; i++)
			{
				float num4 = math.lerp(range.x, range.y, (float)i / ((float)num2 - 1f));
				if (num4 > 1f)
				{
					num4 = 1f;
				}
				float3 v;
				float3 v2;
				float3 v3;
				spline.Evaluate(num4, out v, out v2, out v3);
				Vector3 normalized = v2.normalized;
				Vector3 normalized2 = v3.normalized;
				Vector3 a = Vector3.Cross(normalized, normalized2);
				float num5 = 1f / (float)(profileSegments - 1);
				if (i > 0)
				{
					num3 += (v - b).magnitude;
				}
				for (int j = 0; j < profileSegments; j++)
				{
					float num6 = num5 * (float)j;
					float num7 = (num6 - 0.5f) * 2f;
					float d = Mathf.Cos(num7 * 3.1415927f * 0.5f) * height;
					float d2 = num7 * width;
					Vector3 item = v + d2 * a + d * normalized2;
					list.Add(item);
					list3.Add(new Vector2(num6 * this.uFactor, num3 * this.vFactor));
					list2.Add(normalized2);
				}
				b = v;
			}
			SplineFlatExtrude.<>c__DisplayClass53_0<T> CS$<>8__locals1;
			CS$<>8__locals1.triangles = new List<int>();
			for (int k = 0; k < num2 - 1; k++)
			{
				int num8 = k * profileSegments;
				for (int l = 0; l < profileSegments - 1; l++)
				{
					int num9 = num8 + l;
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num9,
						num9 + 1,
						num9 + profileSegments
					}, ref CS$<>8__locals1);
					SplineFlatExtrude.<Extrude>g__AddTriangles|53_0<T>(new int[]
					{
						num9 + 1,
						num9 + 1 + profileSegments,
						num9 + profileSegments
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

		// Token: 0x06000F7B RID: 3963 RVA: 0x0003D4FA File Offset: 0x0003B6FA
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0003D502 File Offset: 0x0003B702
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003D515 File Offset: 0x0003B715
		private void FlattenSpline()
		{
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0003D58D File Offset: 0x0003B78D
		[CompilerGenerated]
		internal static void <Extrude>g__AddTriangles|53_0<T>(int[] indicies, ref SplineFlatExtrude.<>c__DisplayClass53_0<T> A_1) where T : ISpline
		{
			A_1.triangles.AddRange(indicies);
		}

		// Token: 0x04000C92 RID: 3218
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000C93 RID: 3219
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000C94 RID: 3220
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000C95 RID: 3221
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000C96 RID: 3222
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000C97 RID: 3223
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000C98 RID: 3224
		[SerializeField]
		private int m_ProfileSeg = 2;

		// Token: 0x04000C99 RID: 3225
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000C9A RID: 3226
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 0.999f);

		// Token: 0x04000C9B RID: 3227
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000C9C RID: 3228
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000C9D RID: 3229
		private Mesh m_Mesh;

		// Token: 0x04000C9E RID: 3230
		private bool m_RebuildRequested;

		// Token: 0x04000C9F RID: 3231
		private float m_NextScheduledRebuild;
	}
}
