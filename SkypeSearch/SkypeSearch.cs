using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SkypeCore;
using System.IO;

namespace SkypeSearch
{
    public partial class SkypeSearchForm : Form
    {
        public SkypeSearchForm()
        {
            InitializeComponent();
            LoadSkypeAccounts();
            
        }


        private void LoadSkypeAccounts()
        {
            accountDropDown.Items.AddRange(Utils.GetSkypeAccountsNames());
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.DefaultAccount) && accountDropDown.Items.Count > 0)
            {
                Properties.Settings.Default.DefaultAccount = accountDropDown.Text = accountDropDown.Items[0].ToString();
                Properties.Settings.Default.Save();
            }
            else
            {
                accountDropDown.Text = Properties.Settings.Default.DefaultAccount;
            }

        }

        private void Search()
        {
            try
            {
                string query = string.Empty;
                string account = string.Empty;

                Invoke((MethodInvoker) delegate
                {
                    query = searchTextBox.Text;
                    account = accountDropDown.Text;
                    webBrowser1.DocumentText = "Searching...";
                    accountDropDown.Enabled = searchTextBox.Enabled = buttonSearch.Enabled = false;
                    
                });

                string text = Utils.SearchAndFormat(query, account);

                Invoke((MethodInvoker) delegate
                {
                    webBrowser1.DocumentText = text;
                });

            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker) delegate
                {
                    webBrowser1.DocumentText = "Error: " + ex.ToString();
                });
            }
            finally
            {
                Invoke((MethodInvoker) delegate
                {
                    accountDropDown.Enabled = searchTextBox.Enabled = buttonSearch.Enabled = true;
                    searchTextBox.Focus();
                });
            }

        }

        private void SearchClick(object sender, EventArgs e) { 
                
            new Thread(Search).Start();

        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchClick(sender, e);
            }
        }
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private void button1_Click(object sender, EventArgs e)
        {
            var dataObject = new DataObject();
            TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - Epoch;
            var messagePlain = string.Format(
                    "[{0:hh:mm:ss}] {1}: {2}",
                    DateTime.Now.ToUniversalTime(),
                    "Vic H",
                    "blabla blabla");
            var messageXml = string.Format(
                    "<quote author=\"{0}\" timestamp=\"{1}\">{2}</quote>",
                    "Vic H",
                    (long)timeSpan.TotalSeconds,
                    "blabla blabla");

            dataObject.SetData("System.String", messagePlain);
            dataObject.SetData("UnicodeText", messagePlain);
            dataObject.SetData("Text", messagePlain);
            dataObject.SetData("SkypeMessageFragment", new MemoryStream(Encoding.UTF8.GetBytes(messageXml)));
            dataObject.SetData("Locale", new MemoryStream(BitConverter.GetBytes(CultureInfo.CurrentCulture.LCID)));
            dataObject.SetData("OEMText", messagePlain);

            Clipboard.SetDataObject(dataObject, true);
        }

        private void accountDropDown_TextUpdate(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultAccount = accountDropDown.Text;
            Properties.Settings.Default.Save();
        }

        private void accountDropDown_SelectedValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultAccount = accountDropDown.Text;
            Properties.Settings.Default.Save();
        }
    }
}

