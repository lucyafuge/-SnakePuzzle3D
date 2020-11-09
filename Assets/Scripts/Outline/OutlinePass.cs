using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class OutlinePass : ScriptableRenderPass
{
    private Material material;

    public OutlinePass(Material _material)
    {
        material = _material;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get("OutlinePass");

        using (new ProfilingSample(cmd, "OutlinePass"))
        {
            var mesh = RenderingUtils.fullscreenMesh;
            cmd.DrawMesh(mesh, Matrix4x4.identity, material, 0, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
