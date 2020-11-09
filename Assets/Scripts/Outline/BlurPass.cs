using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class BlurPass : ScriptableRenderPass
{
    enum BLUR
    {
        FIRST = 0,
        SECOND = 1
    };

    private int[] blurShadersIDs = new int[2] { Shader.PropertyToID("BlurShader_1"), Shader.PropertyToID("BlurShader_2") };
    private RenderTargetIdentifier[] blurRTs = new RenderTargetIdentifier[2];

    private RenderTargetIdentifier source;
    private RenderTargetHandle targetHandle;

    private Material blurMaterial;
    private int passesCount;
    private int downSample;

    public BlurPass(RenderTargetIdentifier _source, RenderTargetHandle _targetHandle, Material _blurMaterial, int _passesCount, int _downSample)
    {
        source = _source;
        targetHandle = _targetHandle;
        blurMaterial = _blurMaterial;
        passesCount = _passesCount;
        downSample = _downSample;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        var width = Mathf.Max(1, cameraTextureDescriptor.width >> downSample);
        var height = Math.Max(1, cameraTextureDescriptor.height >> downSample);
        var blurTextureDescriptor = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0, 0);

        for (int i = 0; i != blurRTs.Length; i++)
        {
            blurRTs[i] = new RenderTargetIdentifier(blurShadersIDs[i]);
            cmd.GetTemporaryRT(blurShadersIDs[i], blurTextureDescriptor, FilterMode.Bilinear);
        }
        cmd.GetTemporaryRT(targetHandle.id, blurTextureDescriptor, FilterMode.Bilinear);
        ConfigureTarget(targetHandle.Identifier());
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get("BlurPass");
        if (passesCount > 0)
        {
            cmd.Blit(source, blurRTs[(int)BLUR.FIRST], blurMaterial, 0);
            for (int i = 0; i < passesCount; i++)
            {
                cmd.Blit(blurRTs[(int)BLUR.FIRST], blurRTs[(int)BLUR.SECOND], blurMaterial, 0);
                var tmp = blurRTs[(int)BLUR.FIRST];
                blurRTs[(int)BLUR.FIRST] = blurRTs[(int)BLUR.SECOND];
                blurRTs[(int)BLUR.SECOND] = tmp;
            }
            cmd.Blit(blurRTs[(int)BLUR.FIRST], targetHandle.Identifier());

        }
        else
            cmd.Blit(source, targetHandle.Identifier());

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
