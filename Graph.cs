using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dblpnetwork
{
	public class Graph
	{
		private SortedDictionary<int, SortedSet<int>> storage;
		//private List<string> nodes;
		private Dictionary<string, int> nodesLookup;
		private int counter;

		public int Size
		{
			get
			{
				return nodesLookup.Count;
			}
		}

		public int NofEdges
		{
			get
			{
				int i = 0;
				foreach (var edgelist in storage)
					i += edgelist.Value.Count();

				return i;
			}
		}

		public Graph()
		{
			storage = new SortedDictionary<int, SortedSet<int>>();
			//nodes = new List<string>();
			nodesLookup = new Dictionary<string, int>();
			counter = 0;
		}

		public void AddNode(string name)
		{
			nodesLookup.Add(name, counter);
			counter++;
		}

		public bool AddEdge(string name1, string name2)
		{
			int n1 = GetIndexNode(name1);
			if (n1 == -1)
			{
				AddNode(name1);
				n1 = counter - 1;
			}
			int n2 = GetIndexNode(name2);
			if (n2 == -1)
			{
				AddNode(name2);
				n2 = counter - 1;
			}

			if (n1 < n2)
			{
				if (!storage.ContainsKey(n1))
					storage.Add(n1, new SortedSet<int>());
				return storage[n1].Add(n2);
			}
			else if (n2 > n1)
			{
				if (!storage.ContainsKey(n2))
					storage.Add(n2, new SortedSet<int>());
				return storage[n2].Add(n1);
			}
			else
			{
				return false;
			}
		}

		public int GetIndexNode(string name)
		{
			if (nodesLookup.ContainsKey(name))
				return nodesLookup[name];

			return -1;
		}

		public void ExportToFile(string filenameGraph, string filenameNodes)
		{
			using (var w = new StreamWriter(filenameGraph))
			{
				foreach (var n1 in storage)
				{
					foreach (var n2 in n1.Value)
					{
						var line = string.Format("{0},{1}", n1.Key, n2);
						w.WriteLine(line);
						w.Flush();
					}
				}
			}

			using (var w = new StreamWriter(filenameNodes))
			{
				foreach (var n in nodesLookup)
				{
					var line = string.Format("{0}\t{1}", n.Value, n.Key);
					w.WriteLine(line);
					w.Flush();
				}
			}
		}
	}
}
