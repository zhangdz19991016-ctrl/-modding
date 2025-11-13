using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200020F RID: 527
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	[AddComponentMenu("Splines/Spline Sidewalk Extrude")]
	public class SplineSidewalkExtrude : MonoBehaviour
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0003DBB0 File Offset: 0x0003BDB0
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0003DBB8 File Offset: 0x0003BDB8
		// (set) Token: 0x06000FA2 RID: 4002 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000FA3 RID: 4003 RVA: 0x0003DBC9 File Offset: 0x0003BDC9
		[Obsolete("Use RebuildOnSplineChange instead.", false)]
		public bool rebuildOnSplineChange
		{
			get
			{
				return this.RebuildOnSplineChange;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0003DBD1 File Offset: 0x0003BDD1
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x0003DBD9 File Offset: 0x0003BDD9
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

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0003DBE2 File Offset: 0x0003BDE2
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0003DBEA File Offset: 0x0003BDEA
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0003DBF9 File Offset: 0x0003BDF9
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x0003DC01 File Offset: 0x0003BE01
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0003DC14 File Offset: 0x0003BE14
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x0003DC1C File Offset: 0x0003BE1C
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0003DC2F File Offset: 0x0003BE2F
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x0003DC37 File Offset: 0x0003BE37
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

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0003DC40 File Offset: 0x0003BE40
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x0003DC48 File Offset: 0x0003BE48
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

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0003DC77 File Offset: 0x0003BE77
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

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0003DC8A File Offset: 0x0003BE8A
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

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0003DCA0 File Offset: 0x0003BEA0
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

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0003DD18 File Offset: 0x0003BF18
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

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0003DD77 File Offset: 0x0003BF77
		private void OnEnable()
		{
			Spline.Changed += this.OnSplineChanged;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0003DD8A File Offset: 0x0003BF8A
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0003DD9D File Offset: 0x0003BF9D
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.Splines.Contains(spline) && this.m_RebuildOnSplineChange)
			{
				this.m_RebuildRequested = true;
			}
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0003DDCA File Offset: 0x0003BFCA
		private void Update()
		{
			if (this.m_RebuildRequested && Time.time >= this.m_NextScheduledRebuild)
			{
				this.Rebuild();
			}
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0003DDE8 File Offset: 0x0003BFE8
		public void Rebuild()
		{
			if ((this.m_Mesh = base.GetComponent<MeshFilter>().sharedMesh) == null)
			{
				return;
			}
			this.Extrude<Spline>(this.Splines[0], this.m_Mesh, this.m_SegmentsPerUnit, this.m_Range);
			this.m_NextScheduledRebuild = Time.time + 1f / (float)this.m_RebuildFrequency;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0003DE54 File Offset: 0x0003C054
		private void Extrude<T>(T spline, Mesh mesh, float segmentsPerUnit, float2 range) where T : ISpline
		{
			SplineSidewalkExtrude.<>c__DisplayClass55_0<T> CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			mesh.Clear();
			if (this.sides == SplineSidewalkExtrude.Sides.None)
			{
				return;
			}
			float num = Mathf.Abs(range.y - range.x);
			int num2 = Mathf.Max((int)Mathf.Ceil(spline.GetLength() * num * segmentsPerUnit), 1);
			CS$<>8__locals1.v = 0f;
			CS$<>8__locals1.verts = new List<Vector3>();
			CS$<>8__locals1.n = new List<Vector3>();
			CS$<>8__locals1.uv = new List<Vector2>();
			CS$<>8__locals1.triangles = new List<int>();
			Vector3 b = Vector3.zero;
			SplineSidewalkExtrude.ProfileLine[] array = this.GenerateProfile();
			CS$<>8__locals1.profileVertexCount = array.Length * 2;
			for (int i = 0; i < num2; i++)
			{
				SplineSidewalkExtrude.<>c__DisplayClass55_1<T> CS$<>8__locals2;
				CS$<>8__locals2.isLastSegment = (i == num2 - 1);
				float num3 = math.lerp(range.x, range.y, (float)i / ((float)num2 - 1f));
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				if (num3 < 1E-07f)
				{
					num3 = 1E-07f;
				}
				float3 v;
				float3 v2;
				spline.Evaluate(num3, out CS$<>8__locals2.center, out v, out v2);
				CS$<>8__locals2.forward = v.normalized;
				CS$<>8__locals2.up = v2.normalized;
				CS$<>8__locals2.right = Vector3.Cross(CS$<>8__locals2.forward, CS$<>8__locals2.up);
				if (i > 0)
				{
					CS$<>8__locals1.v += (CS$<>8__locals2.center - b).magnitude;
				}
				foreach (SplineSidewalkExtrude.ProfileLine profileLine in array)
				{
					this.<Extrude>g__DrawLine|55_2<T>(profileLine.start, profileLine.end, profileLine.u0, profileLine.u1, ref CS$<>8__locals1, ref CS$<>8__locals2);
				}
				b = CS$<>8__locals2.center;
			}
			mesh.vertices = CS$<>8__locals1.verts.ToArray();
			mesh.uv = CS$<>8__locals1.uv.ToArray();
			mesh.triangles = CS$<>8__locals1.triangles.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x0003E088 File Offset: 0x0003C288
		private SplineSidewalkExtrude.ProfileLine[] GenerateProfile()
		{
			SplineSidewalkExtrude.<>c__DisplayClass57_0 CS$<>8__locals1;
			CS$<>8__locals1.lines = new List<SplineSidewalkExtrude.ProfileLine>();
			float num = this.height - this.bevel;
			float num2 = Mathf.Sqrt(2f * this.bevel * this.bevel);
			float num3 = this.width - 2f * this.bevel;
			CS$<>8__locals1.uFactor = num + num2 + num3 + num2 + num;
			if ((this.sides | SplineSidewalkExtrude.Sides.Left) == this.sides)
			{
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width, 0f, -this.offset - this.width, this.height - this.bevel, 0f, num, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width + this.bevel, this.height, -this.offset - this.bevel, this.height, num + num2, num + num2 + num3, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset, this.height - this.bevel, -this.offset, 0f, num + num2 + num3 + num2, num + num2 + num3 + num2 + num, ref CS$<>8__locals1);
				if (this.bevel > 0f)
				{
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.width, this.height - this.bevel, -this.offset - this.width + this.bevel, this.height, num, num + num2, ref CS$<>8__locals1);
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(-this.offset - this.bevel, this.height, -this.offset, this.height - this.bevel, num + num2 + num3, num + num2 + num3 + num2, ref CS$<>8__locals1);
				}
			}
			if ((this.sides | SplineSidewalkExtrude.Sides.Right) == this.sides)
			{
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset, 0f, this.offset, this.height - this.bevel, num + num2 + num3 + num2 + num, num + num2 + num3 + num2, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.bevel, this.height, this.offset + this.width - this.bevel, this.height, num + num2 + num3, num + num2, ref CS$<>8__locals1);
				SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.width, this.height - this.bevel, this.offset + this.width, 0f, num, 0f, ref CS$<>8__locals1);
				if (this.bevel > 0f)
				{
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset, this.height - this.bevel, this.offset + this.bevel, this.height, num + num2 + num3 + num2, num + num2 + num3, ref CS$<>8__locals1);
					SplineSidewalkExtrude.<GenerateProfile>g__Add|57_0(this.offset + this.width - this.bevel, this.height, this.offset + this.width, this.height - this.bevel, num + num2, num, ref CS$<>8__locals1);
				}
			}
			return CS$<>8__locals1.lines.ToArray();
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0003E385 File Offset: 0x0003C585
		private void OnValidate()
		{
			this.Rebuild();
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0003E38D File Offset: 0x0003C58D
		internal Mesh CreateMeshAsset()
		{
			return new Mesh
			{
				name = base.name
			};
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0003E3A0 File Offset: 0x0003C5A0
		private void FlattenSpline()
		{
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0003E41C File Offset: 0x0003C61C
		[CompilerGenerated]
		internal static Vector3 <Extrude>g__ProfileToObject|55_1<T>(Vector3 profilePos, ref SplineSidewalkExtrude.<>c__DisplayClass55_1<T> A_1) where T : ISpline
		{
			return A_1.center + profilePos.x * A_1.right + profilePos.y * A_1.up + profilePos.z * A_1.forward;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0003E478 File Offset: 0x0003C678
		[CompilerGenerated]
		private void <Extrude>g__DrawLine|55_2<T>(Vector3 p0, Vector3 p1, float u0, float u1, ref SplineSidewalkExtrude.<>c__DisplayClass55_0<T> A_5, ref SplineSidewalkExtrude.<>c__DisplayClass55_1<T> A_6) where T : ISpline
		{
			Vector3 vector = SplineSidewalkExtrude.<Extrude>g__ProfileToObject|55_1<T>(p0, ref A_6);
			Vector3 vector2 = SplineSidewalkExtrude.<Extrude>g__ProfileToObject|55_1<T>(p1, ref A_6);
			Vector3 item = Vector3.Cross(vector2 - vector, A_6.forward);
			int count = A_5.verts.Count;
			A_5.verts.Add(vector);
			A_5.verts.Add(vector2);
			A_5.n.Add(item);
			A_5.n.Add(item);
			A_5.uv.Add(new Vector2(u0 * this.uFactor, A_5.v * this.vFactor));
			A_5.uv.Add(new Vector2(u1 * this.uFactor, A_5.v * this.vFactor));
			if (!A_6.isLastSegment)
			{
				this.<Extrude>g__AddTriangles|55_0<T>(new int[]
				{
					count,
					count + 1,
					count + A_5.profileVertexCount
				}, ref A_5);
				this.<Extrude>g__AddTriangles|55_0<T>(new int[]
				{
					count + 1,
					count + 1 + A_5.profileVertexCount,
					count + A_5.profileVertexCount
				}, ref A_5);
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0003E596 File Offset: 0x0003C796
		[CompilerGenerated]
		private void <Extrude>g__AddTriangles|55_0<T>(int[] indicies, ref SplineSidewalkExtrude.<>c__DisplayClass55_0<T> A_2) where T : ISpline
		{
			A_2.triangles.AddRange(indicies);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0003E5A4 File Offset: 0x0003C7A4
		[CompilerGenerated]
		internal static void <GenerateProfile>g__Add|57_0(float x0, float y0, float x1, float y1, float u0, float u1, ref SplineSidewalkExtrude.<>c__DisplayClass57_0 A_6)
		{
			A_6.lines.Add(new SplineSidewalkExtrude.ProfileLine(new Vector3(x0, y0), new Vector3(x1, y1), u0 / A_6.uFactor, u1 / A_6.uFactor));
		}

		// Token: 0x04000CAE RID: 3246
		[SerializeField]
		[Tooltip("The Spline to extrude.")]
		private SplineContainer m_Container;

		// Token: 0x04000CAF RID: 3247
		[SerializeField]
		private float offset;

		// Token: 0x04000CB0 RID: 3248
		[SerializeField]
		private float height;

		// Token: 0x04000CB1 RID: 3249
		[SerializeField]
		private float width;

		// Token: 0x04000CB2 RID: 3250
		[SerializeField]
		private float bevel;

		// Token: 0x04000CB3 RID: 3251
		[SerializeField]
		private SplineSidewalkExtrude.Sides sides = SplineSidewalkExtrude.Sides.Both;

		// Token: 0x04000CB4 RID: 3252
		[SerializeField]
		[Tooltip("Enable to regenerate the extruded mesh when the target Spline is modified. Disable this option if the Spline will not be modified at runtime.")]
		private bool m_RebuildOnSplineChange;

		// Token: 0x04000CB5 RID: 3253
		[SerializeField]
		[Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
		private int m_RebuildFrequency = 30;

		// Token: 0x04000CB6 RID: 3254
		[SerializeField]
		[Tooltip("Automatically update any Mesh, Box, or Sphere collider components when the mesh is extruded.")]
		private bool m_UpdateColliders = true;

		// Token: 0x04000CB7 RID: 3255
		[SerializeField]
		[Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
		private float m_SegmentsPerUnit = 4f;

		// Token: 0x04000CB8 RID: 3256
		[SerializeField]
		[Tooltip("The radius of the extruded mesh.")]
		private float m_Width = 0.25f;

		// Token: 0x04000CB9 RID: 3257
		[SerializeField]
		private float m_Height = 0.05f;

		// Token: 0x04000CBA RID: 3258
		[SerializeField]
		[Tooltip("The section of the Spline to extrude.")]
		private Vector2 m_Range = new Vector2(0f, 0.999f);

		// Token: 0x04000CBB RID: 3259
		[SerializeField]
		private float uFactor = 1f;

		// Token: 0x04000CBC RID: 3260
		[SerializeField]
		private float vFactor = 1f;

		// Token: 0x04000CBD RID: 3261
		private Mesh m_Mesh;

		// Token: 0x04000CBE RID: 3262
		private bool m_RebuildRequested;

		// Token: 0x04000CBF RID: 3263
		private float m_NextScheduledRebuild;

		// Token: 0x020004F4 RID: 1268
		[Flags]
		public enum Sides
		{
			// Token: 0x04001D96 RID: 7574
			None = 0,
			// Token: 0x04001D97 RID: 7575
			Left = 1,
			// Token: 0x04001D98 RID: 7576
			Right = 2,
			// Token: 0x04001D99 RID: 7577
			Both = 3
		}

		// Token: 0x020004F5 RID: 1269
		private struct ProfileLine
		{
			// Token: 0x060027B5 RID: 10165 RVA: 0x00090D57 File Offset: 0x0008EF57
			public ProfileLine(Vector3 start, Vector3 end, float u0, float u1)
			{
				this.start = start;
				this.end = end;
				this.u0 = u0;
				this.u1 = u1;
			}

			// Token: 0x04001D9A RID: 7578
			public Vector3 start;

			// Token: 0x04001D9B RID: 7579
			public Vector3 end;

			// Token: 0x04001D9C RID: 7580
			public float u0;

			// Token: 0x04001D9D RID: 7581
			public float u1;
		}
	}
}
