using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace StrangeAttractors
{
    public class AizawaAttractor : StrangeAttractor
    {
        [SerializeField, Tooltip("Default is 0.95")]
        float a = 0.95f;
        [SerializeField, Tooltip("Default is 0.7")]
        float b = 0.7f;
        [SerializeField, Tooltip("Default is 0.6")]
        float c = 0.6f;
        [SerializeField, Tooltip("Default is 3.5")]
        float d = 3.5f;
        [SerializeField, Tooltip("Default is 0.25")]
        float e = 0.25f;
        [SerializeField, Tooltip("Default is 0.1")]
        float f = 0.1f;

        private int aId, bId, cId, dId, eId, fId;
        private string aProp = "a", bProp = "b", cProp = "c", dProp = "d", eProp = "e", fProp = "f";

        protected sealed override void Initialize()
        {
            Assert.IsTrue(computeShader.name == "AizawaAttractor", "Please set AizawaAttractor.compute");
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
                var normalize = (float)i / instanceCount;
                var color = gradient.Evaluate(normalize);
                parameters[i] = new Params(new Vector3(Random.value * emitterSize, 0f, 0f), particleSize, color);
                //parameters[i] = new Params(UnityEngine.Random.insideUnitSphere * emitterSize, particleSize, color);
            }
            cBuffer.SetData(parameters);
        }

        protected override void InitializeShaderUniforms()
        {
            aId = Shader.PropertyToID(aProp);
            bId = Shader.PropertyToID(bProp);
            cId = Shader.PropertyToID(cProp);
            dId = Shader.PropertyToID(dProp);
            eId = Shader.PropertyToID(eProp);
            fId = Shader.PropertyToID(fProp);
        }

        protected override void UpdateShaderUniforms()
        {
            computeShaderInstance.SetFloat(aId, a);
            computeShaderInstance.SetFloat(bId, b);
            computeShaderInstance.SetFloat(cId, c);
            computeShaderInstance.SetFloat(dId, d);
            computeShaderInstance.SetFloat(eId, e);
            computeShaderInstance.SetFloat(fId, f);
        }
    }
}