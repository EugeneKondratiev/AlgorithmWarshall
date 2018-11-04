
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;



namespace MatrixStrongConnectivity
{
	class ReaderWarshall : DGMLWriter
	{
		private const int INF = 999999;
		protected string el;
		protected int[,] matrix;
		protected int quantity;
		public void StreamAxis(string inputUrl, string elementName)
		{
			//System.Diagnostic.Process.GetCurrentProcess().Kill();
			XmlReader reader = XmlReader.Create(inputUrl);
			reader.MoveToContent();
			while (reader.Read())
			{
				switch(reader.NodeType)
				{
					case XmlNodeType.Element:
						if (reader.Name == elementName)
						el = reader.GetAttribute("Id");
						break;
				}
			}
			reader.Close();
			quantity = Convert.ToInt32(el);
			matrix = new int[quantity,quantity];
		}
		public string getEl()
		{
			return el;
		}
		public int getQuantity()
		{
			return quantity;
		}

		public string[,] advertiseStrMatrix()
		{
			string[,] matrixStr = new string[quantity,quantity];
			for (int i = 0; i < quantity; i++)
			{
				for (int j = 0; j < quantity; j++)
				{
					if (matrix[i,j] > 99999)
					{
						matrixStr[i,j] = "inf";
					}
					else
					{
						matrixStr[i,j] = matrix[i, j].ToString();
					}
				}
			}
			return matrixStr;
		}

		public void getDataGrid(string[] matrixDG)
		{
			int k = 0;
			for (int i = 0; i < quantity; i++)
			{
				for (int j = 0; j < quantity; j++)
				{
					if ((matrixDG[k] == "inf") || (matrixDG[k] == "INF"))
					{
						matrix[i, j] = INF;
						k++;
					}
					else
					{
						matrix[i, j] = Int32.Parse(matrixDG[k]);
						k++;
					}
				}
			}
		}
		public void getRichText(string richText)
		{
			el = richText.Replace("\n", string.Empty);
			string[] numbs;
			//string[] tokens = el.Split(new[] { ",", "\n" }, StringSplitOptions.None);
			numbs = el.Split(',');
			int k = 0;
			for (int i = 0; i < quantity; i++)
			{
				for (int j = 0; j < quantity; j++)
				{
					if ((numbs[k] == "inf") || (numbs[k] == "INF"))
					{
						matrix[i, j] = INF;
						k++;
					}
					else
					{
						matrix[i, j] = Int32.Parse(numbs[k]);
						k++;
					}
				}
			}
		}

		public void writeUserMatrix(string xmlpath)
		{
			for (int i = 0; i < quantity; i++)
			{
				for (int j = 0; j < quantity; j++)
				{
					if ((matrix[i,j] >= 1) && (matrix[i,j] < 9999) )
					{
						Link link = new Link((i+1).ToString(),(j+1).ToString(),matrix[i,j].ToString());
						AddLink(link);
					}
				}
			}
			graph.Links = this.Links.ToArray();

			
			XmlRootAttribute root = new XmlRootAttribute("DirectedGraph");
			root.Namespace = "http://schemas.microsoft.com/vs/2009/dgml";
			XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			XmlWriter xmlWriter = XmlWriter.Create(xmlpath, settings);
			serializer.Serialize(xmlWriter, graph);
			xmlWriter.Close();
		}

		public void Warshall(string xmlpath, string typeofMatrix)
		{
			if (typeofMatrix == "Матрица смежности")
			{
				for (int k = 0; k < quantity; k++)
				{
					for (int i = 0; i < quantity; i++)
					{
						for (int j = 0; j < quantity; j++)
						{
							
							matrix[i, j] = matrix[i, j] | (matrix[i, k] & matrix[k, j]);
							if ((matrix[i, j] >= 1) && (matrix[i, j] < 9999))
							{
								Link link = new Link((i + 1).ToString(), (j + 1).ToString(), matrix[i, j].ToString());
								AddLink(link);
							}
							
						}
					}
				}
			} else if (typeofMatrix == "Матрица достижимости")
			{
				for (int k = 0; k < quantity; k++)
				{
					for (int i = 0; i < quantity; i++)
					{
						for (int j = 0; j < quantity; j++)
						{
							if (matrix[i, k] + matrix[k, j] < matrix[i, j])
							{
								matrix[i, j] = matrix[i, k] + matrix[k, j];
								if ((matrix[i, j] >= 1) && (matrix[i, j] < 9999))
								{
									Link link = new Link((i + 1).ToString(), (j + 1).ToString(), matrix[i, j].ToString());
									AddLink(link);
								}
							}
						}
					}
				}
			}
			graph.Links = this.Links.ToArray();


			XmlRootAttribute root = new XmlRootAttribute("DirectedGraph");
			root.Namespace = "http://schemas.microsoft.com/vs/2009/dgml";
			XmlSerializer serializer = new XmlSerializer(typeof(Graph), root);
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			XmlWriter xmlWriter = XmlWriter.Create(xmlpath, settings);
			serializer.Serialize(xmlWriter, graph);
			xmlWriter.Close();
		}
	}
}
