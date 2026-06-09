using System.Collections.Generic;
using UnityEditor.Presets;

[System.Serializable]
public class ExtensionRule
{
    public List<string> extensions;
    public int minimalNamePart;
    public List<string> acceptableSuffixes;
    public Preset preset;

}
