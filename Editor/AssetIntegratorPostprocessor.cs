using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetIntegratorPostprocessor : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if (AssetIntegratorSession.CurrentConfig == null)
        {
            return;
        }
        
        string textureName = Path.GetFileNameWithoutExtension(assetPath);
        string extension = Path.GetExtension(assetPath);

        foreach (var rule in AssetIntegratorSession.CurrentConfig.rules)
        {
            if (rule.extensions.Contains(extension))
            {
                rule.preset.ApplyTo(assetImporter);
            }
        }
    }
}
