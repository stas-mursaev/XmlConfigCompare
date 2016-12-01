namespace XmlConfigCompare
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

        private void btnSelectProd_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbProd.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbProd.Text = openFileDialog1.FileName;
        }

        private void btnSelectUAT_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = tbUAT.Text;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tbUAT.Text = openFileDialog1.FileName;
        }

        private void btnSelectResult_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = tbResult.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                tbResult.Text = saveFileDialog1.FileName;
        }

        readonly List<string> keyAttributes = new List<string> { "name", "key", "ref", "path", "assembly" };

        private class NodeInfo
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
        }

        private class CompareAttrResult
        {
            public string Attribute { get; set; }
            public string ValueOnProd { get; set; }
            public string ValueOnUAT { get; set; }
        }

        private class CompareResult
        {
            public List<string> MissedOnProd { get; set; }
            public List<string> OnlyOnProd { get; set; }
            public List<string> ChangedOnProd { get; set; }
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

            values[name].Name = node.Name.LocalName;
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

        private CompareResult Compare(Dictionary<string, NodeInfo> prod, Dictionary<string, NodeInfo> uat)
        {
            var result = new CompareResult
            {
                OnlyOnProd = prod.Keys.Except(uat.Keys).ToList(),
                MissedOnProd = uat.Keys.Except(prod.Keys).ToList(),
                ChangedOnProd = new List<string>(),
                Changes = new Dictionary<string, List<CompareAttrResult>>()
            };

            foreach (var key in prod.Where(kvp => prod.Keys.Intersect(uat.Keys).Contains(kvp.Key)).Select(kvp => kvp.Key))
            {
                var vp = prod[key].Attributes;
                var vu = uat[key].Attributes;

                var attrOnlyOnProd = (vp.Where(kvp => vp.Keys.Except(vu.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueOnProd = TrimSpaces(vp[attrKey]),
                            ValueOnUAT = null
                        })).ToList();
                
                var attrMissedOnProd = (vu.Where(kvp => vu.Keys.Except(vp.Keys).Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueOnProd = null,
                            ValueOnUAT = TrimSpaces(vu[attrKey])
                        })).ToList();

                var attrBothCompared = (vp.Where(kvp => vp.Keys.Intersect(vu.Keys)
                    .Contains(kvp.Key))
                    .Select(kvp => kvp.Key)
                    .Where(attrKey => !keyAttributes.Contains(attrKey) && !Equals(vp[attrKey], vu[attrKey]))
                    .Select(attrKey => new CompareAttrResult
                        {
                            Attribute = attrKey,
                            ValueOnProd = TrimSpaces(vp[attrKey]),
                            ValueOnUAT = TrimSpaces(vu[attrKey])
                        })).ToList();
                
                if (attrOnlyOnProd.Any() || attrMissedOnProd.Any() || attrBothCompared.Any())
                {
                    if (!result.ChangedOnProd.Contains(key))
                    {
                        result.ChangedOnProd.Add(key);
                        result.Changes.Add(key, new List<CompareAttrResult>());
                    }

                    result.Changes[key].AddRange(attrOnlyOnProd);
                    result.Changes[key].AddRange(attrMissedOnProd);
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

        private string GenerateReport(Dictionary<string, NodeInfo> keysProd, Dictionary<string, NodeInfo> keysUAT, CompareResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("\nNodes that are present only in prod config file\n");
            foreach (var node in result.OnlyOnProd)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysProd[node]));
            }

            sb.AppendLine("\nNodes that are present only in UAT config file\n");
            foreach (var node in result.MissedOnProd)
            {
                sb.AppendLine(node);
                sb.Append(GenerateNodeReport(keysUAT[node]));
            }

            sb.AppendLine("\nNodes that are present in both config files but with different attributes\n");
            foreach (var node in result.ChangedOnProd)
            {
                sb.AppendLine(node);

                if (!string.IsNullOrEmpty(keysUAT[node].Value))
                    sb.AppendFormat("\tvalue = {0}\n", keysUAT[node].Value);
                foreach (var change in result.Changes[node])
                    sb.AppendFormat("\t{0} (prod)\"{1}\" (UAT)\"{2}\"\n", change.Attribute, change.ValueOnProd, change.ValueOnUAT);
                if (result.Changes[node].Any())
                    sb.Append("\n");
            }

            return sb.ToString();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            lblStatus.Text = @"Comparing...";

            var keysProd = XMLToDictionary(tbProd.Text);
            var keysUAT = XMLToDictionary(tbUAT.Text);
            var result = Compare(keysProd, keysUAT);
            var report = GenerateReport(keysProd, keysUAT, result);

            lblStatus.Text = @"Saving report...";

            try
            {
                var file = File.CreateText(tbResult.Text);
                file.WriteLine("Comparison result between {0} (prod) and {1} (UAT)", tbProd.Text, tbUAT.Text);
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
