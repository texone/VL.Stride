﻿using System;
using System.Collections.Generic;
using System.Text;
using Xenko.Shaders;
using Xenko.Core.Mathematics;
using Xenko.Rendering.Materials;
using System.Globalization;
using Xenko.Graphics;
using Buffer = Xenko.Graphics.Buffer;
using System.Linq;

namespace VL.Xenko.Shaders.ShaderFX
{
    public static class ShaderFXUtils
    {
        /// <summary>
        /// Declare a shader variable with a give name and initialize it with a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varName"></param>
        /// <param name="valueGetter"></param>
        /// <returns></returns>
        public static Var<T> DeclAndInitVar<T>(string varName, IComputeValue<T> valueGetter)
            => new Var<T>(valueGetter, varName);

        /// <summary>
        /// Assigns a new value to an existing shader variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existingVar"></param>
        /// <param name="valueGetter"></param>
        /// <returns></returns>
        public static Var<T> AssignVar<T>(this Var<T> existingVar, IComputeValue<T> valueGetter)
            => new Var<T>(valueGetter, existingVar);

        /// <summary>
        /// Retrieves the current value of an existing shader variable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existingVar"></param>
        /// <returns></returns>
        public static IComputeValue<T> GetVarValue<T>(this Var<T> existingVar)
            => new GetVar<T>(existingVar);

        public static ShaderClassSource GetShaderSourceForType<T>(string shaderName, params object[] genericArguments)
        {
            return new ShaderClassSource(shaderName + GetNameForType<T>(), genericArguments);
        }

        public static ShaderClassSource GetShaderSourceForType2<T1, T2>(string shaderName, params object[] genericArguments)
        {
            return new ShaderClassSource(shaderName + GetNameForType<T1>() + GetNameForType<T2>(), genericArguments);
        }

        public static ShaderClassSource GetShaderSourceFunkForType2<T1, T2>(string shaderName, params object[] genericArguments)
        {
            return new ShaderClassSource(shaderName + GetNameForType<T1>() + "To" + GetNameForType<T2>(), genericArguments);
        }

        public static ShaderClassSource GetShaderSourceForInputs<TOut>(string shaderName, IEnumerable<IComputeNode> inputs, params object[] genericArguments)
        {
            var inputTypeNames = inputs.Select(i => i.GetType())
                .Where(t => t.IsGenericType)
                .Select(t => t.GenericTypeArguments.Last())
                .Select(t => GetNameForType(t));

            var inputTypesString = string.Join("", inputTypeNames);
            return new ShaderClassSource(shaderName + inputTypesString + GetNameForType<TOut>(), genericArguments);
        }

        static Dictionary<Type, string> KnownTypes = new Dictionary<Type, string>();

        static ShaderFXUtils()
        {
            KnownTypes.Add(typeof(float), "Float");
            KnownTypes.Add(typeof(Vector2), "Float2");
            KnownTypes.Add(typeof(Vector3), "Float3");
            KnownTypes.Add(typeof(Vector4), "Float4");
            KnownTypes.Add(typeof(Matrix), "Matrix");
            KnownTypes.Add(typeof(int), "Int");
            KnownTypes.Add(typeof(Int2), "Int2");
            KnownTypes.Add(typeof(Int3), "Int3");
            KnownTypes.Add(typeof(Int4), "Int4");
            KnownTypes.Add(typeof(uint), "UInt");
            KnownTypes.Add(typeof(bool), "Bool");
            KnownTypes.Add(typeof(Buffer), "Buffer");
            KnownTypes.Add(typeof(Texture), "Texture");
        }

        public static string GetNameForType<T>()
        {
            return GetNameForType(typeof(T));        
        }

        public static string GetNameForType(Type t)
        {
            if (KnownTypes.TryGetValue(t, out var result))
                return result;

            throw new NotImplementedException("No name defined for type: " + t.Name);
        }

        public static ShaderMixinSource CreateMixin(this ShaderClassSource shaderClassSource)
        {
            var mixin = new ShaderMixinSource();
            mixin.Mixins.Add(shaderClassSource);
            return mixin;
        }

        public static ShaderSource AddComposition(this ShaderMixinSource mixin, IComputeNode compute, string compositionName, ShaderGeneratorContext context, MaterialComputeColorKeys keys)
        {
            var shaderSource = compute?.GenerateShaderSource(context, keys);
            if (shaderSource != null)
                mixin.AddComposition(compositionName, shaderSource);

            return shaderSource;
        }

        public static IEnumerable<IComputeNode> ReturnIfNotNull(params IComputeNode[] children)
        {
            foreach (var child in children)
                if (child != null)
                    yield return child;
        }

        public static IEnumerable<IComputeNode> ReturnIfNotNull(IEnumerable<IComputeNode> children)
        {
            foreach (var child in children)
                if (child != null)
                    yield return child;
        }

        public static string GetTexCoordSemantic(TextureCoordinate texcoord)
        {
            return "TEXCOORD" + GetTextureIndex(texcoord);
        }

        public static int GetTextureIndex(TextureCoordinate texcoord)
        {
            switch (texcoord)
            {
                case TextureCoordinate.Texcoord0:
                    return 0;
                case TextureCoordinate.Texcoord1:
                    return 1;
                case TextureCoordinate.Texcoord2:
                    return 2;
                case TextureCoordinate.Texcoord3:
                    return 3;
                case TextureCoordinate.Texcoord4:
                    return 4;
                case TextureCoordinate.Texcoord5:
                    return 5;
                case TextureCoordinate.Texcoord6:
                    return 6;
                case TextureCoordinate.Texcoord7:
                    return 7;
                case TextureCoordinate.Texcoord8:
                    return 8;
                case TextureCoordinate.Texcoord9:
                    return 9;
                case TextureCoordinate.TexcoordNone:
                default:
                    throw new ArgumentOutOfRangeException("texcoord");
            }
        }

        public static string GetAsShaderString(ColorChannel channel)
        {
            return channel.ToString().ToLowerInvariant();
        }

        public static string GetAsShaderString(Vector2 v)
        {
            return string.Format(CultureInfo.InvariantCulture, "float2({0}, {1})", v.X, v.Y);
        }

        public static string GetAsShaderString(Vector3 v)
        {
            return string.Format(CultureInfo.InvariantCulture, "float3({0}, {1}, {2})", v.X, v.Y, v.Z);
        }

        public static string GetAsShaderString(Vector4 v)
        {
            return string.Format(CultureInfo.InvariantCulture, "float4({0}, {1}, {2}, {3})", v.X, v.Y, v.Z, v.W);
        }

        public static string GetAsShaderString(Color4 c)
        {
            return string.Format(CultureInfo.InvariantCulture, "float4({0}, {1}, {2}, {3})", c.R, c.G, c.B, c.A);
        }

        public static string GetAsShaderString(float f)
        {
            return string.Format(CultureInfo.InvariantCulture, "float4({0}, {0}, {0}, {0})", f);
        }

        public static string GetAsShaderString(int f)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", f);
        }

        public static string GetAsShaderString(uint f)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", f);
        }

        public static string GetAsShaderString(object obj)
        {
            return obj.ToString();
        }

        /// <summary>
        /// Build a encapsulating ShaderMixinSource if necessary.
        /// </summary>
        /// <param name="shaderSource">The input ShaderSource.</param>
        /// <returns>A ShaderMixinSource</returns>
        public static ShaderMixinSource GetShaderMixinSource(ShaderSource shaderSource)
        {
            if (shaderSource is ShaderClassSource)
            {
                var mixin = new ShaderMixinSource();
                mixin.Mixins.Add((ShaderClassSource)shaderSource);
                return mixin;
            }
            if (shaderSource is ShaderMixinSource)
                return (ShaderMixinSource)shaderSource;

            return null;
        }
    }
}
