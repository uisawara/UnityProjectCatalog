using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnityProjectCatalogSettings", menuName = "Settings/Unity Project Catalog Settings")]
public sealed class UnityProjectCatalogSettings : ScriptableObject
{
    public List<string> excludedAssemblies = new();
}