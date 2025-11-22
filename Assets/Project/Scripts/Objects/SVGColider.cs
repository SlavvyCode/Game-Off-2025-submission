using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Presets;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class SVGCollider : MonoBehaviour {


    public GameObject gameObjectToColide;
    public string assetFileLocation, vectorSpritePreset, texturedSpritePreset;
    public TypeOfCollider type;
    private void Start()
    {
        AddCollider(gameObjectToColide, assetFileLocation, vectorSpritePreset, texturedSpritePreset, type);
    }
    public enum TypeOfCollider
    {
        BoxCollider,
        BoxCollider2D,
        CapsuleCollider,
        CircleCollider2D,
        CompositeCollider2D,
        CustomCollider2D,
        EdgeCollider2D,
        MeshCollider,
        PolygonCollider2D,
        SphereCollider,
        TerrainCollider,
        TilemapCollider2D,
        WheelCollider
    }

    /// <summary>
    /// Add a collider to an SVG asset.
    /// </summary>
    /// <param name="gameObject">The svg gameobject to which to add the collider</param>
    /// <param name="assetFileLocation">The location of the asset</param>
    /// <param name="VectorSpritePreset">The preset of the svg asset considered as a vector sprite</param>
    /// <param name="TexturedSpritePreset">The preset of the svg asset considered as a textured sprite (not vectorial image)</param>
    /// <param name="type">The type of the collider to add to the sprite</param>
    public static void AddCollider(GameObject gameObject, string assetFileLocation, string VectorSpritePreset, string TexturedSpritePreset, TypeOfCollider type)
    {

        if (!File.Exists(TexturedSpritePreset) || !File.Exists(VectorSpritePreset))
        {
            Debug.LogError("Missing preset files, aborting operation!");
            return;
        }

        if (!Path.GetExtension(assetFileLocation).Equals(".svg"))
        {
            Debug.LogError("Not an .svg file, aborting operation!");
            return;
        }

        Preset newAssetPreset = AssetDatabase.LoadAssetAtPath<Preset>(TexturedSpritePreset);
        Preset oldAssetPreset = AssetDatabase.LoadAssetAtPath<Preset>(VectorSpritePreset);

        var importer = AssetImporter.GetAtPath(assetFileLocation);
        if (importer != null && newAssetPreset.ApplyTo(importer))
        {
            AssetDatabase.WriteImportSettingsIfDirty(TexturedSpritePreset);
            Debug.Log($"Preset {Path.GetFileName(TexturedSpritePreset)} applied succesfully to asset {Path.GetFileName(assetFileLocation)}");
        }
        else
        {
            Debug.LogError($"Failed to apply preset {Path.GetFileName(TexturedSpritePreset)} to asset {Path.GetFileName(assetFileLocation)}, aborting operation!");
            return;
        }

        AssetDatabase.ImportAsset(assetFileLocation); // reimport the asset so unity sees that I changed the preset

        switch (type)
        {
            case TypeOfCollider.BoxCollider: gameObject.AddComponent<BoxCollider>(); break;
            case TypeOfCollider.BoxCollider2D: gameObject.AddComponent<BoxCollider2D>(); break;
            case TypeOfCollider.CapsuleCollider: gameObject.AddComponent<CapsuleCollider>(); break;
            case TypeOfCollider.CircleCollider2D: gameObject.AddComponent<CircleCollider2D>(); break;
            case TypeOfCollider.CompositeCollider2D: gameObject.AddComponent<CompositeCollider2D>(); break;
            case TypeOfCollider.CustomCollider2D: gameObject.AddComponent<CustomCollider2D>(); break;
            case TypeOfCollider.EdgeCollider2D: gameObject.AddComponent<EdgeCollider2D>(); break;
            case TypeOfCollider.MeshCollider: gameObject.AddComponent<MeshCollider>(); break;
            case TypeOfCollider.PolygonCollider2D: gameObject.AddComponent<PolygonCollider2D>(); break;
            case TypeOfCollider.SphereCollider: gameObject.AddComponent<SphereCollider>(); break;
            case TypeOfCollider.TerrainCollider: gameObject.AddComponent<TerrainCollider>(); break;
            case TypeOfCollider.TilemapCollider2D: gameObject.AddComponent<TilemapCollider2D>(); break;
            case TypeOfCollider.WheelCollider: gameObject.AddComponent<WheelCollider>(); break;
            default:
                Debug.LogError("Wrong collider!");
                break;
        }

        Debug.Log($"{type} succesfully added to {gameObject.name}, reverting to preset {Path.GetFileName(VectorSpritePreset)}");
        if (importer != null && oldAssetPreset.ApplyTo(importer))
        {
            AssetDatabase.WriteImportSettingsIfDirty(VectorSpritePreset);
            Debug.Log($"Preset {Path.GetFileName(VectorSpritePreset)} applied succesfully to asset {Path.GetFileName(assetFileLocation)}");
        }
        else
        {
            Debug.LogError($"Failed to apply preset {Path.GetFileName(TexturedSpritePreset)} to asset {Path.GetFileName(assetFileLocation)}, manually update it");
            return;
        }

        AssetDatabase.ImportAsset(assetFileLocation); // reimport the asset so unity sees that I changed the preset
    }
}