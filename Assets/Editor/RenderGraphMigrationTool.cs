// TODO: Migrate to Render Graph
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Diagnostics;

public class RenderGraphMigrationTool
{
	[MenuItem("Tools/Migrate to Render Graph")]
	public static void MigrateRenderPasses()
	{
		string[] guids = AssetDatabase.FindAssets("t:Script");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			string code = File.ReadAllText(path);

			if (code.Contains("ScriptableRenderPass"))
			{
				if (code.StartsWith("// TODO: Migrate to Render Graph"))
				{
					UnityEngine.Debug.Log($"Already marked for migration: {path}");
					continue;
				}

				UnityEngine.Debug.Log($"Found ScriptableRenderPass in: {path}");
				// Přidá poznámku k migraci.
				File.WriteAllText(path, "// TODO: Migrate to Render Graph\n" + code);
			}
		}

		AssetDatabase.Refresh();
		UnityEngine.Debug.Log("Migration check completed.");
	}

}