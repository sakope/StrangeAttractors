using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace StrangeAttractors
{
    public class JapaneseAttractor : StrangeAttractor
    {
        [SerializeField, Tooltip("Default is 0.1f")]
        float k = 0.1f;
        [SerializeField, Tooltip("Default is 12.0f")]
        float b = 12.0f;

        private int kId, bId;
        private string kProp = "k", bProp = "b";

        protected sealed override void Initialize()
        {
            Assert.IsTrue(computeShader.name == "JapaneseAttractor", "Please set JapaneseAttractor.compute");
            base.Initialize();
        }

        protected sealed override void InitializeComputeBuffer()
        {
            if (cBuffer != null)
                cBuffer.Release();

            cBuffer = new ComputeBuffer(instanceCount, Marshal.SizeOf(typeof(Params)));
            Params[] parameters = new Params[cBuffer.count];
            for (int i = 0; i < instanceCount; i++)
            {
                Vector3 r = Random.insideUnitCircle;
                var color = gradient.Evaluate(r.magnitude);
                //parameters[i] = new Params(new Vector3(Random.value * emitterSize, 0f, 0f), particleSize, color);
                //parameters[i] = new Params(new Vector3(Random.value * emitterSize, Random.value * emitterSize, 0f), particleSize, color);
                parameters[i] = new Params(r * emitterSize, particleSize, color);
                //parameters[i] = new Params(Random.insideUnitSphere * emitterSize, particleSize, color);
            }
            cBuffer.SetData(parameters);
        }

        protected override void InitializeShaderUniforms()
        {
            kId    = Shader.PropertyToID(kProp);
            bId    = Shader.PropertyToID(bProp);
        }

        protected override void UpdateShaderUniforms()
        {
            computeShaderInstance.SetFloat(kId, k);
            computeShaderInstance.SetFloat(bId, b);
        }
    }
}