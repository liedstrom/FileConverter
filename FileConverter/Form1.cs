using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace FileConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                var TextList = ReadFile();

                WriteXmlFile(TextList);

            }
            catch (Exception ex)
            {
                label1.Text = "XML-fil misslyckades.";
                return;
            }

            label1.Text = "XML-fil skapad.";

        }


        static private List<string> ReadFile()
        {
            var tempList = new List<string>();

            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\Test\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of selected file
                    var filePath = openFileDialog.FileName;

                    // Read the file and add lines to the list
                    foreach (string line in File.ReadLines(filePath))
                    {
                        tempList.Add(line);
                    }

                }

            }

            return tempList;
        }

        static private void WriteXmlFile(List<string> stringList)
        {
            var writer = new XmlTextWriter("C:\\Test\\PeopleList.xml", Encoding.UTF8);
            writer.WriteStartDocument(true);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteStartElement("people");

            bool bPeople = false;
            bool bFamily = false;

            // Read the lines in the list
            foreach (string line in stringList)
            {
                var lineArray = line.Split('|');

                switch (lineArray[0])
                {
                    case "P":
                        if (bFamily)
                        {
                            writer.WriteEndElement();

                            bFamily = false;
                        }

                        if (bPeople)
                            writer.WriteEndElement();

                        bPeople = true;

                        writer.WriteStartElement("person");

                        writer.WriteStartElement("firstname");
                        writer.WriteString(lineArray[1]);
                        writer.WriteEndElement();

                        if (lineArray.Length >= 3)
                        {
                            writer.WriteStartElement("lastname");
                            writer.WriteString(lineArray[2]);
                            writer.WriteEndElement();
                        }

                        break;

                    case "A":
                        writer.WriteStartElement("address");

                        writer.WriteStartElement("street");
                        writer.WriteString(lineArray[1]);
                        writer.WriteEndElement();

                        if (lineArray.Length >= 3)
                        {
                            writer.WriteStartElement("city");
                            writer.WriteString(lineArray[2]);
                            writer.WriteEndElement();
                        }

                        if (lineArray.Length >= 4)
                        {
                            writer.WriteStartElement("zip");
                            writer.WriteString(lineArray[3]);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();

                        break;

                    case "T":
                        writer.WriteStartElement("phone");

                        writer.WriteStartElement("mobile");
                        writer.WriteString(lineArray[1]);
                        writer.WriteEndElement();

                        if (lineArray.Length >= 3)
                        {
                            writer.WriteStartElement("landline");
                            writer.WriteString(lineArray[2]);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();

                        break;

                    case "F":
                        if (bFamily)
                        {
                            writer.WriteEndElement();
                        }

                        bFamily = true;

                        writer.WriteStartElement("family");

                        writer.WriteStartElement("name");
                        writer.WriteString(lineArray[1]);
                        writer.WriteEndElement();

                        if (lineArray.Length >= 3)
                        {
                            writer.WriteStartElement("born");
                            writer.WriteString(lineArray[2]);
                            writer.WriteEndElement();
                        }

                        break;

                    default:
                        throw new Exception();

                }

            }


            if (bFamily)
                writer.WriteEndElement();

            if (bPeople)
                writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

        }

    }
}
