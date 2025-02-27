using BlockFactory.scripts.block_library;
using BlockFactory.scripts.blocks.interfaces;
using Godot;

namespace BlockFactory.scripts.blocks;

public partial class PlantBlock : BaseFactoryBlock, ILodBlock
{
    public override void OnPlace(FactoryVoxelTool vt, ulong id, Vector3I position)
    {
        
    }
}