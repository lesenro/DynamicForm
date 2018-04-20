using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormControls;

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
            dynamic lg = new
            {
                Id = 111,
                Password = "123",
                Text = "444444",
                UserName = "uuuuuu",
                DateTimePicker = DateTime.Parse("2016-02-22 16:59"),
                Number=80
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
    }
}
