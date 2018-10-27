using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ParseJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseJSON();
            Console.ReadKey();

        }

        public static void ParseJSON()
        {
            string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = dirPath + "\\MPA_Title Block_Key Plan Control.dyn";

            using (StreamReader reader = File.OpenText(filePath))
            {
                // Read JSON file, and get a JToken from it for iterating
                JObject jObject = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                //Console.WriteLine("JOBJECT");
                //Console.Write(jObject);

                JToken nodeObject = jObject["Nodes"];
                JToken connectorObject = jObject["Connectors"];

                Dictionary<string, string> NodeDictionary =new Dictionary<string, string>();
                Dictionary<string, string> IODictionary = new Dictionary<string, string>();

                //Console.Write(nodeObject);

                //Create dictionary for ID and node name
                //Create dictionary for connector ID and node ID
                foreach (var node in nodeObject)
                {
                    //Console.WriteLine(test);
                    //Console.WriteLine(node["FunctionSignature"]);
                    string stringnodeID = node["Id"].ToString();

                    string stringnodename = "NONE";

                    try
                    {
                        stringnodename = node["FunctionSignature"].ToString();
                        Console.WriteLine(stringnodename);
                    }
                    catch
                    {
                        Console.WriteLine("MISSING FUNCTION SIGNATURE");
                    }

                    if (stringnodename != "NONE")
                    {
                        NodeDictionary.Add(stringnodeID, stringnodename);


                        JToken outputObject = node["Outputs"];

                        foreach (var output in outputObject)
                        {
                            string outputID = output["Id"].ToString();
                            IODictionary.Add(outputID, stringnodeID);
                        }
                        JToken inputObject = node["Inputs"];

                        foreach (var inputs in inputObject)
                        {
                            string inputID = inputs["Id"].ToString();
                            IODictionary.Add(inputID, stringnodeID);
                        }
                        //Console.WriteLine(IODictionary);
                    }

                }
                

                foreach (var connector in connectorObject)
                {
                    string inputID = connector["Start"].ToString();
                    string outputID = connector["End"].ToString();


                    string NodeAID = IODictionary[ inputID ];
                    string NodeBID = IODictionary[ outputID ];

                    string NodeASig = NodeDictionary[ NodeAID ];
                    string NodeBSig = NodeDictionary[ NodeBID ];

                    Console.WriteLine( NodeAID );
                    Console.WriteLine( NodeBID );
                    Console.WriteLine( NodeASig );
                    Console.WriteLine( NodeBSig );
                    //Console.Writeline("************");
               
                    //List StartList = List<connector["Start"].ToString)>;
                }

                //foreach (JToken thing in nodeObject.Values())
                //{
                //    Console.WriteLine(thing["FunctionSignature"]);
                //    Console.WriteLine(("________"));
                //}

               /* foreach (JToken test in connectorObject)
                {
                    string jTokenString = (string)test["Start"];
                    //Console.WriteLine(jTokenString);

                    IEnumerable<JToken> jsonQueryEnumerable = from JToken thing in nestedTokenList
                                                              where thing["Id"].ToString() == "0faed7bef55346c681d5ef3030b97440"
                                                              select thing;

                    Console.WriteLine("SUCCESS??");
                    Console.WriteLine(jsonQueryEnumerable.First());

                    //Console.WriteLine(jsonQueryEnumerable.First().ToString());
                }*/
            }

            Console.Read();

        }
    }
}