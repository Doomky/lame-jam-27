using UnityEngine;

public static class MaterialHelpers
{
    private const string MaterialRootFolder = "Materials/";

    public enum MaterialType
    {
        OnGetHit,
    }

    public static Material GetMaterial<TMaterialEnum>(TMaterialEnum materialType)
    {
        return Resources.Load<Material>($"{MaterialRootFolder}{materialType}");
    }

    public static Material GetMaterial<TMaterialEnum>(string path, TMaterialEnum materialType)
    {
        return Resources.Load<Material>($"{MaterialRootFolder}{path}{materialType}");
    }
}
