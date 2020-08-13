using System;
using System.Collections.Generic;
using System.Text;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Materials;
using Stride.Shaders;
using Buffer = Stride.Graphics.Buffer;
using Stride.Core.Mathematics;
using static VL.Stride.Shaders.ShaderFX.ShaderFXUtils;


namespace VL.Stride.Shaders.ShaderFX
{
    public class SampleTexture<T> : ComputeValue<T>
    {

        public SampleTexture(DeclTexture texture, DeclSampler sampler, IComputeValue<Vector2> texCoord, bool isRW = false)
        {
            TextureDecl = texture;
            SamplerDecl = sampler;
            TexCd = texCoord;
            IsRW = isRW;

            ShaderName = IsRW ? "SampleTextureRW" : "SampleTexture";
        }

        public DeclTexture TextureDecl { get; }

        public DeclSampler SamplerDecl { get; }

        public IComputeValue<Vector2> TexCd { get; }

        public bool IsRW { get; }

        public override ShaderSource GenerateShaderSource(ShaderGeneratorContext context, MaterialComputeColorKeys baseKeys)
        {
            if (TextureDecl == null || SamplerDecl == null)
                return GetShaderSourceForType<T>("Compute");

            TextureDecl.GenerateShaderSource(context, baseKeys);
            SamplerDecl.GenerateShaderSource(context, baseKeys);

            var shaderClassSource = GetShaderSourceForType<T>(ShaderName, TextureDecl.Key, TextureDecl.GetResourceGroupName(context), SamplerDecl.Key, SamplerDecl.GetResourceGroupName(context));

            if (TexCd != null)
            {
                var mixin = shaderClassSource.CreateMixin();
                mixin.AddComposition(TexCd, "TexCd", context, baseKeys);
                return mixin;
            }
            else
                return shaderClassSource;
        }

        public override IEnumerable<IComputeNode> GetChildren(object context = null)
        {
            return ReturnIfNotNull(TextureDecl, TexCd);
        }
    }
}
