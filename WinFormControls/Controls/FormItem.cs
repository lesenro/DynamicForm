using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormControls.Models;
using System.Collections;

namespace WinFormControls.Controls
{
    public partial class FormItem : UserControl
    {
        /// <summary>
        /// 本输入框设置信息
        /// </summary>
        public InputItem Options { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public object Value { get; set; } = null;
        /// <summary>
        /// 变更事件处理
        /// </summary>
        public event EventHandler ValueChanged;
        private void OnValueChanged(object o ,EventArgs ev) {
            ValueChanged?.Invoke(o, ev);
        }
        private Control ctrl = new Control();
        private FormOptions formOptions;
        public FormItem(InputItem item, FormOptions frmoptins)
        {
            Options = item;
            formOptions = frmoptins;
            InitializeComponent();
            Init();
        }

        public void updateOptions(string str)
        {
            Options.Options["options"] = str;
            panel1.Controls.Clear();
            Init();
            Value = "";
        }
        public void updateOptions(List<OptionItem> items)
        {
            Options.Options["options"] = items;
            panel1.Controls.Clear();
            Init();
            Value = "";
        }
        public void setValue(object o)
        {
            try
            {
                object val = null;
                switch (Options.InputType)
                {
                    case InputType.TextBox:
                        TextBox tb = ctrl as TextBox;
                        val = tb.Text = o.ToString();
                        break;
                    case InputType.TextArea:
                        TextBox tbarea = ctrl as TextBox;
                        val = tbarea.Text = o.ToString();
                        break;
                    case InputType.DateTimePicker:
                        DateTimePicker dtp = ctrl as DateTimePicker;
                        val = dtp.Value = (DateTime)o;
                        break;
                    case InputType.Calendar:
                        MonthCalendar calendar = ctrl as MonthCalendar;
                        val = o.ToString();
                        if (val.ToString().Contains("=>"))
                        {
                            string[] tmps = val.ToString().Split("=>".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                            calendar.SelectionStart = DateTime.Parse(tmps[0]);
                            DateTime end = DateTime.Parse(tmps[1]);
                            calendar.MaxSelectionCount = Convert.ToInt32((end - calendar.SelectionStart).TotalDays) + 1;
                            calendar.SelectionEnd = DateTime.Parse(tmps[1]);
                        }
                        else
                        {
                            calendar.SelectionStart = DateTime.Parse(val.ToString());
                        }
                        break;
                    case InputType.Number:
                        NumericUpDown num = ctrl as NumericUpDown;
                        val = num.Value = decimal.Parse(o.ToString());
                        break;
                    case InputType.CheckBoxs:
                        List<CheckBox> cbxlist = ctrl.Tag as List<CheckBox>;
                        val = o.ToString();
                        string[] vals = val.ToString().Split(",".ToArray());
                        foreach(CheckBox cbx in cbxlist)
                        {
                            string key = "";
                            if (cbx.Tag.GetType() == typeof(string))
                            {
                                key = cbx.Tag.ToString();
                            }
                            else
                            {
                                key = ((OptionItem)cbx.Tag).Key;
                            }
                            cbx.Checked = vals.Contains(key);
                        }
                        break;
                    case InputType.CheckBoxList:
                        CheckedListBox chkboxList = ctrl as CheckedListBox;
                        val = o.ToString();
                        string[] clvals = val.ToString().Split(",".ToArray());
                        for(int i =0; i<chkboxList.Items.Count;i++)
                        {
                            var oc = chkboxList.Items[i];
                            string key = "";
                            
                            if (oc.GetType() == typeof(string))
                            {
                                key = oc.ToString();
                            }
                            else
                            {
                                key = ((OptionItem)oc).Key;
                            }
                            chkboxList.SetItemChecked(i, clvals.Contains(key));
                        }
                        break;
                    case InputType.RadioButtons:
                        List<RadioButton> rblist = ctrl.Tag as List<RadioButton>;
                        val = o.ToString();
                        foreach (RadioButton cbx in rblist)
                        {
                            string key = "";
                            if (cbx.Tag.GetType() == typeof(string))
                            {
                                key = cbx.Tag.ToString();
                            }
                            else
                            {
                                key = ((OptionItem)cbx.Tag).Key;
                            }
                            cbx.Checked = (val.ToString() == key);
                        }
                        break;
                    case InputType.ComboBox:
                        ComboBox cblist = ctrl as ComboBox;
                        val = o.ToString();
                        for (int i = 0; i < cblist.Items.Count; i++)
                        {
                            var oc = cblist.Items[i];
                            string key = "";

                            if (oc.GetType() == typeof(string))
                            {
                                key = oc.ToString();
                            }
                            else
                            {
                                key = ((OptionItem)oc).Key;
                            }
                            if((val.ToString() == key))
                            {
                                cblist.SelectedIndex = i;
                            }
                        }
                        break;
                    case InputType.Switch:
                        CheckBox cbox = ctrl as CheckBox;
                        val = o;
                        cbox.Checked = (bool)val;
                        break;
                }
                Value = val;
            }
            catch { }
        }

        public void Init()
        {
            Padding = new Padding(0, formOptions.VerticalPadding, 0, formOptions.VerticalPadding);
            label1.Text = Options.Caption;
            label1.Width = formOptions.LabelWidth;
            label1.TextAlign = formOptions.LabelAlignment;
            Name = Options.Name;

            switch (Options.InputType)
            {
                case InputType.TextBox:
                    ctrl = GetTextbox();
                    break;
                case InputType.TextArea:
                    ctrl = GetTextArea();
                    break;
                case InputType.DateTimePicker:
                    ctrl = GetDateTimePicker();
                    break;
                case InputType.Calendar:
                    ctrl = GetCalendar();
                    break;
                case InputType.Number:
                    ctrl = GetNumber();
                    break;
                case InputType.CheckBoxList:
                    ctrl = GetCheckBoxList();
                    break;
                case InputType.CheckBoxs:
                    ctrl = GetCheckBoxs();
                    break;
                case InputType.RadioButtons:
                    ctrl = GetRadioButtons();
                    break;
                case InputType.ComboBox:
                    ctrl = GetComboBox();
                    break;
                case InputType.Switch:
                    ctrl = GetSwitch();
                    break;
            }
            panel1.Controls.Add(ctrl);
            if (Options.Height == 0)
            {
                Options.Height = ctrl.Height + formOptions.VerticalPadding * 2;
            }
            ctrl.Dock = DockStyle.Fill;
            Height = Options.Height;
        }
        private TextBox GetTextbox()
        {
            TextBox tb = new TextBox();
            if (Options.Rules.ContainsKey("maxlength"))
            {
                tb.MaxLength = (int)Options.Rules["maxlength"];
            }
            if (Options.Options.ContainsKey("password"))
            {
                string psw = Options.Options["password"].ToString();
                tb.PasswordChar = psw.Length > 0 ? psw[0] : '*';
            }
            tb.TextChanged +=(o,ev)=> {
                this.Value = tb.Text;
                OnValueChanged(this, ev);
            } ;
            return tb;
        }

        private TextBox GetTextArea()
        {
            TextBox tb = new TextBox();
            tb.Multiline = true;
            if (Options.Rules.ContainsKey("maxlength"))
            {
                tb.MaxLength = (int)Options.Rules["maxlength"];
            }
            tb.TextChanged += (o, ev) => {
                this.Value = tb.Text;
                OnValueChanged(this, ev);
            };
            return tb;
        }
        private DateTimePicker GetDateTimePicker()
        {
            DateTimePicker dtp = new DateTimePicker();
            if (Options.Options.ContainsKey("format"))
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = Options.Options["format"].ToString();
            }
            if (Options.Rules.ContainsKey("min"))
            {
                dtp.MinDate = DateTime.Parse(Options.Rules["min"].ToString());
            }
            if (Options.Rules.ContainsKey("max"))
            {
                dtp.MaxDate = DateTime.Parse(Options.Rules["max"].ToString());
            }
            dtp.ValueChanged += (o, ev) =>
            {
                this.Value = dtp.Value;
                OnValueChanged(this, ev);
            };
            return dtp;
        }

        private MonthCalendar GetCalendar()
        {

            MonthCalendar calendar = new MonthCalendar();
            ContextMenu ctxMenus = new ContextMenu();
            ctxMenus.MenuItems.Add(new MenuItem("转到今天", (o, ev) =>
            {
                calendar.SetDate(calendar.TodayDate);
            }));
            ctxMenus.MenuItems.Add(new MenuItem("开始日期", (o, ev) =>
            {
                DateTime time = (DateTime)calendar.Tag;
                if (time <= DateTime.MinValue)
                {
                    return;
                }
                calendar.MaxSelectionCount = Convert.ToInt32((calendar.SelectionEnd - time).TotalDays) + 1;
                calendar.SelectionStart = time;
            }));
            ctxMenus.MenuItems.Add(new MenuItem("结束日期", (o, ev) =>
            {
                DateTime time = (DateTime)calendar.Tag;
                if (time <= DateTime.MinValue)
                {
                    return;
                }
                calendar.MaxSelectionCount = Convert.ToInt32((time - calendar.SelectionStart).TotalDays) + 1;
                calendar.SelectionEnd = time;
            }));
            calendar.MouseDown += (o, ev) =>
            {
                calendar.Tag = calendar.HitTest(ev.Location).Time;
            };
            ctxMenus.Popup += (o, ev) =>
            {
                DateTime time = (DateTime)calendar.Tag;
                if (time <= DateTime.MinValue)
                {
                    ctxMenus.MenuItems[1].Enabled = false;
                    ctxMenus.MenuItems[2].Enabled = false;
                }
                else
                {
                    ctxMenus.MenuItems[1].Enabled = true;
                    ctxMenus.MenuItems[2].Enabled = true;
                    if (time > calendar.SelectionEnd)
                    {
                        ctxMenus.MenuItems[1].Enabled = false;
                    }
                    if (time < calendar.SelectionStart)
                    {
                        ctxMenus.MenuItems[2].Enabled = false;
                    }
                }
            };
            calendar.ContextMenu = ctxMenus;
            if (Options.Rules.ContainsKey("min"))
            {
                calendar.MinDate = DateTime.Parse(Options.Rules["min"].ToString());
            }
            if (Options.Rules.ContainsKey("max"))
            {
                calendar.MaxDate = DateTime.Parse(Options.Rules["max"].ToString());
            }
            calendar.DateChanged += (o, ev) =>
            {
                if (ev.Start.ToShortDateString() == ev.End.ToShortDateString())
                {

                    this.Value = ev.Start.ToString();
                }
                else
                {
                    this.Value = ev.Start.ToString() + "=>" + ev.End.ToString();
                }
                OnValueChanged(this, ev);
            };
            return calendar;
        }

        private NumericUpDown GetNumber()
        {
            NumericUpDown num = new NumericUpDown();
            if (Options.Rules.ContainsKey("min"))
            {
                num.Minimum = decimal.Parse(Options.Rules["min"].ToString());
            }
            if (Options.Rules.ContainsKey("max"))
            {
                num.Maximum = decimal.Parse(Options.Rules["max"].ToString());
            }
            if (Options.Options.ContainsKey("thousands"))
            {
                num.ThousandsSeparator = (bool)Options.Options["thousands"];
            }
            
            num.ValueChanged += (o, ev) =>
            {
                this.Value = num.Value;
                OnValueChanged(this, ev);
            };
            return num;
        }

        private CheckedListBox GetCheckBoxList()
        {
            CheckedListBox chkboxList = new CheckedListBox();
            chkboxList.CheckOnClick = true;
            if (Options.Options.ContainsKey("options"))
            {
                var os = Options.Options["options"];
                if (os.GetType() == typeof(string))
                {
                    foreach (string s in os.ToString().Split(",|".ToCharArray()))
                    {
                        chkboxList.Items.Add(s);
                    }
                }
                else
                {
                    List<OptionItem> items = os.GetType() == typeof(ArrayList) ? OptionItem.GetList((ArrayList)os) : os as List<OptionItem>;
                    chkboxList.Items.AddRange(items.ToArray());
                }
            }
            //数据修改事件
            chkboxList.SelectedValueChanged += (o, ev) => {
                List<string> val = new List<string>();
                foreach (var item in chkboxList.CheckedItems)
                {
                    if (item.GetType() == typeof(string))
                    {
                        val.Add(item.ToString());
                    }
                    else
                    {
                        OptionItem oi = item as OptionItem;
                        val.Add(oi.Key);
                    }
                }
                Value = string.Join(",", val.ToArray());
                OnValueChanged(this, ev);
            };
            return chkboxList;
        }
        private Panel GetCheckBoxs()
        {
            Panel pan = new Panel();
            if (Options.Options.ContainsKey("options"))
            {
                var os = Options.Options["options"];
                List<CheckBox> cbxlist = new List<CheckBox>();
                //获取选择项
                //字符串，以,或|切分
                if (os.GetType() == typeof(string))
                {
                    foreach (string s in os.ToString().Split(",|".ToCharArray()))
                    {
                        CheckBox cbx = new CheckBox();
                        cbx.Text = s;
                        cbx.Tag = s;
                        cbxlist.Add(cbx);
                    }
                }
                //数据，使用OptionItem对像
                else
                {
                    List<OptionItem> items = os.GetType() == typeof(ArrayList) ? OptionItem.GetList((ArrayList)os) : os as List<OptionItem>;
                    foreach (OptionItem item in items)
                    {
                        CheckBox cbx = new CheckBox();
                        cbx.Text = item.Name;
                        cbx.Tag = item.Key;
                        cbxlist.Add(cbx);
                    }
                }
                pan.Tag = cbxlist;
                List<Panel> panlist = new List<Panel>();
                int col = cbxlist.Count;
                //默认为自动调整尺寸
                bool autosize = true;
                //计算一行有几列
                if (Options.Options.ContainsKey("columns"))
                {
                    //指定列后，列宽度为平均宽度
                    autosize = false;
                    col = (int)Options.Options["columns"];
                }
                //添加控件
                for(int i = 0, row = -1; i < cbxlist.Count; i++)
                {
                    //添加行
                    if (i % col == 0)
                    {
                        row++;
                        panlist.Add(new Panel());
                        panlist[row].Padding = new Padding(0, (i==0?0: formOptions.VerticalPadding), 0, 0);
                        int lineheight = cbxlist[0].Height;
                        panlist[row].Height = lineheight + (i == 0 ? 0 : formOptions.VerticalPadding);
                    }
                    //修改数据
                    cbxlist[i].CheckedChanged+= (o, ev) => {
                        List<string> val = new List<string>();
                        foreach (var oc in cbxlist)
                        {
                            CheckBox cbx = oc as CheckBox;
                            if (cbx.Checked)
                            {
                                val.Add(cbx.Tag.ToString());
                            }
                        }
                        Value = string.Join(",", val.ToArray());
                        OnValueChanged(this, ev);
                    };
                    //设置样式
                    cbxlist[i].Dock = DockStyle.Left;
                    cbxlist[i].AutoSize = autosize;
                    panlist[row].Controls.Add(cbxlist[i]);
                    cbxlist[i].BringToFront();
                }
                int h = 0;
                //添加行到主控件
                foreach(var op in panlist)
                {
                    Panel p = op as Panel;
                    if (!autosize)
                    {
                        p.SizeChanged+= (o, ev) => {
                            foreach(var oc in p.Controls)
                            {
                                CheckBox cbx = oc as CheckBox;
                                cbx.Width = p.Width / col;
                            }
                        };
                    }
                    pan.Controls.Add(p);
                    p.Dock = DockStyle.Top;
                    p.BringToFront();
                    h += p.Height;
                }
                pan.Height = h;
            }
            return pan;
        }

        private Panel GetRadioButtons()
        {
            Panel pan = new Panel();
            if (Options.Options.ContainsKey("options"))
            {
                var os = Options.Options["options"];
                List<RadioButton> rblist = new List<RadioButton>();
                //获取选择项
                //字符串，以,或|切分
                if (os.GetType() == typeof(string))
                {
                    foreach (string s in os.ToString().Split(",|".ToCharArray()))
                    {
                        RadioButton rbtn = new RadioButton();
                        rbtn.Text = s;
                        rbtn.Tag = s;
                        rblist.Add(rbtn);
                    }
                }
                //数据，使用OptionItem对像
                else
                {
                    List<OptionItem> items = os.GetType() == typeof(ArrayList) ? OptionItem.GetList((ArrayList)os) : os as List<OptionItem>;
                    foreach (OptionItem item in items)
                    {
                        RadioButton rbtn = new RadioButton();
                        rbtn.Text = item.Name;
                        rbtn.Tag = item.Key;
                        rblist.Add(rbtn);
                    }
                }
                pan.Tag = rblist;
                List<Panel> panlist = new List<Panel>();
                int col = rblist.Count;
                //默认为自动调整尺寸
                bool autosize = true;
                //计算一行有几列
                if (Options.Options.ContainsKey("columns"))
                {
                    //指定列后，列宽度为平均宽度
                    autosize = false;
                    col = (int)Options.Options["columns"];
                }
                //添加控件
                for (int i = 0, row = -1; i < rblist.Count; i++)
                {
                    //添加行
                    if (i % col == 0)
                    {
                        row++;
                        panlist.Add(new Panel());
                        panlist[row].Padding = new Padding(0, (i == 0 ? 0 : formOptions.VerticalPadding), 0, 0);
                        int lineheight = rblist[0].Height;
                        panlist[row].Height = lineheight + (i == 0 ? 0 : formOptions.VerticalPadding);
                    }
                    //修改数据
                    rblist[i].CheckedChanged += (o, ev) => {
                        RadioButton rbtn = o as RadioButton;
                        Value = rbtn.Tag;
                        if (rbtn.Checked)
                        {
                            foreach(RadioButton tmp in rblist)
                            {
                                if (tmp.Tag != rbtn.Tag)
                                {
                                    tmp.Checked = false;
                                }
                            }
                        }
                        OnValueChanged(this, ev);
                    };
                    //设置样式.
                    rblist[i].Dock = DockStyle.Left;
                    rblist[i].AutoSize = autosize;
                    panlist[row].Controls.Add(rblist[i]);
                    rblist[i].BringToFront();
                }
                int h = 0;
                //添加行到主控件
                foreach (var op in panlist)
                {
                    Panel p = op as Panel;
                    if (!autosize)
                    {
                        p.SizeChanged += (o, ev) => {
                            foreach (var oc in p.Controls)
                            {
                                RadioButton rbtn = oc as RadioButton;
                                rbtn.Width = p.Width / col;
                            }
                        };
                    }
                    pan.Controls.Add(p);
                    p.Dock = DockStyle.Top;
                    p.BringToFront();
                    h += p.Height;
                }
                pan.Height = h;
            }
            return pan;
        }

        private ComboBox GetComboBox()
        {
            ComboBox cblist = new ComboBox();
            cblist.DropDownStyle = ComboBoxStyle.DropDownList;
            if (Options.Options.ContainsKey("options"))
            {
                var os = Options.Options["options"];
                if (os.GetType() == typeof(string))
                {
                    foreach (string s in os.ToString().Split(",|".ToCharArray()))
                    {
                        cblist.Items.Add(s);
                    }
                }
                else
                {
                    List<OptionItem> items = os.GetType() == typeof(ArrayList) ? OptionItem.GetList((ArrayList)os) : os as List<OptionItem>;
                    cblist.Items.AddRange(items.ToArray());
                }
            }
            //数据修改事件
            cblist.SelectedValueChanged += (o, ev) => {
                var item = cblist.SelectedItem;
                string val = "";
                if (item.GetType() == typeof(string))
                {
                    val = item.ToString();
                }
                else
                {
                    OptionItem oi = item as OptionItem;
                    val = oi.Key;
                }
                Value = val;
                OnValueChanged(this, ev);
            };
            return cblist;
        }
        private CheckBox GetSwitch()
        {
            CheckBox cbx = new CheckBox();

            cbx.CheckedChanged += (o, ev) => {
                this.Value = cbx.Checked;
                OnValueChanged(this, ev);
            };
            return cbx;
        }
    }
}
