﻿using System.Collections.Generic;
using Latios.Kinemation;
using Unity.Entities;

namespace DOTSAnimation
{
    public partial struct AnimationState
    {
        private void Update_LinearBlend(float dt, ref DynamicBuffer<ClipSampler> samplers)
        {
            LinearBlendSampling.UpdateSamplers(ref samplers, StartSamplerIndex, EndSamplerIndex, Blend, dt);
        }
        
        private readonly BoneTransform SampleBone_LinearBlend(int boneIndex, float timeShift, in DynamicBuffer<ClipSampler> samplers)
        {
            return LinearBlendSampling.SampleBone(boneIndex, timeShift, samplers, StartSamplerIndex, EndSamplerIndex);
        }

        private readonly void SamplePose_LinearBlend(ref BufferPoseBlender blender, float timeShift, in DynamicBuffer<ClipSampler> samplers, float blend = 1f)
        {
            LinearBlendSampling.SamplePose(ref blender, timeShift, samplers, StartSamplerIndex, EndSamplerIndex, blend);
        }

        private readonly float NormalizedTime_LinearBlend(in DynamicBuffer<ClipSampler> samplers)
        { 
            LinearBlendSampling.FindSamplers(samplers, StartSamplerIndex, EndSamplerIndex, Blend, out var firstSamplerIndex, out var secondSamplerIndex);
            var duration = LinearBlendSampling.GetBlendDuration(samplers, firstSamplerIndex, secondSamplerIndex);
            var time = LinearBlendSampling.GetBlendTime(samplers, firstSamplerIndex, secondSamplerIndex);
            return AnimationUtils.CalculateNormalizedTime(time, duration);
        }

        private void ResetTime_LinearBlend(ref DynamicBuffer<ClipSampler> samplers)
        {
            for (var i = StartSamplerIndex; i <= EndSamplerIndex; i++)
            {
                var s = samplers[i];
                s.Time = 0;
                samplers[i] = s;
            }
        }

        private float Time_LinearBlend(in DynamicBuffer<ClipSampler> samplers)
        {
            LinearBlendSampling.FindSamplers(samplers, StartSamplerIndex, EndSamplerIndex, Blend, out var firstSamplerIndex, out var secondSamplerIndex);
            return LinearBlendSampling.GetBlendTime(samplers, firstSamplerIndex, secondSamplerIndex);
        }
        
        private readonly int GetActiveSamplerIndex_LinearBlend(DynamicBuffer<ClipSampler> samplers)
        {
            var maxWeightIndex = -1;
            var maxWeight = -1.0f;
            for (var i = StartSamplerIndex; i <= EndSamplerIndex; i++)
            {
                if (samplers[i].Weight > maxWeight)
                {
                    maxWeight = samplers[i].Weight;
                    maxWeightIndex = i;
                }
            }
            return maxWeightIndex;
        }
    }
}
