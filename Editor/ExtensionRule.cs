using System.Collections.Generic;

[System.Serializable]
public class ExtensionRule
{
    public List<string> extensions;
    public int minimalNamePart;
    public List<string> acceptableSuffixes;

}
