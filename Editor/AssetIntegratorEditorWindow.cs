using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AssetIntegratorEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset;

    private Label folderPathLabel;
    private ListView assetListView;
    private string[] assetFiles;

    [MenuItem("Tools/Asset Integrator")]
    public static void OpenWindow()
    {
        AssetIntegratorEditorWindow wnd = GetWindow<AssetIntegratorEditorWindow>();
        wnd.titleContent = new GUIContent("Asset Integrator");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        VisualElement uxml = m_VisualTreeAsset.Instantiate();
        root.Add(uxml);
        ObjectField configObjectField = root.Q<ObjectField>("configField");
        Button openFolderButton = root.Q<Button>("folderOpen");
        openFolderButton.SetEnabled(false);
        configObjectField.objectType = typeof(AssetValidationConfig);
        configObjectField.RegisterValueChangedCallback(evt =>
        {
            AssetIntegratorSession.CurrentConfig = (AssetValidationConfig)evt.newValue;
            openFolderButton.SetEnabled(evt.newValue != null);
        });
        
        folderPathLabel = root.Q<Label>("folderPath");
        assetListView  = root.Q<ListView>("assetListView");
        
        openFolderButton.clicked += OpenFolderButtonClicked;
    }

    private void OpenFolderButtonClicked()
    {
        string path = EditorUtility.OpenFolderPanel("Select Asset Folder", "", "");
        
        if (!string.IsNullOrEmpty(path))
        {
            folderPathLabel.text = path;
        }

        assetFiles = GetAssetFiles(path);
        assetListView.itemsSource = assetFiles;
        assetListView.makeItem = () =>
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            Label nameLabel = new Label();
            nameLabel.name = "nameLabel";

            Label statusLabel = new Label();
            statusLabel.name = "statusLabel";
            
            row.Add(nameLabel);
            row.Add(statusLabel);
            
            return row;
        };
        assetListView.bindItem = (element, i) =>
        {
            Label nameLabel = element.Q<Label>("nameLabel");
            Label statusLabel = element.Q<Label>("statusLabel");

            string fileName = assetFiles[i];
            string extension = Path.GetExtension(fileName);
            nameLabel.text = Path.GetFileNameWithoutExtension(fileName);
            statusLabel.text = IsValidName(nameLabel.text, extension) ? "Valid" : "Invalid";
        };
    }

    private string[] GetAssetFiles(string folderPath)
    {
        string[] allFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
        
        List<string> allowedExtensions = new List<string>();

        foreach (var ext in AssetIntegratorSession.CurrentConfig.rules)
        {
            allowedExtensions.AddRange(ext.extensions);
        }

        string[] assetFiles = allFiles
            .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
            .ToArray();
        
        return assetFiles;
    }

    private bool IsValidName(string name, string extension)
    {
        List<string> parts = name.Split('_').ToList();

        foreach (var ext in AssetIntegratorSession.CurrentConfig.rules)
        {
            if (ext.extensions.Contains(extension))
            {
                if (parts.Count < ext.minimalNamePart)
                {
                    return false;
                }

                string lastTexture = parts.Last();
                
                if (ext.acceptableSuffixes.Count == 0)
                {
                    return true;
                }
                
                if (!ext.acceptableSuffixes.Contains(lastTexture))
                {
                    return false;
                }
                return  true;
            }
        }
        return false;
    }
}
