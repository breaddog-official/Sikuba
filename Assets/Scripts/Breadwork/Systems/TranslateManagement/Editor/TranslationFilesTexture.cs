/*using Scripts.SaveManagement;
using Scripts.TranslateManagement;
using System;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "yml"), Obsolete]
public class TranslationFilesTexture : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Translation loadedTranslation = SaveManager.LoadFromPath<Translation>(ctx.assetPath, SaveManager.Savers.YAML);

        TextAsset text = new TextAsset(loadedTranslation.ToString());
        Texture2D icon = Texture2D.grayTexture;

        Debug.Log("asda");
        ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), text, icon);
        //ctx.SetMainObject(icon);
    }
}*/