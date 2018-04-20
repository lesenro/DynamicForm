using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WinFormControls
{
    public class FormOptions
    {
        public int Width { get; set; } = 0;
        public int LabelWidth { get; set; } = 50;
        public int VerticalPadding { get; set; } = 3;
        public Group Groups { get; set; } = new Group();
        public ContentAlignment LabelAlignment { get; set; } = ContentAlignment.MiddleCenter;
        public List<InputItem> Items { get; set; } = new List<InputItem>();
        public static FormOptions FromJson(string json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Deserialize<FormOptions>(json);
        }

    }
}
