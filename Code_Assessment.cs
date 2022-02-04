using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Assessment
{
    /// <summary>
    ///  Problem: Given a specific input string in json, pick last alphabetic property and output objects sorted by the property value
    ///  Algorithm: Use a denormalized storage structure to allow for easy filtering and sorting
    ///  Details:
    ///  1. Read the input json and convert it to List of object with 3 properties("Item","Key","Value") - denormalized structure
    ///  2. Sort the List based on "Key" in descending order
    ///  3. Filter the List to keep only the top entry per "Item"
    ///  4. Sort the List again this time based on "Value" in ascending order
    ///  5. This gives the final list required, then print the List in the format specified
    /// </summary>

    internal class Program
    {

        public class Entity
        {
            private string item;
            private string key;
            private string values;


            public Entity(string item, string key, string values)
            {
                this.item = item;
                this.key = key;
                this.values = values;
            }

            public string Item
            {
                get { return item; }
                set { item = value; }
            }

            public string Key
            {
                get { return key; }
                set { key = value; }
            }

            public string Value
            {
                get { return values; }
                set { values = value; }
            }

        }
        static void Main(string[] args)
        {

            string inputJson = " [{\"apple\": [{\"color\": \"red\"}, {\"stem\": \"true\"}, {\"taste\": \"sweet\" }],\"cow\": [{\"color\": \"brown\"}, {\"stem\": \"false\"}, {\"taste\": \"umami\"}, { \"legs\": \"4\"}, {\"bark\": \"false\" }],\"tree\": [{\"color\": \"brown\"}, { \"root\": \"true\"}, {\"leaves\": \"32767\"}, {\"bark\": \"true\"}],\"dog\": [{ \"color\": \"brown\"}, {\"appendage \": \"4\"}, {\"bark\": \"true\"}]} ]";

            // Read the input jSon and convert it to List of object (denormalized structure)
            List<Entity> EntityList = ReadJsonData(inputJson);

            //Sort the List of object based on Key in descending order
            List<Entity> sortedKeyList = SortListOfObjectBasedOnKey(EntityList);

            //Filter the List of sortedKeyList to get top 1 of each item
            List<Entity> sortedKeyListFiltered = FilterListBasedOnItem(sortedKeyList);

            //Sort the sortedKeyListFiltered based on Value in ascending order
            List<Entity> sortedValueList = SortListofObjectBasonOnValue(sortedKeyListFiltered);

            //Print the final output in desired format
            PrintEntityList(sortedValueList);


        }

        /// <summary>
        /// Read the input jSon and convert it to List of object with object properties - "Item", "Key", "Value" 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Entity> ReadJsonData(string json)
        {
            List<Entity> Entitylist = new List<Entity>();

            //The input json has outer array body, object(apple,cow,tree,dog), and each object(Eg apple) is an array of objects(properties)
            if (json != null && json != String.Empty)
            {
                JArray rootarray = JArray.Parse(json);


                foreach (JObject outerobj in rootarray.Children<JObject>())
                {
                    foreach (JProperty innerArray in outerobj.Properties())
                    {
                        foreach (JObject innerObj in innerArray.Value)
                        {
                            foreach (JProperty Property in innerObj.Children())
                            {
                                Entitylist.Add(new Entity(innerArray.Name, Property.Name, Property.Value.ToString()));
                            }
                        }

                    }

                }
            }

            return Entitylist;

        }

        /// <summary>
        /// Sort the List of object based on Key in descending order
        /// </summary>
        /// <param name="listOfObject"></param>
        /// <returns></returns>
        public static List<Entity> SortListOfObjectBasedOnKey(List<Entity> listOfObject)
        {
            List<Entity> sortedKeyList = null;
            if (listOfObject != null)
            {
                sortedKeyList = (from e in listOfObject
                                 orderby e.Key descending
                                 select e).ToList();
            }
            return sortedKeyList;
        }

        /// <summary>
        /// Filter the List of sortedKeyList to get top1 of each item
        /// </summary>
        /// <param name="sortedList"></param>
        /// <returns></returns>
        public static List<Entity> FilterListBasedOnItem(List<Entity> sortedList)
        {
            List<Entity> sortedKeyList = new List<Entity>();
            if (sortedList != null)
            {
                int counter = 0;
                foreach (var entity in sortedList)
                {
                    counter++;
                    if (counter == 1)
                    {
                        sortedKeyList.Add(new Entity(entity.Item, entity.Key, entity.Value));
                    }
                    //add data to sortedKeyList only if the "Item" is unique
                    else if (counter > 1 && !(sortedKeyList.Any(i => i.Item == entity.Item)))
                    {
                        sortedKeyList.Add(new Entity(entity.Item, entity.Key, entity.Value));
                    }

                }
            }

            return sortedKeyList;
        }

        /// <summary>
        /// Sort the sortedKeyListFiltered based on Value in ascending order
        /// </summary>
        /// <param name="sortedKeyListFiltered"></param>
        /// <returns></returns>
        public static List<Entity> SortListofObjectBasonOnValue(List<Entity> sortedKeyListFiltered)
        {
            List<Entity> sortedValueList = null;
            if (sortedKeyListFiltered != null)
            {
                sortedValueList = (from e in sortedKeyListFiltered
                                   orderby e.Value ascending
                                   select e).ToList();
            }
            return sortedValueList;
        }

        /// <summary>
        /// The final output
        /// Prints the Processed Entity List in the desired format
        /// </summary>
        /// <param name="sortedValueList"></param>
        public static void PrintEntityList(List<Entity> sortedValueList)
        {
            if (sortedValueList != null)
            {
                foreach (var entity in sortedValueList)
                {
                    Console.WriteLine(entity.Item + "-(" + entity.Key + ":" + entity.Value + ")");
                }
            }
        }
    }

}
