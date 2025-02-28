using System.Linq;
using Godot;
using Godot.Collections;

namespace BlockFactory.scripts.block_library;

[GlobalClass]
public partial class FactoryBlockLibrary : VoxelBlockyTypeLibrary
{
    /// <summary>
    /// A wrapper around <see cref="VoxelBlockyTypeLibrary.GetModelIndexDefault"/> for world generation.
    /// </summary>
    public ulong GetDefaultId(StringName name)
    {
        return (ulong) GetModelIndexDefault(name);
    }

    public ulong GetSingleAttributeId(StringName typeName, StringName value)
    {
        return (ulong) GetModelIndexSingleAttribute(typeName, value);
    }
    
    public ulong GetModelWithAttributes(StringName typeName, Dictionary<string, int> value)
    {
        return (ulong) GetModelIndexWithAttributes(typeName, (Dictionary) value);
    }
    
    public VoxelData GetVoxelDataFromId(int id)
    {
        return new VoxelData(GetTypeNameAndAttributesFromModelIndex(id));
    }

    public VoxelBlockyType GetTypeFromId(int id)
    {
        var data = GetVoxelDataFromId(id);
        return GetTypeFromName(data.Name);
    }

    public VariantData[] GetVariants(StringName typeName)
    {
        var blockType = GetTypeFromName(typeName);
        return blockType._VariantModelsData.Select(v => new VariantData(v)).ToArray();
    }

    public static VariantData[] WrapVariantData(Array data)
    {
        return data.Select(v => new VariantData(v)).ToArray();
    }

    public struct VoxelData
    {
        public string Name;
        public Dictionary<string, int> Attributes = new();
        
        // Block structure: [name, {attributeDict}]
        public VoxelData(Array block)
        {
            Name = block[0].AsString();
            Attributes = block[1].AsGodotDictionary<string, int>();
        }
    }

    public struct VariantData
    {
        public Dictionary<string, int> Attributes = new();
        public VoxelBlockyModel Model;

        // Variant structure: [[name, id], model]
        public VariantData(Variant variantData)
        {
            var v = variantData.AsGodotArray();
            Model = v[1].As<VoxelBlockyModel>();

            // Wonky data structure of the original
            var attributes = v[0].AsGodotArray();
            for (var i = 0; i < attributes.Count; i += 2)
            {
                Attributes[attributes[0].AsString()] = attributes[1].AsInt32();
            }
        }
    }
}