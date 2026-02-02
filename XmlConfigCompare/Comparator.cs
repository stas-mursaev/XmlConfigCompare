using System.Text;
using System.Xml.Linq;

namespace XmlConfigCompare
{
    internal class Comparator
    {
        private readonly string[] defaultKeyAttributes = ["name", "key", "ref", "path", "assembly", "path", "fileextension"];

        private readonly string[] keyAttributes;
        private readonly string[] ignorePaths;

        public Comparator(string[]? keyAttributes, string[]? ignorePaths)
        {
            this.keyAttributes = keyAttributes?.Select(a => a.ToLower()).ToArray() ?? defaultKeyAttributes;
            this.ignorePaths = ignorePaths ?? [];
        }

        private void ProcessNode(ref Dictionary<string, NodeInfo> values, string parentName, XElement node)
        {
            var nodeName = parentName + "\\" + node.Name.LocalName.ToLower();

            var nodeAttributes = node.Attributes().ToDictionary(a => a.Name.LocalName.ToLower(), a => a.Value);

            var foundKeyAttributes = nodeAttributes
                .Where(a => keyAttributes.Contains(a.Key))
                .OrderBy(a => a.Key)
                .ToArray();

            var foundKeyAttributesFormatted = new List<string>();
            foreach (var attribute in foundKeyAttributes)
            {
                foundKeyAttributesFormatted.Add($"{attribute.Key}={attribute.Value.ToLower()}");
                nodeAttributes.Remove(attribute.Key);
            }
            if (foundKeyAttributesFormatted.Count != 0)
                nodeName += $"[{string.Join(",", foundKeyAttributesFormatted)}]";

            foreach (var path in ignorePaths)
            {
                if (nodeName.StartsWith(path))
                    return;
            }

            if (values.TryGetValue(nodeName, out var nodeInfo))
            {
                nodeInfo.Attributes.Add(nodeAttributes);
            }
            else
            {
                values.Add(nodeName, new NodeInfo()
                {
                    Value = node.Value,
                    Attributes = [nodeAttributes]
                });
            }

            foreach (var subNode in node.Elements())
                ProcessNode(ref values, nodeName, subNode);
        }

        public Dictionary<string, NodeInfo> XMLToDictionary(string filename)
        {
            var root = XElement.Load(filename);
            var values = new Dictionary<string, NodeInfo>();
            ProcessNode(ref values, "", root);
            return values;
        }

        private static string TrimSpaces(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            value = value.Trim();
            while (value.Contains("  "))
                value = value.Replace("  ", " ");
            return value;
        }

        private static bool TrimmedEquals(string value1, string value2)
        {
            return string.Equals(TrimSpaces(value1), TrimSpaces(value2));
        }

        private decimal CalculateSimilarity(Dictionary<string, string> attrs1, Dictionary<string, string> attrs2)
        {
            if (attrs1.Count == 0 && attrs2.Count == 0)
                return 1;

            var allKeys = attrs1.Keys.Union(attrs2.Keys).Where(k => !keyAttributes.Contains(k)).ToList();
            if (allKeys.Count == 0)
                return 1;

            var matchCount = 0;
            foreach (var key in allKeys)
            {
                if (attrs1.TryGetValue(key, out var val1) && attrs2.TryGetValue(key, out var val2) && TrimmedEquals(val1, val2))
                    matchCount++;
            }

            return (decimal)matchCount / allKeys.Count;
        }

        private List<CompareAttrResult> GetAttrDifferences(Dictionary<string, string> attrs1, Dictionary<string, string> attrs2)
        {
            var differences = new List<CompareAttrResult>();

            var allKeys = attrs1.Keys.Union(attrs2.Keys).Where(k => !keyAttributes.Contains(k));

            foreach (var key in allKeys)
            {
                var has1 = attrs1.TryGetValue(key, out var val1);
                var has2 = attrs2.TryGetValue(key, out var val2);

                if (has1 && has2)
                {
                    if (!TrimmedEquals(val1!, val2!))
                    {
                        differences.Add(new CompareAttrResult
                        {
                            Attribute = key,
                            ValueInConfig1 = TrimSpaces(val1!),
                            ValueInConfig2 = TrimSpaces(val2!)
                        });
                    }
                }
                else if (has1)
                {
                    differences.Add(new CompareAttrResult
                    {
                        Attribute = key,
                        ValueInConfig1 = TrimSpaces(val1!),
                        ValueInConfig2 = null
                    });
                }
                else
                {
                    differences.Add(new CompareAttrResult
                    {
                        Attribute = key,
                        ValueInConfig1 = null,
                        ValueInConfig2 = TrimSpaces(val2!)
                    });
                }
            }

            return differences;
        }

        private NonUniqueCompareResult CompareNonUniqueNodes(List<Dictionary<string, string>> attrs1, List<Dictionary<string, string>> attrs2)
        {
            const decimal similarityThreshold = 0.5M;
            const decimal ambiguityTolerance = 0.01M; // similarity difference threshold for ambiguity detection

            var result = new NonUniqueCompareResult
            {
                Count1 = attrs1.Count,
                Count2 = attrs2.Count,
                Matched = [],
                OnlyInConfig1 = [],
                OnlyInConfig2 = [],
                Ambiguous = []
            };

            var used1 = new HashSet<int>();
            var used2 = new HashSet<int>();

            // calculate similarity matrix
            var similarities = new List<(int idx1, int idx2, decimal similarity)>();
            for (var i = 0; i < attrs1.Count; i++)
            {
                for (var j = 0; j < attrs2.Count; j++)
                {
                    var sim = CalculateSimilarity(attrs1[i], attrs2[j]);
                    if (sim >= similarityThreshold)
                        similarities.Add((i, j, sim));
                }
            }

            similarities = similarities.OrderByDescending(s => s.similarity).ToList();

            // greedy matching algorithm
            foreach (var (idx1, idx2, similarity) in similarities)
            {
                if (used1.Contains(idx1) || used2.Contains(idx2))
                    continue;

                // are there other candidates with similar scores
                var otherCandidates = similarities
                    .Where(s => s.idx1 == idx1 && !used2.Contains(s.idx2) && s.idx2 != idx2)
                    .Where(s => Math.Abs(s.similarity - similarity) <= ambiguityTolerance)
                    .Select(s => (attrs2[s.idx2], s.similarity))
                    .ToList();

                if (otherCandidates.Count > 0)
                {
                    // ambiguous, add all candidates
                    otherCandidates.Insert(0, (attrs2[idx2], similarity));
                    result.Ambiguous.Add(new NonUniqueNodeAmbiguous
                    {
                        NodeAttrs = attrs1[idx1],
                        Candidates = otherCandidates
                    });
                    used1.Add(idx1);
                    // mark all candidates as used
                    foreach (var candidate in otherCandidates)
                    {
                        var candIdx = attrs2.IndexOf(candidate.Item1);
                        if (candIdx >= 0)
                            used2.Add(candIdx);
                    }
                }
                else
                {
                    // unique
                    result.Matched.Add(new NonUniqueNodeMatch
                    {
                        Node1Attrs = attrs1[idx1],
                        Node2Attrs = attrs2[idx2],
                        Differences = GetAttrDifferences(attrs1[idx1], attrs2[idx2]),
                        SimilarityPercent = similarity * 100
                    });
                    used1.Add(idx1);
                    used2.Add(idx2);
                }
            }

            // remaining unmatched nodes
            for (var i = 0; i < attrs1.Count; i++)
            {
                if (!used1.Contains(i))
                    result.OnlyInConfig1.Add(attrs1[i]);
            }

            for (var j = 0; j < attrs2.Count; j++)
            {
                if (!used2.Contains(j))
                    result.OnlyInConfig2.Add(attrs2[j]);
            }

            return result;
        }

        public CompareResult Compare(Dictionary<string, NodeInfo> config1, Dictionary<string, NodeInfo> config2)
        {
            var result = new CompareResult
            {
                OnlyInConfig1 = config1.Keys.Except(config2.Keys).ToList(),
                MissedInConfig1 = config2.Keys.Except(config1.Keys).ToList(),
                ChangedInConfig1 = [],
                Changes = [],
                NonUniqueNodes = []
            };

            // Logic for comparing two nodes with unique names
            var nodes1 = config1.Where(n => n.Value.Attributes.Count == 1).ToDictionary();
            var nodes2 = config2.Where(n => n.Value.Attributes.Count == 1).ToDictionary();

            foreach (var key in nodes1.Where(kvp => nodes1.Keys.Intersect(nodes2.Keys).Contains(kvp.Key)).Select(kvp => kvp.Key))
            {
                var vp = nodes1[key].Attributes[0];
                var vu = nodes2[key].Attributes[0];

                var attrOnlyInConfig1 = vp.Where(kvp => vp.Keys.Except(vu.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                    {
                        Attribute = attrKey,
                        ValueInConfig1 = TrimSpaces(vp[attrKey]),
                        ValueInConfig2 = null
                    }).ToList();

                var attrMissedInConfig1 = vu.Where(kvp => vu.Keys.Except(vp.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                    {
                        Attribute = attrKey,
                        ValueInConfig1 = null,
                        ValueInConfig2 = TrimSpaces(vu[attrKey])
                    }).ToList();

                var attrBothCompared = vp.Where(kvp => vp.Keys.Intersect(vu.Keys)
                    .Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey) && !TrimmedEquals(vp[attrKey], vu[attrKey]))
                    .Select(attrKey => new CompareAttrResult
                    {
                        Attribute = attrKey,
                        ValueInConfig1 = TrimSpaces(vp[attrKey]),
                        ValueInConfig2 = TrimSpaces(vu[attrKey])
                    }).ToList();

                if (attrOnlyInConfig1.Count != 0 || attrMissedInConfig1.Count != 0 || attrBothCompared.Count != 0)
                {
                    if (!result.ChangedInConfig1.Contains(key))
                    {
                        result.ChangedInConfig1.Add(key);
                        result.Changes.Add(key, []);
                    }

                    result.Changes[key].AddRange(attrOnlyInConfig1);
                    result.Changes[key].AddRange(attrMissedInConfig1);
                    result.Changes[key].AddRange(attrBothCompared);
                }
            }

            // Logic for comparing two nodes with non-unique names

            var nuNodes1 = config1.Where(n => n.Value.Attributes.Count > 1).ToDictionary();
            var nuNodes2 = config2.Where(n => n.Value.Attributes.Count > 1).ToDictionary();
            var nuKeys = nuNodes1.Keys.Union(nuNodes2.Keys).ToArray();

            foreach (var key in nuKeys)
            {
                var attrs1 = nuNodes1.TryGetValue(key, out var node1) ? node1.Attributes : [];
                var attrs2 = nuNodes2.TryGetValue(key, out var node2) ? node2.Attributes : [];

                var nuResult = CompareNonUniqueNodes(attrs1, attrs2);

                // Only add if there's something to report
                if (nuResult.Matched.Any(m => m.Differences.Count > 0) ||
                    nuResult.OnlyInConfig1.Count > 0 ||
                    nuResult.OnlyInConfig2.Count > 0 ||
                    nuResult.Ambiguous.Count > 0)
                {
                    result.NonUniqueNodes.Add(key, nuResult);
                }
            }

            return result;
        }

        private string GenerateNodeReport(NodeInfo node)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(node.Value))
                sb.AppendFormat("\tNode value = {0}\n", node.Value);
            foreach (var attr in node.Attributes.FirstOrDefault()?.Where(attr => !keyAttributes.Contains(attr.Key)) ?? [])
                sb.AppendFormat("\t{0}=\"{1}\"\n", attr.Key, attr.Value);
            if (node.Attributes.Count != 0)
                sb.Append('\n');
            return sb.ToString();
        }

        public string GenerateReport(Dictionary<string, NodeInfo> keysConfig1, Dictionary<string, NodeInfo> keysConfig2, CompareResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\n## Nodes that are present only in config 1 file\n");
            foreach (var node in result.OnlyInConfig1)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysConfig1[node]));
            }

            sb.AppendLine("\n## Nodes that are present only in config 2 file\n");
            foreach (var node in result.MissedInConfig1)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysConfig2[node]));
            }

            sb.AppendLine("\n## Nodes that are present in both config files but with different attributes\n");
            foreach (var node in result.ChangedInConfig1)
            {
                sb.AppendLine(node);

                if (!string.IsNullOrEmpty(keysConfig2[node].Value))
                    sb.AppendFormat("\tNode value = {0}\n", keysConfig2[node].Value);
                foreach (var change in result.Changes[node])
                    sb.AppendFormat("\t(1) {0}=\"{1}\"\n\t(2) {0}=\"{2}\"\n", change.Attribute, change.ValueInConfig1, change.ValueInConfig2);
                if (result.Changes[node].Count != 0)
                    sb.Append('\n');
            }

            // Non-unique nodes report
            if (result.NonUniqueNodes.Count > 0)
            {
                sb.AppendLine("\n## Non-unique nodes (multiple instances with the same path)\n");

                foreach (var (nodePath, nuResult) in result.NonUniqueNodes)
                {
                    sb.AppendLine($"{nodePath}");
                    sb.AppendLine($"\tConfig1: {nuResult.Count1} instance(s), Config2: {nuResult.Count2} instance(s)");
                    sb.AppendLine();

                    // Matched with differences
                    var matchedWithDiffs = nuResult.Matched.Where(m => m.Differences.Count > 0).ToList();
                    if (matchedWithDiffs.Count > 0)
                    {
                        sb.AppendLine("\t[Matched with differences]");
                        foreach (var match in matchedWithDiffs)
                        {
                            sb.AppendLine($"\t  Similarity: {match.SimilarityPercent:F0}%");
                            sb.AppendLine($"\t  Node: {FormatAttrs(match.Node1Attrs)}");
                            foreach (var diff in match.Differences)
                            {
                                sb.AppendLine($"\t    (1) {diff.Attribute}=\"{diff.ValueInConfig1}\"");
                                sb.AppendLine($"\t    (2) {diff.Attribute}=\"{diff.ValueInConfig2}\"");
                            }
                            sb.AppendLine();
                        }
                    }

                    // Only in config1
                    if (nuResult.OnlyInConfig1.Count > 0)
                    {
                        sb.AppendLine("\t[Only in Config1]");
                        foreach (var attrs in nuResult.OnlyInConfig1)
                            sb.AppendLine($"\t  {FormatAttrs(attrs)}");
                        sb.AppendLine();
                    }

                    // Only in config2
                    if (nuResult.OnlyInConfig2.Count > 0)
                    {
                        sb.AppendLine("\t[Only in Config2]");
                        foreach (var attrs in nuResult.OnlyInConfig2)
                            sb.AppendLine($"\t  {FormatAttrs(attrs)}");
                        sb.AppendLine();
                    }

                    // Ambiguous
                    if (nuResult.Ambiguous.Count > 0)
                    {
                        sb.AppendLine("\t[Ambiguous matches]");
                        foreach (var amb in nuResult.Ambiguous)
                        {
                            sb.AppendLine($"\t  Config1 node: {FormatAttrs(amb.NodeAttrs)}");
                            sb.AppendLine("\t  Candidates in Config2:");
                            foreach (var (candidate, similarity) in amb.Candidates)
                                sb.AppendLine($"\t    - {FormatAttrs(candidate)} (similarity: {similarity * 100:F0}%)");
                            sb.AppendLine();
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private static string FormatAttrs(Dictionary<string, string> attrs)
        {
            if (attrs.Count == 0)
                return "(no attributes)";
            return string.Join(", ", attrs.Select(kvp => $"{kvp.Key}=\"{kvp.Value}\""));
        }
    }

    internal class NodeInfo
    {
        public required string Value { get; set; }
        public required List<Dictionary<string, string>> Attributes { get; set; }
    }

    internal class CompareAttrResult
    {
        public required string Attribute { get; set; }
        public string? ValueInConfig1 { get; set; }
        public string? ValueInConfig2 { get; set; }
    }

    internal class NonUniqueNodeMatch
    {
        public required Dictionary<string, string> Node1Attrs { get; set; }
        public required Dictionary<string, string> Node2Attrs { get; set; }
        public required List<CompareAttrResult> Differences { get; set; }
        public required decimal SimilarityPercent { get; set; }
    }

    internal class NonUniqueNodeAmbiguous
    {
        public required Dictionary<string, string> NodeAttrs { get; set; }
        public required List<(Dictionary<string, string> Candidate, decimal SimilarityPercent)> Candidates { get; set; }
    }

    internal class NonUniqueCompareResult
    {
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public required List<NonUniqueNodeMatch> Matched { get; set; }
        public required List<Dictionary<string, string>> OnlyInConfig1 { get; set; }
        public required List<Dictionary<string, string>> OnlyInConfig2 { get; set; }
        public required List<NonUniqueNodeAmbiguous> Ambiguous { get; set; }
    }

    internal class CompareResult
    {
        public required List<string> MissedInConfig1 { get; set; }
        public required List<string> OnlyInConfig1 { get; set; }
        public required List<string> ChangedInConfig1 { get; set; }
        public required Dictionary<string, List<CompareAttrResult>> Changes { get; set; }
        public required Dictionary<string, NonUniqueCompareResult> NonUniqueNodes { get; set; }
    }
}
