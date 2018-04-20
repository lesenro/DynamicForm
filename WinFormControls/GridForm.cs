using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Reflection;

namespace WinFormControls
{
    public partial class GridForm : UserControl
    {
        public FormOptions Options { get; set; }
        private List<FormItem> itemlist = new List<FormItem>();

        public event EventHandler ItemValueChanged;
        private void OnItemValueChanged(object o, EventArgs ev)
        {
            ItemValueChanged?.Invoke(o, ev);
        }

        public GridForm(FormOptions options)
        {
            Options = options;
            InitializeComponent();
            itemlist = FillFormItem();
            this.Controls.Add(InitOptions(Options.Groups, true));
        }
        public GridForm(string json)
        {
            Options = FormOptions.FromJson(json);
            InitializeComponent();
            itemlist = FillFormItem();
            this.Controls.Add(InitOptions(Options.Groups, true));

        }
        public Control InitOptions(Group g, bool isRoot = false)
        {
            Control ctrl = new Control();
            if (g.GroupType == GroupType.Panel)
            {
                ctrl = new Panel();
            }
            else if (g.GroupType == GroupType.GroupBox)
            {
                ctrl = new GroupBox();
            }
            ctrl.Padding = g.Padding;
            ctrl.Name = g.Name;
            ctrl.Text = g.Caption;
            ctrl.Dock = g.Dock;
            ctrl.Tag = g;
            if (g.Children.Count > 0)
            {
                foreach (Group gitem in g.Children)
                {
                    var subCtrl = InitOptions(gitem);
                    ctrl.Controls.Add(subCtrl);
                    subCtrl.BringToFront();
                }
            }
            ctrl.SizeChanged += Ctrl_SizeChanged;
            List<FormItem> items = new List<FormItem>();
            if (isRoot)
            {
                items = itemlist.Where(x => x.Options.GroupName == "" || x.Options.GroupName == g.Name).ToList();
            }
            else
            {
                items = itemlist.Where(x => x.Options.GroupName == g.Name).ToList();
            }
            if (items.Count > 0)
            {
                ctrl.Controls.AddRange(items.ToArray());
                foreach (var item in items)
                {
                    item.BringToFront();
                    item.Dock = DockStyle.Top;
                }
            }
            return ctrl;
        }

        private void Ctrl_SizeChanged(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (ctrl.Dock != DockStyle.Fill)
            {
                Group g = ctrl.Tag as Group;
                ctrl.Width = (int)(ctrl.Parent.Width * g.Width);
            }
        }

        public List<FormItem> FillFormItem()
        {
            List<FormItem> formitems = new List<FormItem>();
            foreach(InputItem item in Options.Items)
            {
                var fitem = new FormItem(item, Options);
                fitem.ValueChanged += (o,ev)=> {
                    OnItemValueChanged(o, ev);
                };
                formitems.Add(fitem);
            }
            return formitems;
        }


        public void FillData<T>(T data)
        {
            Type t = data.GetType();
            var properties = t.GetProperties();
            foreach(PropertyInfo pinfo in properties)
            {
                FormItem item = itemlist.Find(x => x.Name == pinfo.Name);
                if (item != null)
                {
                    item.setValue(pinfo.GetValue(data,null));
                }
            }
        }
        public void FillData(string itemname,object value)
        {
            FormItem item = itemlist.Find(x => x.Name == itemname);
            if (item != null)
            {
                item.setValue(value);
            }
        }

        public T GetValues<T>() where T : new()
        {
            return GetValues(Activator.CreateInstance<T>());
        }
        public T GetValues<T>(T data) where T : new()
        {
            T instance = Activator.CreateInstance<T>();
            Type t = instance.GetType();
            var properties = t.GetProperties();
            foreach (PropertyInfo pinfo in properties)
            {
                FormItem item = itemlist.Find(x => x.Name == pinfo.Name);
                if (item != null)
                {
                    pinfo.SetValue(instance, item.Value, null);
                }
                else
                {
                    pinfo.SetValue(instance, pinfo.GetValue(data, null), null);
                }
            }
            return instance;
        }
    }
}
