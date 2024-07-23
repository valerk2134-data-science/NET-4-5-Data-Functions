using ReportFunctionsNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
// Licence. The code is in public domain. The default copyright is applied.
namespace XMLFileFunctionsNamespace
{
	public static class XMLFileFunctions
	{
		// note. 2024.05.27 11:14. Warsaw. Workplace 
		// console numerating and address output.
		public static class ToConsole
		{
			/// <summary>
			/// Written. 2024.05.27 11:58. Warsaw. Workplace.
			/// </summary>
			/// <param name="xml_doc_in"></param>
			public static void Root(XmlDocument xml_doc_in)
			{
				Console.WriteLine("Root is " + xml_doc_in.DocumentElement.Name);
				if (xml_doc_in.ChildNodes.Count > 1)
				{
					Console.WriteLine("Multiple root is found");
				}
			}


            /// <summary>
            /// Written. Warsaw. Workplace. 2024.06.28 13:35.  
			/// Tested. Works. Warsaw. Workplace. 2024.06.28 14:23. 
            /// </summary>
            /// <param name="xml_node_in"></param>
            public static void NodeWhole(XmlNode xml_node_in)
            {
                string node_name_str = xml_node_in.Name;
                Console.WriteLine("." + node_name_str);
				XmlNodeList nodes = xml_node_in.ChildNodes;
				for (int i = 0; i < nodes.Count; i++)
				{
					Console.WriteLine(".\t" + nodes[i].Name + "\t" + nodes[i].InnerText);
				}
            }

            /// <summary>
            /// Written. Warsaw. Workplace. 2024.06.28 14:23. <br></br>
			/// Tested. Works. Warsaw. Workplace. 2024.06.28 14:25. 
            /// </summary>
            /// <param name="xml_node_in"></param>
            public static void Node(XmlNode xml_node_in)
            {
                string node_name_str = xml_node_in.Name;
                Console.WriteLine("." + node_name_str);
                XmlNodeList nodes = xml_node_in.ChildNodes;
                for (int i = 0; i < nodes.Count; i++)
                {
                    Console.WriteLine(".\t" + nodes[i].Name);
                }
            }

            /// <summary>
            /// Written. Note made in Warsaw. Workplace. 2024.06.28 13:31. 
            /// </summary>
            /// <param name="xml_node_in"></param>
            public static void NodeName(XmlNode xml_node_in)
			{
				string node_name_str = xml_node_in.Name;
				Console.WriteLine(node_name_str);
			}

                /// <summary>
                /// Written. 2024.05.27 11:54. Warsaw. Workplace. <br></br>
                /// Tested. Works. 2024.05.27 12:16. Warsaw. Workplace.
                /// </summary>
                /// <param name="xml_doc_in"></param>
                public static void NodesOfNode(XmlElement xml_root_in, int[] ladder_path)
			{
				// 2024.05.27 12:19. Warsaw. Workplace.
				// NodesOfNode
				// NodeList
				XmlNodeList node_list = xml_root_in.ChildNodes;
				XmlNode step_node = null;
				for (int i = 0; i < ladder_path.Length; i++)
				{
					step_node = node_list[ladder_path[i] - 1];
					node_list = step_node.ChildNodes;
				}
				for (int i = 0; i < node_list.Count; i++)
				{
					Console.WriteLine((i + 1).ToString() + " " + node_list[i].Name);
				}
			}
			/// <summary>
			/// Show in console the nodes of the root of xml file. <br></br>
			/// Written. 2024.05.27 11:54. Warsaw. Workplace. <br></br>
			/// Tested. Works. 2024.05.27 12:22. Warsaw. Workplace.
			/// </summary>
			/// <param name="xml_doc_in"></param>
			public static void NodesOfRoot(XmlElement xml_root_in)
			{
				XmlNodeList nodes_of_root = GetRootNodes(xml_root_in);
				for (int i = 0; i < nodes_of_root.Count; i++)
				{
					Console.WriteLine((i + 1).ToString() + " " + nodes_of_root[i].Name);
				}
			}
		}


		/// <summary>
		/// Written. Warsaw. Workplace. 2024.07.08 11:27. 
		/// </summary>
		/// <param name="xml_doc"></param>
		/// <param name="xml_filepath"></param>
		public static void SaveFile(XmlDocument xml_doc, string xml_filepath)
		{
			xml_doc.Save(xml_filepath);
		}

		/// <summary>
		/// Written. Warsaw. Workplace. 2024.06.28 14:00. 
		/// </summary>
		public static class WriteFile
		{
			/// <summary>
			/// Written. Warsaw. Workplace. 2024.06.28 14:01. <br></br>
			/// Tested. Works. Warsaw. Workplace. 2024.06.28 14:14. <br></br>
			/// Appends element with name and inner text. <br></br>
			/// </summary>
			/// <param name="node_to_file"></param>
			/// <param name="filename_in"></param>
			public static void FromNode(XmlElement node_to_file, string filename_in)
			{
				XmlDocument xml_doc = new XmlDocument();
				XmlElement root_node = xml_doc.CreateElement(node_to_file.Name);

				for (int i = 0; i < node_to_file.ChildNodes.Count; i++)
				{
					XmlElement child_node = (XmlElement)xml_doc.ImportNode(node_to_file.ChildNodes[i], true);
                       
                    root_node.AppendChild(child_node);
				}
				xml_doc.AppendChild(root_node);				
				xml_doc.Save(filename_in);
			}
		}


		/// <summary>
		/// Written. Warsaw. Workplace. 2024.06.28 13:21. 
		/// </summary>
		public static class Element
		{
			/// <summary>
			/// Written. Warsaw. Workplace. 2024.06.28 13:22. 
			/// </summary>
			public static class Create
			{
				/// <summary>
				/// Written. Warsaw. Workplace. 2024.06.28 14:57. <br></br>
				/// Tested. Not Working. Note made in Warsaw. Workplace. 2024.06.28 15:06. <br></br>
				/// Writing XmlElement in existing XmlElement requires locally made XmlDocument and import node. <br></br>
				/// </summary>
				/// <param name="name_in_use"></param>
				/// <returns></returns>
                public static XmlElement BlankXmlElement(string name_in_use)
				{
					XmlDocument doc = new XmlDocument();
					return doc.CreateElement(name_in_use);
				}
                

                    /// <summary>
                    /// Written. Warsaw. Workplace. 2024.06.28 14:53. <br></br>
					/// Tested. Works. Warsaw. Workplace. 2024.06.28 15:05. 
                    /// </summary>
                    /// <param name="nodes_in"></param>
                    /// <param name="root_name"></param>
                    /// <returns></returns>
                    public static XmlElement FromNodeList(XmlNodeList nodes_in, string root_name)
				{
                    XmlDocument xml_doc = new XmlDocument();
                    XmlElement root_node = xml_doc.CreateElement(root_name);

                    for (int i = 0; i < nodes_in.Count; i++)
                    {
                        XmlElement child_node = (XmlElement)xml_doc.ImportNode(nodes_in[i], true);

                        root_node.AppendChild(child_node);
                    }
					return root_node;
                }




                    /// <summary>
                    /// Written. Note made in Warsaw. Workplace. 2024.06.28 14:50. 
                    /// Tested. Works. Note made in Warsaw. Workplace. 2024.06.28 14:50. 
                    /// </summary>
                    /// <param name="data_in"></param>
                    /// <param name="node_name"></param>
                    /// <param name="column_name"></param>
                    /// <returns></returns>
                    public static XmlElement NodeColumns(string[] data_in, string node_name, string column_name = "column_")
				{
					XmlDocument doc_for_node = new XmlDocument();

					XmlElement node_out = doc_for_node.CreateElement(node_name);
					XmlElement[] data_nodes = new XmlElement[data_in.Length];
					for (int i = 0; i < data_in.Length; i++)
					{
						// Note made in Warsaw. Workplace. 2024.06.28 13:27. 
						// space is selected so the text in XML is good to read.

						// Note made in Warsaw. Workplace. 2024.06.28 13:55. 
						// space " " is not allowed in XMLElement name.
						data_nodes[i] = doc_for_node.CreateElement(column_name + (i + 1).ToString());
						data_nodes[i].InnerText = data_in[i];
						node_out.AppendChild(data_nodes[i]);
					}
					return node_out;
				}
			}
		}


		/// <summary>
		/// Written. 2024.05.24 11:15. Warsaw. Workplace. <br></br>
		/// Tested. Works. 2024.05.24 11:16. Warsaw. Workplace.
		/// </summary>
		/// <param name="filename_in"></param>
		/// <param name="name_of_root"></param>
		public static void CreateFile(string filename_in, string name_of_root)
		{
			XmlDocument xml_doc = new XmlDocument();
			XmlElement root_of_xml = xml_doc.CreateElement(name_of_root);
			xml_doc.AppendChild(root_of_xml);
			xml_doc.Save(filename_in);
			// 2024.05.24 11:36. Warsaw. Workplace.
			// Selecting writter is choice of person. It is clear what it does but I assume
			// it takes more time to fill xml document.
			/*
			XmlWriter xml = XmlWriter.Create(filename_in);
			xml.WriteStartDocument();
			xml.WriteStartElement(name_of_root);
			xml.WriteEndElement();
			xml.WriteEndDocument();
			xml.Flush();
			*/
		}





        /// <summary>
        /// Written. Warsaw. Workplace. 2024.07.08 11:22. <br></br>
        /// 
        /// </summary>
        /// <param name="xml_root"></param>
        /// <param name="node_name"></param>
        /// <returns></returns>
        public static bool IsNodeExist(string xml_file_path, string node_name)
        {
            XmlDocument doc = LoadFile(xml_file_path);
			XmlElement xml_root = GetRoot(doc);			
			bool check_result = true;
            if (xml_root.SelectSingleNode(node_name) == null)
            {
                check_result = false;
            }
            return check_result;

            // Note made in Warsaw. Workplace. 2024.07.08 11:16. 
            // alternatively
            /*
			static bool NodeExists(XmlDocument xmlDoc, string nodeName)
            {
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName(nodeName);
                return nodeList.Count > 0;
            }
			*/


        }




        /// <summary>
        /// Written. Warsaw. Workplace. 2024.07.08 11:19.  <br></br>
        /// 
        /// </summary>
        /// <param name="xml_root"></param>
        /// <param name="node_name"></param>
        /// <returns></returns>
        public static bool IsNodeExist(XmlElement xml_root, string node_name)
        {
			bool check_result = true;
			if (xml_root.SelectSingleNode(node_name) == null)
			{
				check_result = false;
			}
			return check_result;

            // Note made in Warsaw. Workplace. 2024.07.08 11:16. 
            // alternatively
            /*
			static bool NodeExists(XmlDocument xmlDoc, string nodeName)
            {
                XmlNodeList nodeList = xmlDoc.GetElementsByTagName(nodeName);
                return nodeList.Count > 0;
            }
			*/


        }

		/// <summary>
		/// Renames the node.<br></br> 
		/// Full path after root should be provided. <br></br> 
		/// 
		/// was not tested.
		/// </summary>
		/// <param name="xml_filepath"></param>
		/// <param name="node_name"></param>
		/// <param name="new_name"></param>
		public static void RenameNode(string xml_filepath, string node_name, string new_name)
		{
			XmlDocument xml_doc = LoadFile(xml_filepath);
			XmlElement xml_root = GetRoot(xml_doc);
			XmlElement xml_node = GetNode(xml_root, node_name);
			XmlElement new_node = xml_doc.CreateElement(new_name);
			new_node.InnerText = xml_node.InnerText;
            xml_root.InsertBefore(new_node, xml_node);
            xml_root.RemoveChild(xml_node);
			SaveFile(xml_doc, xml_filepath);	
			// Written. Warsaw. Workplace. 2024-07-22 10:54. 
        }

            /// <summary>
            /// Warsaw. Workplace. 2024.07.08 11:32.  <br></br>
            /// Tested. Works. Warsaw. Workplace. 2024.07.08 11:41. 
            /// </summary>
            /// <param name="xml_root"></param>
            /// <param name="node_name"></param>
            /// <returns></returns>
            public static void ChangeNodeText(string xml_filepath, string node_name, string data_in)
        {
			try
			{
				XmlDocument xml_doc = LoadFile(xml_filepath);
				XmlElement xml_root = GetRoot(xml_doc);
				XmlElement required_element = (XmlElement)xml_root.SelectSingleNode(node_name);
				required_element.InnerText = data_in;
				xml_doc.Save(xml_filepath);
			}
			catch
			{
				ReportFunctions.ReportError("Error. Check filepath and nodename");
			}
        }


        /// <summary>
        /// Written. 2024.05.27 09:44. Warsaw. Workplace. <br></br>
        /// Tested. Works. 2024.05.27 09:48. Warsaw. Workplace.
        /// </summary>
        /// <param name="xml_root"></param>
        /// <param name="node_name"></param>
        /// <returns></returns>
        public static XmlElement GetNode(XmlElement xml_root, string node_name)
		{
			return (XmlElement)xml_root.SelectSingleNode(node_name);
		}
		/// <summary>
		/// Return nodes of the root of xml file. <br></br>
		/// Written. 2024.05.27 12:21. Warsaw. Workplace. <br></br>
		/// Tested. Works. 2024.05.27 12:23. Warsaw. Workplace.
		/// </summary>
		/// <param name="xml_doc_in"></param>
		public static XmlNodeList GetRootNodes(XmlElement xml_root_in)
		{
			XmlNodeList nodes_of_root = xml_root_in.ChildNodes;
			return nodes_of_root;
		}

		/// <summary>
		/// Written. Warsaw. Workplace. 2024.07.08 13:12. 
		/// </summary>
		/// <param name="xml_filepath"></param>
		/// <returns></returns>
        public static XmlNodeList GetRootNodes(string xml_filepath)
        {
			XmlDocument xml_doc = LoadFile(xml_filepath);
			XmlNodeList nodes_of_root = GetRoot(xml_doc).ChildNodes;
            return nodes_of_root;
        }




        /// <summary>
        /// Written. 2024.05.27 09:40. Warsaw. Workplace. <br></br>
        /// Tested. Works. 2024.05.27 10:17. Warsaw. Workplace.
        /// </summary>
        /// <param name="xml_node_in"></param>
        /// <param name="attribute_name"></param>
        /// <param name="attribute_value"></param>
        public static void AddAttribute(XmlNode xml_node_in, string attribute_name, string attribute_value)
		{
			XmlDocument xml_doc = xml_node_in.OwnerDocument;
			XmlAttribute attribute_add = xml_doc.CreateAttribute(attribute_name);
			attribute_add.Value = attribute_value;
			xml_node_in.Attributes.SetNamedItem(attribute_add);
			string filename_in_use = XmlFilepath(xml_doc);
			xml_doc.Save(filename_in_use);
		}
		/// <summary>
		/// Written. 2024.05.27 09:09. Warsaw. Workplace. <br></br>
		/// Tested. Works. 2024.05.27 09:30. Warsaw. Workplace. <br></br>
		/// Important. Name of element cannot begin with the digit 1, 2, 3 ... . There should be no digit in the beginning of element name.
		/// </summary>
		/// <param name="xml_doc_in"></param>
		/// <returns></returns>
		/// 
		public static void AddToRoot(XmlDocument xml_doc_in, string element_name)
		{
			XmlElement xml_root = xml_doc_in.DocumentElement;
			XmlElement new_xml_element = xml_doc_in.CreateElement(element_name);
			xml_root.AppendChild(new_xml_element);
			string filename_in_use = XmlFilepath(xml_doc_in);
			xml_doc_in.Save(filename_in_use);
		}
		/// <summary>
		/// Written. 2024.05.27 10:11. Warsaw. Workplace 
		/// </summary>
		/// <param name="xml_doc_in"></param>
		/// <returns></returns>
		public static string XmlFilepath(XmlDocument xml_doc_in)
		{
			string filename_in_use = xml_doc_in.DocumentElement.BaseURI;
			filename_in_use = filename_in_use.Replace("file:///", "");
			filename_in_use = filename_in_use.Replace("/", "\\");
			return filename_in_use;
		}
		/// <summary>
		/// Written. 2024.05.27 09:02. Warsaw. Workplace.
		/// </summary>
		/// <param name="xml_doc_in"></param>
		/// <returns></returns>
		public static XmlElement GetRoot(XmlDocument xml_doc_in)
		{
			return xml_doc_in.DocumentElement;
		}


		/// <summary>
		/// Return root (XmlElement) of xml file using provided filepath of the file. <br></br>
		/// Written. Warsaw. Workplace. 2024.07.08 13:14. <br></br>
		/// Tested. Works. Warsaw. Workplace. 2024.07.08 13:16. <br></br> 
		/// </summary>
		/// <param name="xml_doc_in"></param>
		/// <returns></returns>
        public static XmlElement GetRoot(string xml_filepath)
        {
            return LoadFile(xml_filepath).DocumentElement;
        }


        /// <summary>
        /// Written. 2024.05.24 11:36. Warsaw. Workplace. <br></br>
        /// Tested. Works. 2024.05.24 11:39. Warsaw. Workplace.
        /// </summary>
        /// <param name="filename_in"></param>
        /// <returns></returns>
        public static XmlDocument LoadFile(string filename_in)
		{
			// 2024.05.24 11:36. Warsaw. Workplace.
			// There is trouble spreaded with BOM (byte order mark)
			XmlDocument xml_doc = new XmlDocument();
			xml_doc.Load(filename_in);
			return xml_doc;
		}
		/// <summary>
		/// Written. 2024.05.24 11:41. Warsaw. Workplace. <br></br>
		/// Tested. Works. 2024.05.24 11:43. Warsaw. Workplace. 
		/// </summary>
		/// <param name="filename_in"></param>
		/// <returns></returns>
		public static void LoadFile(ref XmlDocument xml_doc, string filename_in)
		{
			xml_doc = new XmlDocument();
			xml_doc.Load(filename_in);
		}
		// 2024.05.24 14:41. Warsaw. Workplace.
		// There is no trouble cast between XmlElement and XmlNode
		// There is no difference in usage has been found.
		// 1. XmlNode is for navigation, adding and removing elements (nodes) from xml file.
		// 2. XmlElement is for adding, removing attribute (editing certain Xmlnode - certain Xmlelement)
	}
}
