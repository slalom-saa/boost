using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Slalom.Boost.VisualStudio.Forms
{
    public partial class AddItemForm : Form
    {
        private int oHeight = 0;
        public AddItemForm(string name, bool allowProperties = true)
        {
            this.InitializeComponent();

            Text = name;

            CancelButton = btnCancel;
            AcceptButton = btnOk;
            btnCancel.CausesValidation = false;

            panel1.Hide();
            oHeight = this.Height;
            this.Height = 135;

            if (!allowProperties)
            {
                checkBox1.Hide();
            }
        }

        public string ItemName
        {
            get { return txtName.Text.ToPascalCase(); }
            set { txtName.Text = value; }
        }

        public IEnumerable<string> Properties => lbProperties.Items.OfType<string>();

        private void HandleAccept(object sender, EventArgs e)
        {
            if (txtProperty.Focused && !String.IsNullOrWhiteSpace(txtProperty.Text))
            {
                lbProperties.Items.Add(txtProperty.Text.ToPascalCase());
                txtProperty.Text = "";
            }
            else if (!String.IsNullOrWhiteSpace(txtName.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void HandleCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lbProperties.Items.Add(txtProperty.Text);
            txtProperty.Text = "";
        }

        private void lbProperties_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                lbProperties.Items.Remove(lbProperties.SelectedItem);
            }
        }

        private void AddItemForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                panel1.Visible = true;
                this.Height = oHeight;
                txtProperty.Focus();
                checkBox1.Hide();
            }
            else
            {
                panel1.Visible = false;
                this.Height = 135;
            }
        }
    }
}