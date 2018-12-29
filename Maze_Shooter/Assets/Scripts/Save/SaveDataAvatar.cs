using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
[TypeInfoBox("Unique avatars are used instead of file names for save files. Avoids annoying typing. IMPORTANT: Each" +
             "avatar in the assets folder must have a unique name and sprite")]
public class SaveDataAvatar : ScriptableObject
{
             [PreviewField]
             public Sprite sprite;             
}
