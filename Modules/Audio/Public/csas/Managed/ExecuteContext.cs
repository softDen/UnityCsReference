// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License


using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.UNode.Audio")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.UNode.Audio.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.Audio")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Unity.Audio.Tests")]

namespace Unity.Experimental.Audio
{
    internal unsafe struct ExecuteContext<TParams, TProvs>
        where TParams : struct, IConvertible
        where TProvs  : struct, IConvertible
    {
        public ulong DSPClock { get { return m_DSPClock; } }
        public uint DSPBufferSize { get { return m_DSPBufferSize; } }
        public uint SampleRate { get { return m_SampleRate; } }
        public void PostEvent<TNodeEvent>(TNodeEvent eventMsg) where TNodeEvent : struct
        {
            PostEvent(DSPGraph.GetTypeHashCode<TNodeEvent>(), eventMsg);
        }

        // FIXME: Temporarily exposed until BurstRuntime.GetHashCode64<T> has a Unity-core
        // equivalent, or DSPGraph's C# code is moved to a package.
        public void PostEvent<TNodeEvent>(long eventTypeHash, TNodeEvent eventMsg) where TNodeEvent : struct
        {
            ExecuteContextInternal.Internal_PostEvent(
                m_DSPNodePtr, eventTypeHash, UnsafeUtility.AddressOf(ref eventMsg),
                UnsafeUtility.SizeOf<TNodeEvent>());
        }

        internal ulong m_DSPClock;
        internal uint  m_DSPBufferSize;
        internal uint  m_SampleRate;
        internal void* m_DSPNodePtr;

        public SampleBufferArray               Inputs;
        public SampleBufferArray               Outputs;
        public ParameterData<TParams>          Parameters;
        public SampleProviderContainer<TProvs> Providers;
    }
}

