﻿namespace XmlConfigCompare
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Text;
    using System.Windows.Forms;

    public partial class FrmXmlConfigCompare : Form
    {
        public FrmXmlConfigCompare()
        {
            InitializeComponent();
        }

        private void btnSelectConfig1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbConfig1.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbConfig1.Text = openFileDialog1.FileName;
        }

        private void btnSelectConfig2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbConfig2.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbConfig2.Text = openFileDialog1.FileName;
        }

        private void btnSelectConfig3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbConfig3.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbConfig3.Text = openFileDialog1.FileName;
        }

        private void btnSelectResult_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = tbResult.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                tbResult.Text = saveFileDialog1.FileName;
        }

        readonly List<string> keyAttributes = new List<string> { "name", "key", "ref", "path", "assembly", "path", "fileextension" };

        private class NodeInfo
        {
            //public string Name { get; set; }
            public string Value { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
        }

        private class CompareAttrResult
        {
            public string Attribute { get; set; }
            public string ValueInConfig1 { get; set; }
            public string ValueInConfig2 { get; set; }
        }

        private class CompareResult
        {
            public List<string> MissedInConfig1 { get; set; }
            public List<string> OnlyInConfig1 { get; set; }
            public List<string> ChangedInConfig1 { get; set; }
            public Dictionary<string, List<CompareAttrResult>> Changes { get; set; }
        }

        private void ProcessNode(ref Dictionary<string, NodeInfo> values, string parentName, XElement node)
        {
            string keyAttr = null;
            string keyValue = null;
            if (node.FirstAttribute != null && keyAttributes.Contains(node.FirstAttribute.Name.LocalName.ToLower()))
            {
                keyAttr = node.FirstAttribute.Name.LocalName.ToLower();
                keyValue = node.FirstAttribute.Value.ToLower();
            }

            var name = parentName + "\\" + node.Name.LocalName.ToLower();
            if (!string.IsNullOrEmpty(keyAttr))
                name = name + string.Format("[{0}={1}]", keyAttr, keyValue);

            if (!values.ContainsKey(name)) values.Add(name, new NodeInfo());

            //values[name].Name = node.Name.LocalName;
            values[name].Value = node.Value;
            values[name].Attributes = node.Attributes().ToDictionary(a => a.Name.LocalName.ToLower(), a => a.Value);

            foreach (var subNode in node.Elements())
                ProcessNode(ref values, name, subNode);
        }

        private Dictionary<string, NodeInfo> XMLToDictionary(string filename)
        {
            var root = XElement.Load(filename);
            var values = new Dictionary<string, NodeInfo>();
            ProcessNode(ref values, "", root);
            return values;
        }

        private string TrimSpaces(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = value.Trim();
            while (value.Contains("  "))
                value = value.Replace("  ", " ");
            return value;
        }

        private bool Equals(string value1, string value2)
        {
            return string.Equals(TrimSpaces(value1), TrimSpaces(value2));
        }

        private CompareResult Compare(Dictionary<string, NodeInfo> config1, Dictionary<string, NodeInfo> config2)
        {
            var result = new CompareResult
            {
                OnlyInConfig1 = config1.Keys.Except(config2.Keys).ToList(),
                MissedInConfig1 = config2.Keys.Except(config1.Keys).ToList(),
                ChangedInConfig1 = new List<string>(),
                Changes = new Dictionary<string, List<CompareAttrResult>>()
            };

            foreach (var key in config1.Where(kvp => config1.Keys.Intersect(config2.Keys).Contains(kvp.Key)).Select(kvp => kvp.Key))
            {
                var vp = config1[key].Attributes;
                var vu = config2[key].Attributes;

                var attrOnlyInConfig1 = (vp.Where(kvp => vp.Keys.Except(vu.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueInConfig1 = TrimSpaces(vp[attrKey]),
                            ValueInConfig2 = null
                        })).ToList();
                
                var attrMissedInConfig1 = (vu.Where(kvp => vu.Keys.Except(vp.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueInConfig1 = null,
                            ValueInConfig2 = TrimSpaces(vu[attrKey])
                        })).ToList();

                var attrBothCompared = (vp.Where(kvp => vp.Keys.Intersect(vu.Keys)
                    .Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey) && !Equals(vp[attrKey], vu[attrKey]))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueInConfig1 = TrimSpaces(vp[attrKey]),
                            ValueInConfig2 = TrimSpaces(vu[attrKey])
                        })).ToList();
                
                if (attrOnlyInConfig1.Any() || attrMissedInConfig1.Any() || attrBothCompared.Any())
                {
                    if (!result.ChangedInConfig1.Contains(key))
                    {
                        result.ChangedInConfig1.Add(key);
                        result.Changes.Add(key, new List<CompareAttrResult>());
                    }

                    result.Changes[key].AddRange(attrOnlyInConfig1);
                    result.Changes[key].AddRange(attrMissedInConfig1);
                    result.Changes[key].AddRange(attrBothCompared);
                }
            }
            
            return result;
        }

        private string GenerateNodeReport(NodeInfo node)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(node.Value))
                sb.AppendFormat("\tNode value = {0}\n", node.Value);
            foreach (var attr in node.Attributes.Where(attr => !keyAttributes.Contains(attr.Key)))
                sb.AppendFormat("\t{0}=\"{1}\"\n", attr.Key, attr.Value);
            if (node.Attributes.Any())
                sb.Append("\n");
            return sb.ToString();
        }

        private string GenerateReport(Dictionary<string, NodeInfo> keysConfig1, Dictionary<string, NodeInfo> keysConfig2, CompareResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\nNodes that are present only in config 1 file\n");
            foreach (var node in result.OnlyInConfig1)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysConfig1[node]));
            }

            sb.AppendLine("\nNodes that are present only in config 2 file\n");
            foreach (var node in result.MissedInConfig1)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysConfig2[node]));
            }

            sb.AppendLine("\nNodes that are present in both config files but with different attributes\n");
            foreach (var node in result.ChangedInConfig1)
            {
                sb.AppendLine(node);

                if (!string.IsNullOrEmpty(keysConfig2[node].Value))
                    sb.AppendFormat("\tNode value = {0}\n", keysConfig2[node].Value);
                foreach (var change in result.Changes[node])
                    sb.AppendFormat("\t(1) {0}=\"{1}\"\n\t(2) {0}=\"{2}\"\n", change.Attribute, change.ValueInConfig1, change.ValueInConfig2);
                if (result.Changes[node].Any())
                    sb.Append("\n");
            }

            return sb.ToString();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            lblStatus.Text = @"Comparing...";

            var keysConfig1 = XMLToDictionary(tbConfig1.Text);
            var keysConfig2 = XMLToDictionary(tbConfig2.Text);
            var result = Compare(keysConfig1, keysConfig2);
            var report = GenerateReport(keysConfig1, keysConfig2, result);

            lblStatus.Text = @"Saving report...";

            try
            {
                var file = File.CreateText(tbResult.Text);
                file.WriteLine("Comparison result between {0} (1) and {1} (2)", tbConfig1.Text, tbConfig2.Text);
                file.Write(report);
                file.Flush();
                file.Close();

                lblStatus.Text = @"Done";
            }
            catch (Exception ex)
            {
                lblStatus.Text = @"Error saving the report: " + ex.Message; 
            }
        }
    }
}
