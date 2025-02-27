using Godot;

namespace BlockFactory.scripts.blocks.interfaces;

public interface IProcessingBlock
{
    public void Process(FactoryVoxelTool vt, ulong blockId, Vector3I blockPos);
}