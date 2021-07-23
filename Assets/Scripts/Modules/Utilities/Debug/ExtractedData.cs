using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GameUtilities.HedieVisualizer
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract class ExtractedData
    {
        [JsonIgnore]
        public abstract string[] Tags { get; }

        [JsonProperty("kind")]
        private Dictionary<string, bool> Kind
        {
            get 
            {
                var d = new Dictionary<string, bool>();

                foreach (var tag in Tags)
                {
                    d.Add(tag, true);
                }

                return d;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class TextData : ExtractedData
    {
        public override string[] Tags => new string[] { "text" };

        [JsonProperty("text")]
        public string TextValue { get; set; }

        public TextData(string text)
        {
            this.TextValue = text;
        }
    }

    public class PlotData : ExtractedData
    {
        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class PlotNode
        {
            [JsonProperty("text")]
            public string Name = null;

            [JsonProperty("y")]
            private List<float> Values = new List<float>();

            [JsonProperty("x")]
            private List<float> Times;

            public void AddData(float value, float? time = null)
            {
                Values.Add(value);

                if (time != null)
                {
                    if (Times == null)
                    {
                        Times = new List<float>();
                    }

                    Times.Add((float)time);
                }
            }
        }

        public override string[] Tags => new string[] { "plotly" };

        [JsonProperty("data")]
        private List<PlotNode> Data = new List<PlotNode>();

        public void SetTitles(params string[] titles)
        {
            for(int i = 0; i < titles.Length; i++)
            {
                GetGraph(i).Name = titles[i];
            }
        }

        public PlotNode GetGraph(int index)
        {
            if (index >= Data.Count)
            {
                for (int i = Data.Count; i <= index; i++)
                {
                    Data.Add(new PlotNode());
                }
            }

            return Data[index];
        }
    }

    public class GraphData : ExtractedData
    {
        public override string[] Tags => new string[] { "graph" };

        [JsonProperty("nodes")]
        public IList<NodeData> Nodes { get; set; } = new List<NodeData>();

        [JsonProperty("edges")]
        public IList<EdgeData> Edges { get; set; } = new List<EdgeData>();

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class NodeData
        {
            public NodeData(string id)
            {
                Id = id;
            }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("shape")]
            public string Shape { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class EdgeData
        {
            public EdgeData(string from, string to)
            {
                From = from;
                To = to;
            }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("dashes")]
            public bool? Dashes { get; set; }
        }

        public static GraphData From<T>(IEnumerable<T> items, Func<T, NodeInfo<T>, NodeInfo<T>> f)
        {
            var d = new GraphData();
            var q = new Queue<T>(items);
            var i = 0;
            var infos = new Dictionary<T, NodeInfo<T>>();

            string GetId(NodeInfo<T> item)
            {
                if (item.Id == null)
                    item.Id = "hediet.de/" + (i++);
                return item.Id;
            }

            NodeInfo<T> GetNodeInfo(T item)
            {
                if (infos.ContainsKey(item))
                    return infos[item];

                var info = f(item, new NodeInfo<T>());
                infos.Add(item, info);
                return info;
            }

            while (q.Count > 0)
            {
                var nodeInfo = GetNodeInfo(q.Dequeue());
                var nd = new NodeData(GetId(nodeInfo));
                d.Nodes.Add(nd);

                nd.Label = nodeInfo.Label;
                nd.Color = nodeInfo.Color;

                foreach (var e in nodeInfo.Edges)
                {
                    var ed = new EdgeData(nd.Id, GetId(GetNodeInfo(e.To)));
                    d.Edges.Add(ed);

                    ed.Label = e.Label;
                    ed.Id = e.Id;

                    q.Enqueue(e.To);
                }
            }

            return d;
        }

        public class NodeInfo<T>
        {
            public IList<EdgeInfo<T>> Edges { get; set; } = new List<EdgeInfo<T>>();
            public string Label { get; set; }
            public string Id { get; set; }
            public string Color { get; set; }

            public NodeInfo<T> AddEdge(T to, string id = null, string label = null)
            {
                var e = new EdgeInfo<T>(to);
                e.Id = id;
                e.Label = label;
                Edges.Add(e);
                return this;
            }
        }

        public class EdgeInfo<T>
        {
            public T To { get; set; }
            public string Label { get; set; }
            public string Id { get; set; }

            public EdgeInfo(T to)
            {
                To = to;
            }
        }
    }
}