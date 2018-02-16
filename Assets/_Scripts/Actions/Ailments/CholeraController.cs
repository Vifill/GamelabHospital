﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CholeraController : Actionable
{
    public ConditionConfig CurrentCondition { private set; get; }
    public List<ToolName> UsableTools;

    public override bool CanBeActioned(ToolName pCurrentTool, GameObject pObjectActioning)
    {
        if (UsableTools.Contains(pCurrentTool))
        {
            return true;
        }
        return false;
    }
}