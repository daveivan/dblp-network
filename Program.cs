using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace dblpnetwork
{
    class MainClass
    {
const string OUTPUT_GRAPH = "./graph.csv";
		const string OUTPUT_NODES = "./nodes.csv";
		const string DEFAULT_DBLP_XML = @"dblp.xml";

		static void createGraph(string filename, int startYear, int endYear)
		{
			try
			{
				Graph g = new Graph();
				XmlReaderSettings settings = new XmlReaderSettings();

				// SET THE RESOLVER
				settings.XmlResolver = new XmlUrlResolver();

				settings.ValidationType = ValidationType.DTD;
				settings.DtdProcessing = DtdProcessing.Parse;
				settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
				settings.IgnoreWhitespace = true;

				// Create the XmlReader object.
				XmlReader reader = XmlReader.Create(filename, settings);

				reader.ReadToDescendant("dblp");
				reader.Read();
				int c = 0;
				int matched = 0;
				IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;

				do
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(reader.ReadOuterXml());
					XmlNode item = doc.DocumentElement;
					XmlNode exist = item.SelectSingleNode(String.Format("year[(text()<={0}) and (text()>={1})]", endYear, startYear));

					if (exist != null)
					{
						matched++;
						XmlNodeList authors = item.SelectNodes("author");
						for (int i = 0; i < authors.Count; i++)
						{
							for (int j = i + 1; j < authors.Count; j++)
							{
								g.AddEdge(authors[i].InnerText, authors[j].InnerText);
							}
						}

						Console.Write("\r Processing... Matched publications : {0}", matched);
					}

					c++;
				}
				while (!reader.EOF && reader.Depth > 0);

				//return doc;
				g.ExportToFile(OUTPUT_GRAPH, OUTPUT_NODES);

				Console.WriteLine("\nDone! Number of nodes: {0}, number of edges: {1}", g.Size, g.NofEdges);
			}
			catch (XmlException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Exception object Line, pos: (" + e.LineNumber + "," + e.LinePosition + ")");
			}

		}

		private static void ValidationCallBack(object sender, ValidationEventArgs e)
		{
			Console.WriteLine("Validation Error: {0}", e.Message);
		}

		static void Main(string[] args)
		{
			try
			{
				//parse args
				int startYear = -1, endYear = -1;
				string filename = DEFAULT_DBLP_XML;
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i];
					switch (text)
					{
						case "-y":
							if (!Int32.TryParse(args[++i], out startYear))
								throw new Exception("Invalid -y argument.");
							break;
						case "-t":
							if (!Int32.TryParse(args[++i], out endYear))
								throw new Exception("Invalid -t argument.");
							break;
						case "-f":
							filename = args[++i];
							break;
						default:
							throw new Exception(String.Format("Argument {0} is invalid.", text));
					}
				}

				if (startYear == -1)
					throw new Exception("Argument -y is required.");
				if (endYear == -1)
					endYear = startYear;

				createGraph(filename, startYear, endYear);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				PrintHelp();
			}
		}

		static void PrintHelp()
		{
			Console.WriteLine("DBLP parsing and creating Collaborative network");
			Console.WriteLine("Usage: Run program with parameters: ");
			Console.WriteLine("-y = Start year (required)");
			Console.WriteLine("-t = end year (optional)");
			Console.WriteLine("-f = path to dblp xml file (optional, default is ./dblp.xml");
			Console.WriteLine("Example: dblp.exe -y 2000 -t 2002");
			Console.WriteLine("Output (2 files): ");
			Console.WriteLine("graph.csv - edge list with ID of authors where edge indicates co-authors");
			Console.WriteLine("nodes.csv - key:value list of authors (ID:name) ");
		}
    }
}
