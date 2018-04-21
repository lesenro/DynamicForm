using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormControls.Models
{
    public class OptionItem
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public object Data { get; set; } = null;
        public string Describe { get; set; } = "";
        public override string ToString()
        {
            return Name;
        }
        public static List<OptionItem> GetList(ArrayList list) {
            List<OptionItem> items = new List<OptionItem>();
            foreach (object o in list)
            {
                Dictionary<string, object> dict = o as Dictionary<string, object>;
                OptionItem item = new OptionItem();
                if (dict.ContainsKey("Key"))
                {
                    item.Key = dict["Key"].ToString();
                }
                if (dict.ContainsKey("Name"))
                {
                    item.Name = dict["Name"].ToString();
                }
                if (dict.ContainsKey("Data"))
                {
                    item.Data = dict["Data"];
                }
                if (dict.ContainsKey("Describe"))
                {
                    item.Describe = dict["Describe"].ToString();
                }
                items.Add(item);
            }
            return items;
        }
    }
}
