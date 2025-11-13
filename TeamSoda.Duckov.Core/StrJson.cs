using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02000097 RID: 151
public class StrJson
{
	// Token: 0x0600052F RID: 1327 RVA: 0x000177E4 File Offset: 0x000159E4
	private StrJson(params string[] contentPairs)
	{
		this.entries = new List<StrJson.Entry>();
		for (int i = 0; i < contentPairs.Length - 1; i += 2)
		{
			this.entries.Add(new StrJson.Entry(contentPairs[i], contentPairs[i + 1]));
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001782A File Offset: 0x00015A2A
	public StrJson Add(string key, string value)
	{
		this.entries.Add(new StrJson.Entry(key, value));
		return this;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0001783F File Offset: 0x00015A3F
	public static StrJson Create(params string[] contentPairs)
	{
		return new StrJson(contentPairs);
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00017848 File Offset: 0x00015A48
	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{");
		for (int i = 0; i < this.entries.Count; i++)
		{
			StrJson.Entry entry = this.entries[i];
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"",
				entry.key,
				"\":\"",
				entry.value,
				"\""
			}));
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	// Token: 0x040004B1 RID: 1201
	public List<StrJson.Entry> entries;

	// Token: 0x02000450 RID: 1104
	public struct Entry
	{
		// Token: 0x06002696 RID: 9878 RVA: 0x00086FE2 File Offset: 0x000851E2
		public Entry(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x04001AEA RID: 6890
		public string key;

		// Token: 0x04001AEB RID: 6891
		public string value;
	}
}
