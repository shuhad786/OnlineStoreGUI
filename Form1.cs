using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OnlineStoreLibrary;
using System.Data.SqlClient;

namespace OnlineStoreGUI
{
    public partial class Form1 : Form
    {
        BindingSource InputSource = new();
        BindingSource OutPutSource = new();
        OnlineStore OnlineAccess = new();
        SqlConnection CN = new(@"Data Source=DESKTOP-LAIUVSI\SQLEXPRESS;Initial Catalog=Inventory;Integrated Security=True");
        

        public Form1()
        {
            InitializeComponent();
        }

        // Adding a product via button click
        private void Add_button_Click(object sender, EventArgs e)
        {
            try
            {
                // Adds values from text box to the list 
                Products Input = new(int.Parse(First_textBox.Text), decimal.Parse(Second_textBox.Text), (Third_textBox.Text), (Fourth_textBox.Text));
                OnlineAccess.ProductsList.Add(Input);
                First_textBox.Text = "";
                Second_textBox.Text = "";
                Third_textBox.Text = "";
                Fourth_textBox.Text = "";
                Add_label.Text = "Added new product";
                Add_label.ForeColor = Color.Blue;
                InputSource.ResetBindings(false); // resets the list box
                Connect_label.Text = "Connected";
                Connect_label.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // to display product to check out display box
        private void CheckOut_button_Click(object sender, EventArgs e)
        {
            Products added = (Products)Inventory_listBox.SelectedItem;
            OnlineAccess.OnlineStoreList.Add(added);
            decimal total = OnlineAccess.CheckOut();
            CheckOut_label.Text = "R" + total.ToString();
            OutPutSource.ResetBindings(false);
            OnlineAccess.PrintReceipt(); // prints a text file to path file in OnlineStore class

           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Added new product display
            InputSource.DataSource = OnlineAccess.ProductsList;
            Inventory_listBox.DataSource = InputSource;
            Inventory_listBox.DisplayMember = ToString();
            


            // Check out product display to box
            
            OutPutSource.DataSource = OnlineAccess.OnlineStoreList;
            CheckOut_listBox.DataSource = OutPutSource;
            CheckOut_listBox.DisplayMember = ToString();
           

            
            Connect_label.ForeColor = Color.Red;
        }

        private void Connect_button_Click(object sender, EventArgs e)
        {
            try
            {
               

                CN.Open();
                string selected1 = Inventory_listBox.SelectedItem.ToString();
                string q = "select * from Input_table where Brand='" + selected1 + "'";
               
                SqlDataAdapter SDA1 = new(q, CN);
                DataTable DT1 = new();
                SDA1.Fill(DT1);
                Connect_dataGridView.DataSource = InputSource;
                InputSource.ResetBindings(false);

                string selected2 = CheckOut_listBox.SelectedItem.ToString();
                string r = "select * from Input_table where Brand='" + selected2 + "'";
                SqlDataAdapter SDA2 = new(r, CN);
                DataTable DT2 = new();
                SDA2.Fill(DT2);
                CheckOut_dataGridView.DataSource = OutPutSource;
                OutPutSource.ResetBindings(false);

                Connect_label.Text = "Updated";
                Connect_label.ForeColor = Color.Goldenrod;

                CN.Close(); // closes connection
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            }
            // Only allows numbers to be entered in First_textBox
            private void First_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // Only allows numbers to be entered in Second_textBox
        private void Second_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        // Only allows Letters to be entered in Third_textBox
        private void Third_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        
        // Only allows Letters to be entered in Fourth_textBox
        private void Fourth_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


      

       
    }
}
