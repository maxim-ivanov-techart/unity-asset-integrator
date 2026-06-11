using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetIntegratorPostprocessor : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        ApplyPresetForAsset(assetPath);
    }

    private void OnPreprocessModel()
    {
        ApplyPresetForAsset(assetPath);
    }

    private void ApplyPresetForAsset(string assetPath)
    {
        if (AssetIntegratorSession.CurrentConfig == null)
        {
            return;
        }
        
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
