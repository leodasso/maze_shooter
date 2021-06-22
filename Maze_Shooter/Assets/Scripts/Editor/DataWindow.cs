#if UNITY_EDITOR
namespace ShootyGhost
{
    using Sirenix.OdinInspector.Editor;
    using System.Linq;
    using UnityEngine;
    using Sirenix.Utilities.Editor;
    using Sirenix.Serialization;
    using UnityEditor;
    using Sirenix.Utilities;
	using Sirenix.OdinInspector;
	using Arachnid;
	using System.Collections.Generic;


    public class DataWindow : OdinMenuEditorWindow
    {
        [MenuItem("Hauntii/Data Inspector")]
        private static void OpenWindow()
        {
            var window = GetWindow<DataWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
			window.titleContent = new GUIContent("Hauntii Data", EditorIcons.PacmanGhost.Raw);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(true);

			tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;

			tree.Config.DrawSearchToolbar = true;

        	//tree.Add("Menu Style", 		tree.DefaultMenuStyle, 	EditorIcons.SettingsCog);
			tree.Add("Game Master", 	GameMaster.Get(), 		EditorIcons.PacmanGhost);

			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(IntValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(FloatValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(HeartsValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(DynamicsProfile), true).AddThumbnailIcons();

			tree.Add("Audio", 	null, 		EditorIcons.Sound);
			tree.AddAllAssetsAtPath("Audio", "Assets/Audio/Audio Collections", typeof(AudioCollection), true).AddThumbnailIcons();

			tree.Add("Save Data", null, EditorIcons.GridBlocks);

			BuildSaveDataTree<string>(tree, "StringValue");
			BuildSaveDataTree<int>(tree, "IntValue");
			BuildSaveDataTree<float>(tree, "FloatValue");
			BuildSaveDataTree<bool>(tree, "BoolValue");
			BuildSaveDataTree<Hearts>(tree, "HeartsValue");
			BuildSaveDataTree<bool>(tree, "ConstellationData");

            return tree;
        }

		void BuildSaveDataTree<T>(OdinMenuTree tree, string typeName)
		{
			var guids = AssetDatabase.FindAssets("t:" + typeName, new[] {"Assets/Data"});
			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ValueAsset<T>)) as ValueAsset<T>;
				if (!asset.useSaveFile) continue;

				// remove 'assets/data/'
				string trimmedPath = assetPath.Substring(12);

				// remove '.asset'
				int length = trimmedPath.Length - 6;
				trimmedPath = trimmedPath.Substring(0, length);
				tree.AddObjectAtPath("Save Data/" + trimmedPath, asset).AddThumbnailIcons();
			}
		}
    }
}
#endif