using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TestADO
{
    public partial class Form1 : Form
    {
        static SqlConnection GeoConnection = new SqlConnection(@"Data Source=HOME-PC;Initial Catalog=GeoSystemDB;Integrated Security=True");
        static string query = "SELECT * FROM [Geodesist_geosystem_schema].[Geodesy]";
        static SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
        DataSet GeoDataset = new DataSet("Geodesy");
        SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
   


        public Form1()
        {
            InitializeComponent();
            GetTableNames();
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlDataAdapter1.Fill(GeoDataset, "[Geodesist_geosystem_schema].[Geodesy]");
            dataGridView1.DataSource = GeoDataset.Tables["[Geodesist_geosystem_schema].[Geodesy]"];
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                string str = GetCurrentTable(comboBox1.Text);
                string query = "SELECT * FROM " + str;
                SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                commands.DataAdapter.Update(GeoDataset.Tables[str]);
            }
            catch (Exception a)
            {

                MessageBox.Show(a.ToString());
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            
            try
            {
                string str = GetCurrentTable(comboBox1.Text);
                string query = "SELECT * FROM " + str;
                SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                GeoDataset.EndInit();
                var index = dataGridView1.CurrentRow.Index;
                GeoDataset.Tables[str].Rows[index].Delete();
                SqlDataAdapter1.Update(GeoDataset.Tables[str]);
            }
            catch(Exception a)
            {
                MessageBox.Show(a.ToString());
            }
                
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string str = (string)dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText;
            
            DataRelationCollection dr = geoSystemDBDataSet1.Relations;

            foreach(DataRelation temp in dr)
            {
                
                if(temp.ChildTable.TableName == comboBox1.Text)
                {
                    if(temp.ChildColumns[0].ColumnName == str)
                    {
                        comboBox2.Items.Add(temp.ParentTable.TableName);
                        /*string tableStr = GetCurrentTable(temp.ParentTable.TableName);
                        string query = "SELECT * FROM " + tableStr + " WHERE " + temp.ParentColumns[0].ColumnName + " = " + dataGridView1.CurrentCell.Value;
                        SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                        SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                        GeoDataset = new DataSet("Geo");
                        commands.DataAdapter.Fill(GeoDataset, tableStr);
                        dataGridView2.DataSource = GeoDataset.Tables[tableStr];*/
                        break;
                    }
                }
                else if (temp.ParentTable.TableName == comboBox1.Text && !comboBox2.Items.Contains(temp.ChildTable.TableName))
                {
                    comboBox2.Items.Add(temp.ChildTable.TableName);
                    /*string tableStr = GetCurrentTable(temp.ChildTable.TableName);
                    string query = "SELECT * FROM " + tableStr + " WHERE " + temp.ChildColumns[0].ColumnName + " = " + dataGridView1.CurrentCell.Value;
                    SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                    SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                    GeoDataset = new DataSet("Geodesy");
                    commands.DataAdapter.Fill(GeoDataset, tableStr);
                    dataGridView2.DataSource = GeoDataset.Tables[tableStr];*/
                  

                }
                
            }

            comboBox2.Text = comboBox2.Items[0].ToString();
        }

        public string GetCurrentColumnName()
        {
            DataRelationCollection dr = geoSystemDBDataSet1.Relations;
            foreach (DataRelation temp in dr)
            {
                if (temp.ParentTable.TableName == comboBox1.Text && temp.ChildTable.TableName == comboBox2.Text)
                {


                    return temp.ChildColumns[0].ColumnName;



                }
                else if (temp.ChildTable.TableName == comboBox1.Text && temp.ParentTable.TableName == comboBox2.Text)
                    return temp.ParentColumns[0].ColumnName;


            }
            return "";

        }
        public void GetTableNames()
        {
            comboBox1.Items.Clear();
            GeoConnection.Open();
            DataTable dt = GeoConnection.GetSchema("Tables");
            GeoConnection.Close();
            foreach(DataRow dr in dt.Rows)
            {
                if(dr.ItemArray[3].ToString()=="BASE TABLE"&&dr.ItemArray[2].ToString()!="sysdiagrams")
                {
                    comboBox1.Items.Add(dr["TABLE_NAME"]);
                }
               
                
            }
            Console.WriteLine();
        }

        public string GetCurrentTable(string str)
        {
            GeoConnection.Open();
            DataTable dt = GeoConnection.GetSchema("Tables");
            GeoConnection.Close();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.ItemArray[2].ToString().Equals(str))
                {
                    string temp = "[" + dr.ItemArray[1].ToString() + "]." + "[" + dr.ItemArray[2].ToString() + "]";
                    return temp;
                }
                

            }
            return "";
        }

        public void Clear(DataGridView dataGridView)
        {
            
            while (dataGridView.Rows.Count > 1)
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
                    dataGridView.Rows.Remove(dataGridView.Rows[i]);
        }
        

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Clear(dataGridView1);
                string str = GetCurrentTable(comboBox1.Text);
                string query = "SELECT * FROM " + str;
                SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                GeoDataset = new DataSet("Geodesy");
                commands.DataAdapter.Fill(GeoDataset, str);
                dataGridView1.DataSource = GeoDataset.Tables[str];
            }
            catch (Exception a)
            {

                MessageBox.Show(a.ToString());
            }
        }

       

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Clear(dataGridView2);
                string str = GetCurrentTable(comboBox2.Text);
                string query = "SELECT * FROM " + str + " WHERE " + GetCurrentColumnName() + " = " + dataGridView1.CurrentCell.Value;
                SqlDataAdapter SqlDataAdapter1 = new SqlDataAdapter(query, GeoConnection);
                SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);
                GeoDataset = new DataSet("G");
                commands.DataAdapter.Fill(GeoDataset, str);
                dataGridView2.DataSource = GeoDataset.Tables[str];


            }
            catch (Exception a)
            {

                MessageBox.Show(a.ToString());
            }
        }
    }
}
