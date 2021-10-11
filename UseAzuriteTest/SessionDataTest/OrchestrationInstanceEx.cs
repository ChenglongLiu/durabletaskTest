using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using DurableTask.Core;

namespace SessionDataTest
{
        public class OrchestrationInstanceEx : OrchestrationInstance
    {
        private static readonly DataContractSerializer s_customSerializer =
               new DataContractSerializer(typeof(OrchestrationInstanceEx));

        private static readonly DataContractSerializer s_defaultSerializer =
            new DataContractSerializer(typeof(OrchestrationInstance));


        [DataMember]
        public Dictionary<string, string> Dic = new Dictionary<string, string>();

        /// <summary>
        /// Creates a <see cref="OrchestrationInstanceEx"/> from the provided
        /// <see cref="OrchestrationRuntimeState"/>, using its extension data if available.
        /// </summary>
        /// <param name="runtimeState">The runtime state to create this instance from.</param>
        /// <returns>A new or deserialized instance.</returns>
        public static OrchestrationInstanceEx Initialize(OrchestrationRuntimeState runtimeState)
        {
            if (runtimeState == null)
            {
                throw new ArgumentNullException(nameof(runtimeState));
            }

            OrchestrationInstance instance = runtimeState.OrchestrationInstance;
            if (instance is OrchestrationInstanceEx custom)
            {
                return custom;
            }

            custom = Get(instance);
            return custom;
        }

        /// <summary>
        /// Gets a <see cref="OrchestrationInstanceEx"/> from a <see cref="OrchestrationInstance"/>,
        /// using its extension data if available.
        /// </summary>
        /// <param name="instance">The orchestration instance. Not null.</param>
        /// <returns>The custom orchestration instance.</returns>
        public static OrchestrationInstanceEx Get(OrchestrationInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (instance is OrchestrationInstanceEx custom)
            {
                return custom;
            }

            // We need to first get custom extension data by serializing & deserializing.
            using (var stream = new MemoryStream())
            {
                s_defaultSerializer.WriteObject(stream, instance);
                stream.Position = 0;
                return (OrchestrationInstanceEx)s_customSerializer.ReadObject(stream);
            }
        }
    }
}