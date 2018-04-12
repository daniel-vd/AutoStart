using CSharpAnalytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace AutoStart {
    public partial class AutoStart : Form {

        FilesManager filesManager = new FilesManager();

        string id = null;
        string path = null;
        string programsFile;
        string name = null;

        string input = "";

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AutoStart());
        }

        public AutoStart() {
            InitializeComponent();


            //Analytics
            AutoMeasurement.Instance = new WinFormAutoMeasurement();
            AutoMeasurement.Start(new MeasurementConfiguration("UA-117174081-1"));

            AutoMeasurement.Client.TrackScreenView("Main screen");

            programsFile = filesManager.programsFile;

            //Initialize datagridview
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("path", "File path");
            dataGridView1.Columns.Add("name", "File name");

            //We don't want the id column to be visible, but for debugging purposes it might come in handy,
            //So we hide it
            dataGridView1.Columns["id"].Visible = false;

            //Make sure all files exist
            filesManager.CheckFiles();

            //Load programs file
            XmlDocument doc = new XmlDocument();
            doc.Load(programsFile);

            //Get data from programs file
            foreach (XmlNode node in doc.SelectNodes("/Programs/Program")) {
                id = node.Attributes["id"].InnerText;
                name = node.SelectSingleNode("name").InnerText;
                path = node.SelectSingleNode("path").InnerText;
                ProgramList.programList.Add(new string[] { id, path, name });
            }

            //Populate datagridview
            foreach (string[] program in ProgramList.programList) {
                dataGridView1.Rows.Add(program[0], program[1], program[2]);
            }



        }

        public void SetTextLabel1(string text) {
            this.label1.Text = text;
        }

        //Add program
        private void button1_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                String programPath = openFileDialog1.FileName;
                int id = filesManager.getID() + 1;
                String programName = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);

                //Add program to datagridview
                dataGridView1.Rows.Add(id, programPath, programName);

                //Add program to programs file
                filesManager.AddProgramsRow(programPath, id, programName);
            }
            AutoMeasurement.Client.TrackEvent("Add row", "Row Editing");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void button2_Click(object sender, EventArgs e) {
            //Delete program
            if (MessageBox.Show("Are your sure you want to delete this program? This action cannot be undone!", "Delete row", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }


            //Get index of row
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            //Get selected row by index
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
            //Get value of cell in column 'id' in row
            string id = selectedRow.Cells["id"].Value.ToString();

            XmlDocument doc = new XmlDocument();
            doc.Load(programsFile);

            XmlNode node = doc.SelectSingleNode("/Programs/Program[@id='" + id + "']");
            node.ParentNode.RemoveChild(node);

            doc.Save(programsFile);

            dataGridView1.Rows.Remove(selectedRow);

            AutoMeasurement.Client.TrackEvent("Delete row", "Row Editing");
        }

        //Edit name of row
        private void button3_Click(object sender, EventArgs e) {

            //Get index of row
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            //Get selected row by index
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];
            //Get value of cell in column 'id' in row
            string id = selectedRow.Cells["id"].Value.ToString();

            //Show inputDialog
            if (ShowInputDialog(selectedRow.Cells["name"].Value.ToString()).ToString().ToUpper() != "OK")
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(programsFile);

            XmlNode node = doc.SelectSingleNode("/Programs/Program[@id='" + id + "']");
            node.SelectSingleNode("name").InnerText = input;

            //XmlElement element = (XmlElement) node.SelectSingleNode("/Programs/Program/name");

            //XElement element2 = XElement.Parse(element.OuterXml);

            //element2.Value = "krakaka";

            selectedRow.Cells["name"].Value = input;

            doc.Save(programsFile);

            AutoMeasurement.Client.TrackEvent("Edit row", "Row Editing");
        }

        private DialogResult ShowInputDialog(string input) {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "Edit file name";
            inputBox.StartPosition = FormStartPosition.CenterParent;

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            this.input = textBox.Text;
            return result;
        }
    }
}
