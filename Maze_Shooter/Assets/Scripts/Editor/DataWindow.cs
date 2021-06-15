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

    // 
    // Be sure to check out OdinMenuStyleExample.cs as well. It shows you various ways to customize the look and behaviour of OdinMenuTrees.
    // 

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
            OdinMenuTree tree = new OdinMenuTree(true)
            {
                { "Home",                           GameMaster.Get(),                           EditorIcons.House                       }, // Draws the this.someData field in this case.
            };  

			tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
        	tree.Add("Menu Style", tree.DefaultMenuStyle);

			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(IntValue), true);
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(FloatValue), true);
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(HeartsValue), true);
			tree.AddAllAssetsAtPath("Data", "Assets/Data", typeof(DynamicsProfile), true);

            return tree;
        }
    }
}
#endif
