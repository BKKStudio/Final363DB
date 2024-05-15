using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Reflection.Emit;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
namespace Final363DB
{
    public partial class SlothLovely : Form
    {
        public SlothLovely()
        {
            InitializeComponent();
        }
        //6400502 Seksak Aranchot 

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.jet.OleDb.4.0;" +
           @"Data Source=D:\Final363\db\Database1.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter adapter = new OleDbDataAdapter();

        //Get datalists
        private void getDataListsInGridview()
        {
            try
            {
                con.Open();

                // First query to populate the DataGridView
                string sql = "SELECT * FROM laundryshopdb";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "laundryshopdb");  // Fill the DataSet

                        dataGridView1.DataSource = data.Tables["laundryshopdb"];  // Set the DataSource
                    }
                }

                // Query to count rows where paymentstatus is 'yes'
                sql = "SELECT COUNT(*) FROM laundryshopdb WHERE paymentstatus = 'ชำระแล้ว'";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                                                           // Display the count in label5
                    label5.Text = $"{count} คน";
                    chart1.Series["Series1"].Points.AddXY("ชำระแล้ว", count);
                }

                // Query to count rows where paymentstatus is 'no'
                sql = "SELECT COUNT(*) FROM laundryshopdb WHERE paymentstatus = 'ค้างชำระ'";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    chart1.Series["Series1"].Points.AddXY("ค้างชำระ", count);                                   // Display the count in label8
                    label8.Text = $"{count} คน";
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error or show a message to the user)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void getDataCheckMember()
        {

            try
            {
                con.Open();
                string sql = "SELECT COUNT(*) FROM members";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    label10.Text = $"{count} คน";                                     // Display the count in label5
                    label14.Text = $"{count} คน";

                }
            }
            catch(Exception ex)
            {

            }
            finally { con.Close(); }
        }



        private void countEntriesWithCurrentPickupDate()
        {
            try
            {
                // เปิดการเชื่อมต่อฐานข้อมูลถ้ายังไม่เปิด
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query เพื่อดึงข้อมูล pickupdate จากตาราง laundryshopdb
                string sql = "SELECT pickupdate FROM laundryshopdb";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data, "laundryshopdb");

                int count = 0;
                DateTime currentDate = DateTime.Now.Date;  // ได้เฉพาะวันที่โดยไม่มีเวลา

                foreach (DataRow row in data.Tables["laundryshopdb"].Rows)
                {
                    if (DateTime.TryParse(row["pickupdate"].ToString(), out DateTime pickupDate))
                    {
                        // เปรียบเทียบวันที่โดยไม่สนใจเวลา
                        if (pickupDate.Date == currentDate)
                        {
                            count++;
                        }
                    }
                }

                // แสดงผลจำนวนข้อมูลที่มี pickupdate ตรงกับวันที่ปัจจุบันใน label
                textBox1.Text =  Convert.ToString(count) + " คน";
                   
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
            finally
            {
                // ปิดการเชื่อมต่อในส่วนสุดท้ายถ้าการเชื่อมต่อเปิดอยู่
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }


        private void countEntriesWithCurrentdeliverydate()
        {
            try
            {
                // เปิดการเชื่อมต่อฐานข้อมูลถ้ายังไม่เปิด
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query เพื่อดึงข้อมูล pickupdate จากตาราง laundryshopdb
                string sql = "SELECT deliverydate FROM laundryshopdb";
                OleDbCommand cmd = new OleDbCommand(sql, con);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data, "laundryshopdb");

                int count = 0;
                DateTime currentDate = DateTime.Now.Date;  // ได้เฉพาะวันที่โดยไม่มีเวลา

                foreach (DataRow row in data.Tables["laundryshopdb"].Rows)
                {
                    if (DateTime.TryParse(row["deliverydate"].ToString(), out DateTime Deliverydate))
                    {
                        // เปรียบเทียบวันที่โดยไม่สนใจเวลา
                        if (Deliverydate.Date == currentDate)
                        {
                            count++;
                        }
                    }
                }

                // แสดงผลจำนวนข้อมูลที่มี pickupdate ตรงกับวันที่ปัจจุบันใน label
                textBox2.Text = Convert.ToString(count) + " คน";
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
            finally
            {
                // ปิดการเชื่อมต่อในส่วนสุดท้ายถ้าการเชื่อมต่อเปิดอยู่
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }




        private void countwash()
        {
            try
            {
                // เปิดการเชื่อมต่อฐานข้อมูลถ้ายังไม่เปิด
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query to count rows where wash is 'yes'
                string sql = "SELECT COUNT(*) FROM laundryshopdb WHERE wash = 'ซัก'";
                cmd = new OleDbCommand(sql, con);

                int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count

                // Display the count in txtbox3
                textBox3.Text = Convert.ToString(count) + "  รายการ";
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
            finally
            {
                // ปิดการเชื่อมต่อในส่วนสุดท้ายถ้าการเชื่อมต่อเปิดอยู่
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }


        private void countIrons()
        {
            try
            {
                // เปิดการเชื่อมต่อฐานข้อมูลถ้ายังไม่เปิด
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query to count rows where wash is 'yes'
                string sql = "SELECT COUNT(*) FROM laundryshopdb WHERE irons =  'รีด'";
                cmd = new OleDbCommand(sql, con);

                int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count

                // Display the count in txtbox3
                textBox4.Text = Convert.ToString(count) + "  รายการ";
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message);
            }
            finally
            {
                // ปิดการเชื่อมต่อในส่วนสุดท้ายถ้าการเชื่อมต่อเปิดอยู่
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }


        private void calculatePrices()
        {
            try
            {
                // เปิดการเชื่อมต่อฐานข้อมูลถ้ายังไม่เปิด
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                // Query to sum prices where paymentstatus is 'yes'
                string sql = "SELECT SUM(price) FROM laundryshopdb WHERE paymentstatus = 'ชำระแล้ว'";
                int totalYes = 0;
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    totalYes = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    // แสดงผลรวมใน textbox13
                    textBox13.Text = totalYes.ToString() + " บาท";
                }

                // Query to sum prices where paymentstatus is 'no'
                sql = "SELECT SUM(price) FROM laundryshopdb WHERE paymentstatus = 'ค้างชำระ'";
                int totalNo = 0;
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    totalNo = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    // แสดงผลรวมใน textbox14
                    textBox14.Text = totalNo.ToString() + " บาท";
                }
            }
            catch (Exception ex)
            {
                // แสดงข้อความแจ้งเตือนเมื่อเกิดข้อผิดพลาด
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // ปิดการเชื่อมต่อในส่วนสุดท้ายถ้าการเชื่อมต่อเปิดอยู่
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }



        //Get dataHistory
        private void getDataHistory()
        {
            try
            {
                con.Open();

                // First query to populate the DataGridView
                string sql = "SELECT name_cus, status FROM history";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "history");  // Fill the DataSet

                        dataGridView2.DataSource = data.Tables["history"];  // Set the DataSource
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error or show a message to the user)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void getDataMembersTable()
        {
            try
            {
                con.Open();

                // Prepare the SQL query
                string sql;
                if (!string.IsNullOrEmpty(textBox5.Text))
                {
                    // If textBox5 has a value, use it for searching
                    sql = "SELECT * FROM members WHERE name_member LIKE '%' + @searchValue + '%' OR tell LIKE '%' + @searchValue + '%'"
;
                }
                else
                {
                    // If textBox5 is empty, retrieve all data from members table
                    sql = "SELECT * FROM members";
                }

                // Execute the SQL query
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    // Add parameter if textBox5 has a value
                    if (!string.IsNullOrEmpty(textBox5.Text))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", textBox5.Text);
                    }

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "members");  // Fill the DataSet

                        dataGridView3.DataSource = data.Tables["members"];  // Set the DataSource
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error or show a message to the user)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        //Add Members
        private void Addmembers()
        {
            try
            {
                con.Open();

                // Prepare the SQL query to insert data into members table
                string sql = "INSERT INTO members (name_member, tell) VALUES (@nameMember, @tell)";

                // Execute the SQL query with parameters from textbox6 and textbox7
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nameMember", textBox6.Text);
                    cmd.Parameters.AddWithValue("@tell", textBox7.Text);
                    cmd.ExecuteNonQuery();  // Execute the query to insert data
                    
                }
                textBox6.Text = "";
                textBox7.Text = "";
                MessageBox.Show("Member added successfully."); // Display success message
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message); // Display error message
            }
            finally
            {
                con.Close(); // Close the connection
            }
        }






        //*********************************** หน้ารายการร ***************************************************

        //นับข้อมูล ที่ส่งคืนแล้ว จากตาราง History 

        private void Success()
        {
            try
            {
                con.Open();

                // Query to count rows where status is 'ส่งคืนแล้ว'
                string sql = "SELECT COUNT(*) FROM history WHERE status = 'ส่งคืนแล้ว'";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    label23.Text = $"{count} คน";         // Display the count in label23
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }




        private void SomSak_Wash_Load(object sender, EventArgs e)
        {
            getDataListsInGridview();
            getDataCheckMember();
            countEntriesWithCurrentPickupDate();
            countEntriesWithCurrentdeliverydate();
            countwash();
            countIrons();
            calculatePrices();
            getDataHistory();
            getDataMembersTable();
            Success();

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            getDataMembersTable();
        }

        private void Add_member_Click(object sender, EventArgs e)
        {
           
            Addmembers();
            try
            {
                con.Open();

                // First query to populate the DataGridView
                string sql = "SELECT * FROM members";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "members");  // Fill the DataSet

                        dataGridView3.DataSource = data.Tables["members"];  // Set the DataSource
                    }
                }
                sql = "SELECT COUNT(*) FROM members";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    label10.Text = $"{count} คน";                                // Display the count in label5
                    label14.Text = $"{count} คน";
          
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error or show a message to the user)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // ตรวจสอบว่าคลิกบนแถวของตาราง
                {
                    DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];
                    textBox15.Text = selectedRow.Cells["id"].Value.ToString();
                    textBox6.Text = selectedRow.Cells["name_member"].Value.ToString();
                    textBox7.Text = selectedRow.Cells["tell"].Value.ToString();
                    // เพิ่มการเรียกใช้เมธอดหรือโค้ดที่ต้องการทำงานเมื่อคลิกบนแถวของตาราง
                }
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดตามที่ต้องการ
            }

        }
        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
   
        }
        private void Edit_member_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int id = Convert.ToInt32(textBox15.Text);
                string sql = "UPDATE members SET name_member = @nameMember, tell = @tell WHERE id = @id";

                // Execute the SQL query with parameters from textbox6, textbox7, and textBox15
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nameMember", textBox6.Text);
                    cmd.Parameters.AddWithValue("@tell", textBox7.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();  // Execute the query to update data
                }
                MessageBox.Show("Update Data Success");
                sql = "SELECT * FROM members";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "members");  // Fill the DataSet

                        dataGridView3.DataSource = data.Tables["members"];  // Set the DataSource
                    }
                }
                sql = "SELECT COUNT(*) FROM members";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    label10.Text = $"{count} คน";                                // Display the count in label5
                    label14.Text = $"{count} คน";

                }
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดตามที่ต้องการ
            }
            finally
            {
                con.Close();
            }
        }

        private void delete_member_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int id = Convert.ToInt32(textBox15.Text);
                string sql = "DELETE FROM members WHERE id = @id";

                // Execute the SQL query with parameter from textBox15
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();  // Execute the query to delete data
                }

                MessageBox.Show("Delete Data Success");
                textBox15.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                // รีเฟรช dataGridView3 หลังจากทำการลบข้อมูล
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดตามที่ต้องการ
            }
            finally
            {
                con.Close();
            }
        }

        // ฟังก์ชั่นสำหรับรีเฟรช dataGridView3
        private void RefreshDataGridView()
        {
            string sql = "SELECT * FROM members";
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    DataSet data = new DataSet();
                    adapter.Fill(data, "members");  // Fill the DataSet

                    dataGridView3.DataSource = data.Tables["members"];  // Set the DataSource
                }
            }

            // นับจำนวนสมาชิกใหม่และแสดงใน label
            UpdateMemberCountLabel();
        }

        // ฟังก์ชั่นสำหรับอัปเดตจำนวนสมาชิกที่แสดงใน label
        private void UpdateMemberCountLabel()
        {
            string sql = "SELECT COUNT(*) FROM members";
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                label10.Text = $"{count} คน";                                // Display the count in label10
                label14.Text = $"{count} คน";                                // Display the count in label14
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string name = textBox9.Text;
                string clothesDetail = textBox8.Text;
                int amount = Convert.ToInt32(textBox12.Text);
                int price = Convert.ToInt32(textBox11.Text);
                string paymentStatus = radioButton1.Checked ? "ชำระแล้ว" : "ค้างชำระ";
                DateTime deliveryDate = dateTimePicker1.Value;
                DateTime pickUpDate = dateTimePicker2.Value;
                string tell = textBox16.Text;
                string wash = checkBox1.Checked ? "ซัก" :"";
                string irons = checkBox2.Checked ? "รีด" : "";

                string sql = "INSERT INTO laundryshopdb (namecustomors, clothes_detail, amount, price, paymentstatus, deliverydate, pickupdate, tell, wash, irons) " +
                             "VALUES (@name, @clothesDetail, @amount, @price, @paymentStatus, @deliveryDate, @pickUpDate, @tell, @wash, @irons)";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@clothesDetail", clothesDetail);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@paymentStatus", paymentStatus);
                    cmd.Parameters.AddWithValue("@deliveryDate", deliveryDate);
                    cmd.Parameters.AddWithValue("@pickUpDate", pickUpDate);
                    cmd.Parameters.AddWithValue("@tell", tell);
                    cmd.Parameters.AddWithValue("@wash", wash);
                    cmd.Parameters.AddWithValue("@irons", irons);

                    cmd.ExecuteNonQuery();  // Execute the query to insert data
                }
                 sql = "INSERT INTO history (name_cus,status) " +
                        "VALUES (@Name_cus, @Status)";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@Status", "ส่งซัก");
                   
                    cmd.ExecuteNonQuery();  // Execute the query to insert data
                }
                // First query to populate the DataGridView
                 sql = "SELECT name_cus, status FROM history";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "history");  // Fill the DataSet

                        dataGridView2.DataSource = data.Tables["history"];  // Set the DataSource
                    }
                }
                sql = "SELECT * FROM laundryshopdb";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "laundryshopdb");  // Fill the DataSet

                        dataGridView1.DataSource = data.Tables["laundryshopdb"];  // Set the DataSource
                    }
                }

                // Query to sum prices where paymentstatus is 'no'
                sql = "SELECT SUM(price) FROM laundryshopdb WHERE paymentstatus = 'ค้างชำระ'";
                int totalNo = 0;
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    totalNo = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    // แสดงผลรวมใน textbox14
                    textBox14.Text = totalNo.ToString() + " บาท";
                }
                calculatePrices();
                textBox9.Text = "";
                textBox8.Text = "";
                textBox11.Text = "";
                textBox16.Text = "";
                textBox12.Text = "";

                MessageBox.Show("Data added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // ตรวจสอบว่าคลิกบนเซลล์ของตาราง
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    textBox17.Text = selectedRow.Cells["ID"].Value.ToString();
                    textBox9.Text = selectedRow.Cells["namecustomors"].Value.ToString();
                    textBox8.Text = selectedRow.Cells["clothes_detail"].Value.ToString();
                    textBox12.Text = selectedRow.Cells["amount"].Value.ToString();
                    textBox11.Text = selectedRow.Cells["price"].Value.ToString();
                    textBox16.Text = selectedRow.Cells["tell"].Value.ToString();

                    // ตั้งค่าค่าวันที่ใน DateTimePicker 1
                    dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["deliverydate"].Value);

                    // ตั้งค่าค่าวันที่ใน DateTimePicker 2
                    dateTimePicker2.Value = Convert.ToDateTime(selectedRow.Cells["Pickupdate"].Value);

                    // ตรวจสอบค่าในเซลล์ "wash" และกำหนดค่าให้กับ CheckBox 1
                    if (selectedRow.Cells["wash"].Value.ToString() == "ซัก")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }

                    // ตรวจสอบค่าในเซลล์ "irons" และกำหนดค่าให้กับ CheckBox 2
                    if (selectedRow.Cells["irons"].Value.ToString() == "รีด")
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดตามที่ต้องการ
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
   
            try
            {
              
                con.Open();

                int id = Convert.ToInt32(textBox17.Text);
                string sql = "UPDATE laundryshopdb SET namecustomors = @Namecustomors,clothes_detail = @clothesDetail,amount = @amount " +
                    ",price = @price , paymentstatus = @paymentStatus , deliverydate =   @deliveryDate ,  pickupdate = @pickUpDate , tell = @tell " +
                    ", wash =  @wash , irons =  @irons  WHERE id = @id";

                // Execute the SQL query with parameters from textbox6, textbox7, and textBox15
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", textBox9.Text);
                    cmd.Parameters.AddWithValue("@clothesDetail", textBox8.Text);
                    cmd.Parameters.AddWithValue("@amount", Convert.ToInt32(textBox12.Text));
                    cmd.Parameters.AddWithValue("@price", Convert.ToInt32(textBox11.Text));
                    cmd.Parameters.AddWithValue("@paymentStatus", radioButton1.Checked ? "ชำระแล้ว" : "ค้างชำระ");
                    cmd.Parameters.AddWithValue("@deliveryDate", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@pickUpDate", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@tell", textBox16.Text);
                    cmd.Parameters.AddWithValue("@wash", checkBox1.Checked ? "ซัก" : "");
                    cmd.Parameters.AddWithValue("@irons",checkBox2.Checked ? "รีด" : "");
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();  // Execute the query to insert data
                }
                MessageBox.Show("Update Data Success");
                
                sql = "SELECT * FROM laundryshopdb";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "laundryshopdb");  // Fill the DataSet

                        dataGridView1.DataSource = data.Tables["laundryshopdb"];  // Set the DataSource
                    }
                }
            

                // Query to sum prices where paymentstatus is 'no'
                sql = "SELECT SUM(price) FROM laundryshopdb WHERE paymentstatus = 'ค้างชำระ'";
               int   totalNo = 0;
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    totalNo = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    // แสดงผลรวมใน textbox14
                    textBox14.Text = totalNo.ToString() + " บาท";
                }

                // Query to sum prices where paymentstatus is 'yes'
                sql = "SELECT SUM(price) FROM laundryshopdb WHERE paymentstatus = 'ชำระแล้ว'";
                int totalYes = 0;
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    var result = cmd.ExecuteScalar();
                    totalYes = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    // แสดงผลรวมใน textbox13
                    textBox13.Text = totalYes.ToString() + " บาท";
                }
                sql = "SELECT COUNT(*) FROM laundryshopdb WHERE paymentstatus = 'ชำระแล้ว'";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                                                           // Display the count in label5
                    label5.Text = $"{count} คน";
                    chart1.Series["Series1"].Points.AddXY("ชำระแล้ว", count);
                }

                // Query to count rows where paymentstatus is 'no'
                sql = "SELECT COUNT(*) FROM laundryshopdb WHERE paymentstatus = 'ค้างชำระ'";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    int count = (int)cmd.ExecuteScalar();  // Execute the query and get the count
                    chart1.Series["Series1"].Points.AddXY("ค้างชำระ", count);                                   // Display the count in label8
                    label8.Text = $"{count} คน";
                }
                calculatePrices();
                textBox9.Text = "";
                textBox8.Text = "";
                textBox11.Text = "";
                textBox16.Text = "";
                textBox12.Text = "";
                textBox17.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void getDatListsTable()
        {
            try
            {
                con.Open();

                // Prepare the SQL query
                string sql;
                if (!string.IsNullOrEmpty(textBox10.Text))
                {
                    // If textBox5 has a value, use it for searching
                    sql = "SELECT * FROM laundryshopdb WHERE namecustomors LIKE '%' + @searchValue + '%' OR clothes_detail LIKE '%' + @searchValue + '%' " +
                          "OR amount LIKE '%' + @searchValue + '%' OR price LIKE '%' + @searchValue + '%' OR paymentstatus LIKE '%' + @searchValue + '%' " +
                          "OR deliverydate LIKE '%' + @searchValue + '%' OR pickupdate LIKE '%' + @searchValue + '%' OR tell LIKE '%' + @searchValue + '%' " +
                          "OR wash LIKE '%' + @searchValue + '%' OR irons LIKE '%' + @searchValue + '%'";
                }
                else
                {
                    // If textBox5 is empty, retrieve all data from members table
                    sql = "SELECT * FROM laundryshopdb";
                }

                // Execute the SQL query
          
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    // Add parameter if textBox5 has a value
                    if (!string.IsNullOrEmpty(textBox10.Text))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", textBox10.Text);
                    }

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "laundryshopdb");  // Fill the DataSet

                        dataGridView1.DataSource = data.Tables["laundryshopdb"];  // Set the DataSource
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error or show a message to the user)
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                getDatListsTable();
                countIrons();
                countwash();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // ฟังก์ชั่นสำหรับรีเฟรช dataGridView1
        private void RefreshDataGridView2()
        {
            string sql = "SELECT * FROM laundryshopdb";
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                {
                    DataSet data = new DataSet();
                    adapter.Fill(data, "laundryshopdb");  // Fill the DataSet

                    dataGridView1.DataSource = data.Tables["laundryshopdb"];  // Set the DataSource
                }
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

                con.Open();
                RefreshDataGridView2();
                int id = Convert.ToInt32(textBox17.Text);
                string sql = "DELETE FROM laundryshopdb WHERE ID = @id";

                // Execute the SQL query with parameter from textBox15
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();  // Execute the query to delete data
                }

                sql = "INSERT INTO history (name_cus,status) " +
                      "VALUES (@Name_cus, @Status)";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@name", textBox9.Text);
                    cmd.Parameters.AddWithValue("@Status", "ส่งคืนแล้ว");

                    cmd.ExecuteNonQuery();  // Execute the query to insert data
                }
                sql = "SELECT name_cus, status FROM history";
                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        DataSet data = new DataSet();
                        adapter.Fill(data, "history");  // Fill the DataSet

                        dataGridView2.DataSource = data.Tables["history"];  // Set the DataSource
                    }
                }
                MessageBox.Show("ส่งคืนเรียบร้อยแล้ว");
                textBox9.Text = "";
                textBox8.Text = "";
                textBox11.Text = "";
                textBox16.Text = "";
                textBox12.Text = "";
                textBox17.Text = "";
                // รีเฟรช dataGridView3 หลังจากทำการลบข้อมูล
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดตามที่ต้องการ
            }
            finally
            {
                con.Close();
            }
        }

       

     
    }
}
