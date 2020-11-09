using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;

public class OutlineFeature : ScriptableRendererFeature
{
    [Serializable]
    public class RenderSettings
    {
        public Material material = null;
        public int OverrideMaterialPassIndex = 0;
        [Space]
        public LayerMask layerMask = 0;
    }

    [Serializable]
    public class BlurSettings
    {
        public Material material;
        public int passesCount;
        public int downSample;
    }   

    [Serializable]
    public class OutlineSettings
    {
        public Material material;
    }

    [SerializeField] private string renderTextureName;
    [SerializeField] private string blurTextureName;
    [SerializeField] private RenderSettings renderSettings;
    [SerializeField] private BlurSettings blurSettings;
    [SerializeField] private OutlineSettings outlineSettings;
    [SerializeField] private RenderPassEvent passEvent;

    private RenderTargetHandle renderTexture;
    private RenderTargetHandle blurTexture;
    private OutlineRenderObjectsPass renderPass;
    private BlurPass blurPass;
    private OutlinePass outlinePass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(renderPass);
        renderer.EnqueuePass(blurPass);
        renderer.EnqueuePass(outlinePass);
    }

    public override void Create()
    {
        renderTexture.Init(renderTextureName);
        blurTexture.Init(blurTextureName);

        renderPass = new OutlineRenderObjectsPass(renderTexture, renderSettings.layerMask, renderSettings.material);
        renderPass.renderPassEvent = passEvent;

        blurPass = new BlurPass(renderTexture.Identifier(), blurTexture, blurSettings.material, blurSettings.passesCount, blurSettings.downSample);
        blurPass.renderPassEvent = passEvent;

        outlinePass = new OutlinePass(outlineSettings.material);
        outlinePass.renderPassEvent = passEvent;

    }
}
