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
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(true);
			tree.DrawSearchToolbar();

			tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
        	tree.Add("Menu Style", 		tree.DefaultMenuStyle, 	EditorIcons.SettingsCog);
			tree.Add("Game Master", 	GameMaster.Get(), 		EditorIcons.PacmanGhost);

			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(IntValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(FloatValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(HeartsValue), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(DynamicsProfile), true).AddThumbnailIcons();

			tree.Add("Audio", 	null, 		EditorIcons.Sound);
			tree.AddAllAssetsAtPath("Audio", "Assets/Audio/Audio Collections", typeof(AudioCollection), true).AddThumbnailIcons();

			tree.Add("Save Data", null, EditorIcons.GridBlocks);
			tree.AddAllAssetsAtPath("Save Data", "Assets/Save File Data", typeof(SavedInt), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Save Data", "Assets/Save File Data", typeof(SavedFloat), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Save Data", "Assets/Save File Data", typeof(SavedBool), true).AddThumbnailIcons();
			tree.AddAllAssetsAtPath("Save Data", "Assets/Save File Data", typeof(SavedString), true).AddThumbnailIcons();

            return tree;
        }
    }
}
#endif