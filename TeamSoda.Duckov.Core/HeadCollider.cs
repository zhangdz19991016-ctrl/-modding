using System;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class HeadCollider : MonoBehaviour
{
	// Token: 0x060003A7 RID: 935 RVA: 0x000100B5 File Offset: 0x0000E2B5
	public void Init(CharacterMainControl _character)
	{
		this.character = _character;
		this.character.OnTeamChanged += this.OnSetTeam;
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x000100D5 File Offset: 0x0000E2D5
	private void OnDestroy()
	{
		if (this.character)
		{
			this.character.OnTeamChanged -= this.OnSetTeam;
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x000100FC File Offset: 0x0000E2FC
	private void OnSetTeam(Teams team)
	{
		bool enabled = Team.IsEnemy(Teams.player, team);
		this.sphereCollider.enabled = enabled;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00010120 File Offset: 0x0000E320
	private void OnDrawGizmos()
	{
		Color yellow = Color.yellow;
		yellow.a = 0.3f;
		Gizmos.color = yellow;
		Gizmos.DrawSphere(base.transform.position, this.sphereCollider.radius * base.transform.lossyScale.x);
	}

	// Token: 0x040002C2 RID: 706
	private CharacterMainControl character;

	// Token: 0x040002C3 RID: 707
	[SerializeField]
	private SphereCollider sphereCollider;
}
