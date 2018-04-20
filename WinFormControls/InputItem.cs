using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormControls
{
    public enum InputType
    {
        TextBox,TextArea, DateTimePicker,DateTime, Number,CheckBoxs,RichText,RadioButtons,ComboBox,Switch
    }
    public class InputItem
    {
        public InputType InputType { get; set; } = InputType.TextBox;
        public string Name { get; set; } = "";
        public string Caption { get; set; } = "";
        public string GroupName { get; set; } = "";
        public int Height { get; set; } = 0;
        public Dictionary<string, object> Options { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Rules { get; set; } = new Dictionary<string, object>();
    }
}
