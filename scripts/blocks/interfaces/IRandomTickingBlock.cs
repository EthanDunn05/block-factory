using Godot;

namespace BlockFactory.scripts.blocks.interfaces;

public interface IRandomTickingBlock
{
    public void RandomTick(FactoryVoxelTool vt, ulong blockId, Vector3I blockPos);
}