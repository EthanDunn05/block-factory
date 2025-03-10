﻿using System.Collections.Generic;
using Godot;

namespace BlockFactory.scripts.blocks.attributes;

[Tool]
public abstract partial class FactoryAttribute : VoxelBlockyAttributeCustom
{
    public abstract new string AttributeName { get; }
    public abstract new int DefaultValue { get; }
    public abstract string[] Values { get; }
    
    public FactoryAttribute()
    {
        base.AttributeName = AttributeName;
        base.DefaultValue = DefaultValue;
        ValueCount = Values.Length;

        for (var i = 0; i < ValueCount; i++)
        {
            SetValueName(i, Values[i]);
        }
    }
}