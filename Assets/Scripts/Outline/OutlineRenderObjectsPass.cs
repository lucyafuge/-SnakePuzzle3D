using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class OutlineRenderObjectsPass : ScriptableRenderPass
{
    private RenderTargetHandle targetHandle;
    private RenderTargetIdentifier depth;


    private List<ShaderTagId> shaderTagIds = new List<ShaderTagId>() { new ShaderTagId("UniversalForward") };
    private FilteringSettings filteringSettings;
    private RenderStateBlock renderStateBlock;

    private Material overrideMaterial;

    public OutlineRenderObjectsPass(RenderTargetHandle _targetHandle, int layerMask, Material material)
    {
        targetHandle = _targetHandle;
        filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
        renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

        overrideMaterial = material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIds, ref renderingData, sortingCriteria);
        drawingSettings.overrideMaterial = overrideMaterial;

        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        cmd.GetTemporaryRT(targetHandle.id, cameraTextureDescriptor);
        ConfigureTarget(targetHandle.Identifier());
        ConfigureClear(ClearFlag.All, Color.clear);
    }
}
