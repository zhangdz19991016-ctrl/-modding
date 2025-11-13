using System;

// Token: 0x02000074 RID: 116
public class Team
{
	// Token: 0x0600044B RID: 1099 RVA: 0x00013A16 File Offset: 0x00011C16
	public static bool IsEnemy(Teams selfTeam, Teams targetTeam)
	{
		return selfTeam != Teams.middle && (selfTeam == Teams.all || (targetTeam != Teams.middle && selfTeam != targetTeam));
	}
}
