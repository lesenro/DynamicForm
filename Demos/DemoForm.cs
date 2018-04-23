using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormControls;
using WinFormControls.Controls;
using WinFormControls.Models;

namespace Demos
{
    public partial class DemoForm : Form
    {
        GridForm gform;
        public DemoForm()
        {
            InitializeComponent();
        }

        private void DemoForm_Load(object sender, EventArgs e)
        {
            string json=Encoding.Default.GetString(Properties.Resources.GridFormTest);
            FormOptions options = FormOptions.FromJson(json);
            
            options.Width = panel1.ClientSize.Width;
            gform = new GridForm(options);
            panel1.Controls.Add(gform);
            gform.Dock = DockStyle.Fill;
            gform.ItemValueChanged += Gform_ItemValueChanged;
        }

        private void Gform_ItemValueChanged(object sender, EventArgs e)
        {
            FormItem item = sender as FormItem;
            var vvv = item.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login lg = new Login
            {
                Id = 111,
                Password = "123",
                Text = "444444",
                UserName = "uuuuuu",
                DateTimePicker = DateTime.Parse("2016-02-22 16:59"),
                Number = 80,
                Cbxlist1 = "k1,k2",
                Cbxlist2 = "二,四",
                Cbxlist3 = "k4,k3",
                Cbxlist4 = "一,三",
                RadioBtns1 = "k1",
                RadioBtns2 = "三",
                ComboBox1 = "k2",
                ComboBox2 = "四",
                Switch1 = true,
                Calendar= "2016-02-12=>2016-02-22"
            };
            gform.FillData(lg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var  log =gform.GetValues<Login>();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gform.FillData("UserName", "这是新的");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            gform.UpdateOptionItems("Cbxlist2", "J,Q,K,A");
            gform.UpdateOptionItems("Cbxlist4", "五,六,七,八");
            List<OptionItem> items1 = new List<OptionItem>();
            items1.Add(new OptionItem
            {
                Key="d1",
                Name="新一"
            });
            items1.Add(new OptionItem
            {
                Key = "d2",
                Name = "新2"
            });
            items1.Add(new OptionItem
            {
                Key = "d3",
                Name = "新3"
            });
            items1.Add(new OptionItem
            {
                Key = "d4",
                Name = "新4"
            });
            items1.Add(new OptionItem
            {
                Key = "d5",
                Name = "新5"
            });
            gform.UpdateOptionItems("Cbxlist1", items1);
            gform.UpdateOptionItems("Cbxlist3", items1);

            gform.UpdateOptionItems("RadioBtns1", items1);
            gform.UpdateOptionItems("RadioBtns2", "五,六,七,八");
            gform.UpdateOptionItems("ComboBox1", items1);
            gform.UpdateOptionItems("ComboBox2", "J,Q,K,A");
        }
    }
}
