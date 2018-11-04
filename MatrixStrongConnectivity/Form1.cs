using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MatrixStrongConnectivity
{
	public partial class Form1 : Form
	{
		//private DGMLWriter obW;
		private ReaderWarshall obRW;
		private string[] matrixStrOut;
		private string[,] matrixStrInput;
		private string filename;
		int numb;
		private bool button1Status = false; 
		//private DataTable dTable;
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			//XpsDocument ob = new 
		
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//richTextBox1.Clear();
			if ((textBox1.Text != "") && (Int32.TryParse(textBox1.Text, out numb)) && (comboBox1.Text != ""))
			{
				saveFileDialog1.Filter = "dgml files (*.dgml)|*.dgml";
				saveFileDialog1.FilterIndex = 2;
				saveFileDialog1.RestoreDirectory = true;
				if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
					return;

				clearDataGrid(dataGridView1);
				filename = saveFileDialog1.FileName;

				obRW = new ReaderWarshall();
				obRW.CreateMatrix(filename, Convert.ToInt32(textBox1.Text));

				obRW.StreamAxis(filename, "Node");
				//richTextBox1.AppendText(obRW.advertiseStrMatrix());
				displayDataGrid(dataGridView1);
				button1Status = true;
			} else
			{
				MessageBox.Show("Введите размер матрицы и выберите тип!");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (button1Status)
			{
				//obRW.getRichText(richTextBox1.Text);
				obRW.getDataGrid(readDataGrid(dataGridView1));
				//richTextBox2.AppendText(obRW.advertiseStrMatrix());
				clearDataGrid(dataGridView2);
				displayDataGrid(dataGridView2);
				obRW.writeUserMatrix(filename);
				filename = filename.Replace(".", "WarshallResult.");
				obRW.Warshall(filename, comboBox1.Text);
				//richTextBox2.Clear();

				clearDataGrid(dataGridView2);
				displayDataGrid(dataGridView2);
				//richTextBox2.AppendText(obRW.advertiseStrMatrix());
			} else
			{
				MessageBox.Show("Создайте матрицу");
			}

		}

		private void clearDataGrid(DataGridView dGrid)
		{
			dGrid.Rows.Clear();
			dGrid.Columns.Clear();
			dGrid.Refresh();
		}
		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void displayDataGrid(DataGridView dGrid)
		{
			matrixStrOut = new string[obRW.getQuantity()];
			matrixStrInput = obRW.advertiseStrMatrix();

			for (int i = 0; i < obRW.getQuantity(); i++)
			{
				dGrid.Columns.Add(i.ToString(), (i + 1).ToString());
				for (int j = 0; j < obRW.getQuantity(); j++)
				{
					matrixStrOut[j] = matrixStrInput[j, i];
					if (i == 0)
					{
						dGrid.Rows.Add();
						dGrid.Rows[j].HeaderCell.Value = (j + 1).ToString();
					}
					dGrid.Rows[j].Cells[i].Value = matrixStrOut[j];
				}
			}
		}

		private string[] readDataGrid(DataGridView dGrid)
		{
			string[] matrix = new string[obRW.getQuantity()*obRW.getQuantity()];
			int k = 0;
			foreach (DataGridViewRow row in dGrid.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					matrix[k++] = cell.Value.ToString();
				}
			}
			return matrix;
		}
	}
}
