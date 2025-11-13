using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x02000077 RID: 119
public class AimTargetFinder : MonoBehaviour
{
	// Token: 0x06000464 RID: 1124 RVA: 0x000144ED File Offset: 0x000126ED
	private void Start()
	{
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000144F0 File Offset: 0x000126F0
	public Transform Find(bool search, Vector3 findPoint, ref CharacterMainControl foundCharacter)
	{
		Transform result = null;
		if (search)
		{
			result = this.Search(findPoint, ref foundCharacter);
		}
		return result;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001450C File Offset: 0x0001270C
	private Transform Search(Vector3 findPoint, ref CharacterMainControl character)
	{
		character = null;
		if (this.overlapcColliders == null)
		{
			this.overlapcColliders = new Collider[6];
			this.damageReceiverLayers = GameplayDataSettings.Layers.damageReceiverLayerMask;
		}
		int num = Physics.OverlapSphereNonAlloc(findPoint, this.searchRadius, this.overlapcColliders, this.damageReceiverLayers);
		Collider collider = null;
		if (num > 0)
		{
			int i = 0;
			while (i < num)
			{
				DamageReceiver component = this.overlapcColliders[i].GetComponent<DamageReceiver>();
				if (!(component == null) && component.Team != Teams.player)
				{
					collider = this.overlapcColliders[i];
					if (component.health != null)
					{
						character = component.health.GetComponent<CharacterMainControl>();
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		if (collider)
		{
			return collider.transform;
		}
		return null;
	}

	// Token: 0x040003CD RID: 973
	private Vector3 searchPoint;

	// Token: 0x040003CE RID: 974
	public float searchRadius;

	// Token: 0x040003CF RID: 975
	private LayerMask damageReceiverLayers;

	// Token: 0x040003D0 RID: 976
	private Collider[] overlapcColliders;
}
