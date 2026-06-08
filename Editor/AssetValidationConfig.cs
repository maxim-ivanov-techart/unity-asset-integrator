using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetValidationConfig", menuName = "Asset Integrator/Asset Validation Config")]
public class AssetValidationConfig : ScriptableObject
{
    public List<ExtensionRule> rules = new List<ExtensionRule>();
}
