using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormControls
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
                    case InputType.Number:
                        NumericUpDown num = ctrl as NumericUpDown;
                        val = num.Value = decimal.Parse(o.ToString());
                        break;
                   // case InputType.CheckBoxs:
                        
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
                case InputType.Number:
                    ctrl = GetNumber();
                    break;
                case InputType.CheckBoxs:
                    ctrl = GetCheckBoxs();
                    break;
            }
            ctrl.Dock = DockStyle.Fill;
            panel1.Controls.Add(ctrl);
            if (Options.Height == 0)
            {
                Options.Height = ctrl.Height + formOptions.VerticalPadding * 2;
            }
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
                dtp.MinDate = (DateTime)Options.Rules["min"];
            }
            if (Options.Rules.ContainsKey("max"))
            {
                dtp.MaxDate = (DateTime)Options.Rules["max"];
            }
            dtp.ValueChanged += (o, ev) =>
            {
                this.Value = dtp.Value;
                OnValueChanged(this, ev);
            };
            return dtp;
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
            num.ValueChanged += (o, ev) =>
            {
                this.Value = num.Value;
                OnValueChanged(this, ev);
            };
            return num;
        }

        private CheckedListBox GetCheckBoxs()
        {
            CheckedListBox chkboxList = new CheckedListBox();
            if (Options.Options.ContainsKey("options"))
            {
                chkboxList.Items.Add("aaaa");
                chkboxList.Items.Add("bbbbb");
            }
            return chkboxList;
        }
    }
}
