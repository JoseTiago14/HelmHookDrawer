using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace yaml.parser
{

    public class YamlReader
    {
        public IReadOnlyCollection<Resource> Read(string yaml)
        {
            var deserializer = new DeserializerBuilder().Build();
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            var yamlParts = yaml.Split(new[] { "---\n" }, StringSplitOptions.None).Where(s => !string.IsNullOrWhiteSpace(s));
            var rawResources = yamlParts.Select(s =>
            {
                var r = new StringReader(s);
                var yamlObject = deserializer.Deserialize(r);

                var jsonRaw = serializer.Serialize(yamlObject ?? string.Empty);
                return JsonConvert.DeserializeObject<ResourceRaw>(jsonRaw);
            }).ToList();

            return rawResources.Where(r => Enum.TryParse(r.Kind, out KindType _)).Select(r =>
            {
                Enum.TryParse(r.Kind, true, out KindType parsedKind);
                long.TryParse(r.Metadata?.Annotations?.Weight ?? string.Empty, out var weightParsed);
                var parsedHooks = (r.Metadata?.Annotations?.Hook ?? string.Empty)
                    .Split(',')
                    .Select(h => h.Replace("-", ""))
                    .Where(h => Enum.TryParse(h, true, out HookType _))
                    .Select(h => (HookType)Enum.Parse(typeof(HookType), h, true))
                    .ToList();

                return new Resource(parsedKind, r.Metadata?.Name ?? string.Empty, 
                    r.Metadata?.Namespace ?? string.Empty, weightParsed,
                    r.Metadata?.Labels?.ChartName ?? string.Empty,
                    parsedHooks);
            }).ToList();
        }

        private class ResourceRaw
        {
            public string Kind { get; set; }
            public ResourceMetadata Metadata { get; set; }
        }

        private class ResourceMetadata
        {
            public string Name { get; set; }
            public string Namespace { get; set; }
            public MetadataAnnotations Annotations { get; set; }
            public Labels Labels { get; set; }
        }

        private class MetadataAnnotations
        {
            [JsonProperty("helm.sh/hook")]
            public string Hook { get; set; }
            [JsonProperty("helm.sh/hook-weight")]
            public string Weight { get; set; }
        }

        private class Labels
        {
            public string ChartName { get; set; }
        }
    }
}
