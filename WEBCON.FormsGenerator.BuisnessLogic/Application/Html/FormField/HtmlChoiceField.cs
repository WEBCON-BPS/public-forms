using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Interface;
using WEBCON.FormsGenerator.BusinessLogic.Domain.Model;

namespace WEBCON.FormsGenerator.BusinessLogic.Application.Html.FormField
{
    internal abstract class HtmlChoiceField : HtmlBaseField, IChoice
    {
        protected HtmlChoiceField(FieldValue value) : base(value)
        {
        }
        public abstract string CreateChoice(object value);
        public abstract string UpdateChoicesOnFormContent(string formContent);
        protected string UpdateChoicesOnFormContent<T>(string formContent, string nodePath, string alternatePath, string descendants, Func<object, IEnumerable<KeyValuePair<T, string>>> actionGetValueList)
        {
            if (Value.Value == null) return formContent;
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(formContent);
            var groupNode = html.DocumentNode.SelectSingleNode(nodePath);
            if (groupNode == null)
            {
                if (string.IsNullOrEmpty(alternatePath)) return formContent;
                groupNode = html.DocumentNode.SelectSingleNode(alternatePath);
                if (groupNode == null) return formContent;
                nodePath = alternatePath;
            }
            var inputNodes = html.DocumentNode.SelectNodes(nodePath).Descendants(descendants).ToList();
            if (!inputNodes?.Any() ?? true) return formContent;
            var valueList = actionGetValueList?.Invoke(Value.Value);
            if (!valueList?.Any() ?? true) return formContent;
            var contentValues = inputNodes.SelectMany(x => x.Attributes).Where(x => x.Name == "value").Select(x => x.Value).ToList();
            if (!contentValues?.Any()??true) return formContent;
            var newValues = valueList.Select(x => x.Value).Except(contentValues).ToList();
            var removedValues = inputNodes.Where(x => x.Attributes.Any(x => x.Name == "value" && !valueList.Select(x => x.Value).Contains(x.Value))).ToList();

            if (newValues?.Any() ?? false)
                newValues.ToList().ForEach(x => AddNode(valueList, groupNode, x));

            if (removedValues?.Any() ?? false)
                removedValues.ForEach(x => RemoveNode(x, descendants));

            return html.DocumentNode.OuterHtml;
        }
        private void AddNode<T>(IEnumerable<KeyValuePair<T, string>> valueList, HtmlNode groupNode, string value)
        {
            var choice = CreateChoice(valueList.FirstOrDefault(z => z.Value == value).Key);
            if (choice != null)
            {
                var elem = HtmlNode.CreateNode(choice);
                if (elem != null) groupNode.AppendChild(elem);
            }
        }
        private void RemoveNode(HtmlNode node, string descendantName)
        {
            if (node.ParentNode.Name == "div" && node.ParentNode.ChildNodes.Where(n => n.Name != "#text").ToArray().Count() == 2 && !node.ParentNode.ChildNodes.Any(x => node.Name != descendantName && node.Name != "label" && x.Name != "#text"))
                node.ParentNode.Remove();
            else
            {
                if (node.NextSibling != null && node.NextSibling.Name.Equals("label")) node.NextSibling.Remove();
                node.Remove();
            }
        }
    }
}
