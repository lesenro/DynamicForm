using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormControls
{
    public enum GroupType
    {
        GroupBox,Panel
    }
    public class Group
    {
        public GroupType GroupType { get; set; } = GroupType.Panel;
        public string Name { get; set; } = "";
        public Padding Padding { get; set; } = new Padding(0);
        public string Caption { set; get; } = "";
        public DockStyle Dock { get; set; } = DockStyle.None;
        public float Width { get; set; } = 1f;
        public List<Group> Children { get; set; } = new List<Group>();
    }
}
