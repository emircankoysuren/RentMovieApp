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

namespace RentMovieApp
{
    public partial class Form1 : Form
    {
        private string connectionF = "Server=EMIR\\MYPROJECTSQL;Database=FilmKiralamaDB;Integrated Security=True;";
        SqlConnection connection;
        

        public Form1()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenConnection();
        }
        #region FUNCTION TOOPEN
        private void OpenConnection()
        {
            try
            {

                connection = new SqlConnection(connectionF);

                // bağlantıyı açıyoruz
                connection.Open();

                // bağlantı başarılıysa bir mesaj kutusu gösteriyoruz
                MessageBox.Show("Database connection is opened succesfully!"); // 

            }
            catch (Exception error)
            {
                // bağlantı sırasında bir hata oluşursa hata mesajını gösteriyoruz
                MessageBox.Show("Database connection failure: " + error.Message); // 
            }
        }
        #endregion
        
        #region FUNCTION TOCLOSE
        private void CloseConnection()
        {
            // bağlantı nesnesi var ve açık ise
            if (connection != null && connection.State == ConnectionState.Open)
            {
                // Bağlantıyı kapat
                connection.Close();

                // bağlantı kapatılılınca bir mesaj kutusu gösteriyoruz
                MessageBox.Show("Database connection is closed succesfully!");
            }
        }
        #endregion


        #region LOADEVENT
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMembers();
        }
        #endregion


        #region LOAD FUNCTION
        private void LoadMembers()
        {
            try
            {
                OpenConnection(); // bağlantıyı açıyoruz

                // sql'den members tablosunun verilerinin hepsini çekiyoruz
                string query = "SELECT * FROM [dbo].[MEMBERS]";

                // sql Komutu oluştur
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // data Adapter oluşturuyoruz
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // veriyi tutacak bir DataTable oluşturuyoruz
                        DataTable dataTable = new DataTable();

                        // data adapter ile DataTable'ı doldur
                        adapter.Fill(dataTable);

                        // dgv'in veri kaynağını ayarlayarak veriyi göster
                        Members.DataSource = dataTable;
                       
                        
                        if (Members.Columns.Contains("MembershipDate")) 
                        {
                            // sadece Tarih göstermek için:
                            Members.Columns["MembershipDate"].DefaultCellStyle.Format = "dd.MM.yyyy";
                           
                        }
                      
                        if (Members.Columns.Contains("MembershipDate"))
                        {
                           

                           
                           Members.Columns["MembershipDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                           
                        }



                    }
                }



            }
            catch (Exception error)
            {
                MessageBox.Show("An error occurred while loading members: " + error.Message);
            }
            
        }
        #endregion


        #region ADD MEMBER CLICKEVENT
        private void button1_Click_1(object sender, EventArgs e)
        {
            
            string ad = textBox1.Text;      
            string soyad = textBox2.Text; 
            string telefon = textBox3.Text; 
            string adres = textBox4.Text; 

          
            AddMember(ad, soyad, telefon, adres);

           
            LoadMembers();

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        #endregion


        #region ADD MEMBER FUNCTION
        private void AddMember(string ad, string soyad, string telefon, string adres)
        {




            
            if (string.IsNullOrWhiteSpace(ad) || string.IsNullOrWhiteSpace(soyad))
            {
                MessageBox.Show("Member name and surname cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open(); // Bağlantıyı aç

                    
                    string query = "INSERT INTO [dbo].[MEMBERS] (Name, Surname, Contact, Adress) VALUES (@Name, @Surname, @Contact, @Adress)";
                   

                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                     
                        command.Parameters.AddWithValue("@Name", ad);
                        command.Parameters.AddWithValue("@Surname", soyad);
                        command.Parameters.AddWithValue("@Contact", (object)telefon ?? DBNull.Value); 
                        command.Parameters.AddWithValue("@Adress", (object)adres ?? DBNull.Value); 
                       
                        

                       
                        command.ExecuteNonQuery();

                        MessageBox.Show("New member is added succesfully", "SUCCESFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }

                }
                catch (Exception error)
                {
                    MessageBox.Show("Failure on adding members: " + error.Message, "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
        }
        #endregion


        #region DELETE FUNCTION
        private void DeleteMember(int memberId)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open(); // Bağlantıyı aç

                   
                    string query = "DELETE FROM [dbo].[MEMBERS] WHERE MemberID = @MemberID"; 

                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@MemberID", memberId);

                        
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Member is deleted succesfully!", "SUCCESFUL", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            
                            MessageBox.Show("There is no member to delete.", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    } 

                }
                catch (Exception error)
                {
                    MessageBox.Show("An error occured while deleting member: " + error.Message, "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }
        #endregion






        #region designerevents
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void Members_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        #region ADD CLICK EVENT
        private void addbtn_Click(object sender, EventArgs e)
        {
           
            string ad = textBox1.Text;      
            string soyad = textBox2.Text; 
            string telefon = textBox3.Text; 
            string adres = textBox4.Text; 

           
            AddMember(ad, soyad, telefon, adres);

           
            LoadMembers();

           
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
        #endregion 


        #region Delete Click Event
        private void deletebtn_Click(object sender, EventArgs e)
        {
            
            if (Members.SelectedRows.Count > 0)
            {
                
                DialogResult confirmResult = MessageBox.Show("Are you sure about deleting member who selected?", "APPROVE TO DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        
                        DataGridViewRow selectedRow = Members.SelectedRows[0];

                       
                        DataRowView drv = selectedRow.DataBoundItem as DataRowView;

                        if (drv != null)
                        {
                           
                            int memberId = Convert.ToInt32(drv["MemberID"]);

                           
                            DeleteMember(memberId);

                            LoadMembers();
                        }
                        else
                        {
                          
                            MessageBox.Show("Cannot access to selected data source. ", "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception error)
                    {
                       
                        MessageBox.Show("Failure on deleting member who selected. " + error.Message, "FAILURE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
              
            }
            else
            {
               
                MessageBox.Show("Please select a member to delete. ", "INFORMATION", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        #endregion

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
        
        
        
        // TABPAGES 2 CONTROLS



        #region LOADMOVIESFUNCTION
        private void LoadFilms()
        {
            
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open(); 

                   
                    string query = "SELECT * FROM [dbo].[MOVIE]"; 
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); 
                       
                        MoviesDVG.DataSource = dataTable;


                        // deisgn blockları
                        MoviesDVG.Columns["MovieID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
                        MoviesDVG.Columns["MovieName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        MoviesDVG.Columns["Director"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        MoviesDVG.Columns["PublicationDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        MoviesDVG.Columns["Stock"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        MoviesDVG.Columns["Rent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        MoviesDVG.RowTemplate.Height = 22;
                    
                        
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("An error occurred while loading movies: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }
        #endregion



        #region MOVIETABPAGE

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
        
       
           
        
        #endregion


        #region MOVIEADDEVENT
        private void button1_Click_2(object sender, EventArgs e)
        {
           
            string baslik = textBox5.Text;     
            string yonetmen = textBox6.Text; 

          
            
                int yil = 0; 
                if (!int.TryParse(textBox7.Text, out yil))
                {
                    MessageBox.Show("Please enter valid number for year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

            int stokAdedi = 0; 
            if (!int.TryParse(textBox8.Text, out stokAdedi))
            {
                MessageBox.Show("Please enter valid number for stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            int kiradaAdet = 0; 
            if (!string.IsNullOrWhiteSpace(textBox9.Text) && !int.TryParse(textBox9.Text, out kiradaAdet))
            {
                
                MessageBox.Show("Please enter valid number for rent.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            


            AddFilm(baslik, yonetmen, yil, stokAdedi, kiradaAdet);

            
            LoadFilms();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

        }
        #endregion
        
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        #region MOVIEADD
       


        private void AddFilm(string baslik, string yonetmen, int yil, int stokAdedi, int kiradaAdet)
        {
          
            if (string.IsNullOrWhiteSpace(baslik))
            {
                MessageBox.Show("Title cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

          
            if (yil <= 0 || stokAdedi < 0 || kiradaAdet < 0)
            {
                MessageBox.Show("Invalid year, stock or rent amount .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            if (kiradaAdet > stokAdedi)
            {
                MessageBox.Show("Movie amount of rented cannot be higher than stock amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }


          
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open(); 

                    
                    string query = "INSERT INTO [dbo].[MOVIE] (MovieName, Director, PublicationDate, Stock, Rent) VALUES (@MovieName, @Director, @PublicationDate, @Stock, @Rent)";

                  
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        
                        command.Parameters.AddWithValue("@MovieName", baslik);
                        command.Parameters.AddWithValue("@Director", (object)yonetmen ?? DBNull.Value); // Yönetmen boşsa NULL olabilir
                        command.Parameters.AddWithValue("@PublicationDate", yil);
                        command.Parameters.AddWithValue("@Stock ", stokAdedi);
                        command.Parameters.AddWithValue("@Rent", kiradaAdet);

                        
                        command.ExecuteNonQuery(); 

                        MessageBox.Show("New movie is added succesfully!", "Succesfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    } 
                }
                catch (Exception error)
                {
                    MessageBox.Show("Failure on adding movie!: " + error.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } 
        }
        #endregion


        #region DELETE FUNCTION
        private void DeleteFilm(int filmId)
        {
            
            DialogResult confirmResult = MessageBox.Show("Are you sure about deleting movie that selected? ", "Approve to delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                
                using (SqlConnection connection = new SqlConnection(connectionF))
                {
                    try
                    {
                        connection.Open(); 

                       
                        string query = "DELETE FROM [dbo].[MOVIE] WHERE MovieID = @MovieID";

                       
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                           
                            command.Parameters.AddWithValue("@MovieID", filmId);

                          
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Movie is deleted succesfully!", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                               
                                MessageBox.Show("There is no movie to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        } 

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Failure on deleting movie: " + error.Message, "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            }
            
        }




        #endregion


        #region DELETE CLICK EVENT
        private void button2_Click_1(object sender, EventArgs e)
        {

            
            if (MoviesDVG.SelectedRows.Count > 0)
            {
                
                DialogResult confirmResult = MessageBox.Show("Are you sure about deleting movie that selected?", "Aprrove to delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        
                        DataGridViewRow selectedRow = MoviesDVG.SelectedRows[0];

                        
                        DataRowView drv = selectedRow.DataBoundItem as DataRowView;

                        if (drv != null)
                        {
                            
                            int filmId = Convert.ToInt32(drv["MovieID"]); 

                            
                            DeleteFilm(filmId);

                            LoadFilms();
                        }
                        else
                        {
                            MessageBox.Show("Cannot access on selected source of row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failure on deleting selected movie!: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select movie to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion



        // TABPAGES 3 CONTROLS ( RENT TABPAGE)
        private void MoviesDVG_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region COMBOBOX MEMBERS
        private void PopulateMemberComboBox()
        {
           
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open(); 

                 
                    string query = "SELECT MemberID, Name + ' ' + Surname AS FullName FROM [dbo].[MEMBERS]"; // Tablo adının doğru olduğundan emin olun

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); 

                        
                        comboBox1.DataSource = dataTable; 
                        comboBox1.DisplayMember = "FullName"; 
                        comboBox1.ValueMember = "MemberID";
                        comboBox1.SelectedIndex = -1;



                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("An error occurred while loading the members into the ComboBox: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion


        #region COMBOBOX MOVIES
        private void PopulateFilmComboBox()
        {
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open();
                    
                    string query = "SELECT MovieID, MovieName FROM [dbo].[MOVIE] WHERE Stock > Rent"; //
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        
                        comboBox2.DataSource = dataTable;
                       
                        comboBox2.DisplayMember = "MovieName"; 
                        comboBox2.ValueMember = "MovieID";
                        comboBox2.SelectedIndex = -1; 
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("An error occurred while loading the members into the ComboBox: " + error.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }







        #endregion

        #region TAB PAGES CONTROL
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
           

           
            if (tabControl.SelectedTab == tabPage2) 
            {
                LoadFilms();
            }
            else if (tabControl.SelectedTab == tabPage3) 
            {
                
                PopulateMemberComboBox();
                PopulateFilmComboBox();
                LoadRentals();
            }
            
        }
        #endregion


        #region LOADRENTPAGE
        private void LoadRentals()
        {
            

            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                try
                {
                    connection.Open();
                   
                    string query = @"
                SELECT
                    R.RentID AS RentalID,         
                    M.Name + ' ' + M.Surname AS MemberFullName,
                    F.MovieName AS MovieTitle,    
                    R.RentDate AS RentalDate,
                    R.DueDate AS DueDate,          
                    R.RentPrice AS RentalFee,      
                    R.MemberID,                    
                    R.MovieID                     
                FROM [dbo].[RENT] AS R
                INNER JOIN [dbo].[MEMBERS] AS M ON R.MemberID = M.MemberID
                INNER JOIN [dbo].[MOVIE] AS F ON R.MovieID = F.MovieID
                ORDER BY R.RentDate DESC";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dvgRent.DataSource = dataTable;
                        if (dvgRent.Columns.Contains("MemberFullName"))
                        {
                           
                            dvgRent.Columns["MemberFullName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
                        }
                        if (dvgRent.Columns.Contains("RentalDate"))
                        {

                            dvgRent.Columns["RentalDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }
                        if (dvgRent.Columns.Contains("DueDate"))
                        {
                           
                            dvgRent.Columns["DueDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
                        }


                        if (dvgRent.Columns.Contains("RentalID")) dvgRent.Columns["RentalID"].HeaderText = "Rental ID";
                        if (dvgRent.Columns.Contains("MemberFullName")) dvgRent.Columns["MemberFullName"].HeaderText = "Member Full Name";
                        if (dvgRent.Columns.Contains("MovieTitle")) dvgRent.Columns["MovieTitle"].HeaderText = "Movie Title";
                        if (dvgRent.Columns.Contains("RentalDate")) dvgRent.Columns["RentalDate"].HeaderText = "Rental Date";
                        if (dvgRent.Columns.Contains("DueDate")) dvgRent.Columns["DueDate"].HeaderText = "Due Date";
                        if (dvgRent.Columns.Contains("RentalFee")) dvgRent.Columns["RentalFee"].HeaderText = "Rental Fee";

                       
                        if (dvgRent.Columns.Contains("MemberID")) dvgRent.Columns["MemberID"].Visible = false;
                        if (dvgRent.Columns.Contains("MovieID")) dvgRent.Columns["MovieID"].Visible = false;
                        
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show("An error occurred while loading rental records: " + error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        #endregion


        #region RENTBUTTON
        private void button3_Click(object sender, EventArgs e)
        {
           
            if (comboBox1.SelectedValue == null || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a member.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboBox2.SelectedValue == null || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a movie.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

          
            int memberId = (int)comboBox1.SelectedValue;
            int movieId = (int)comboBox2.SelectedValue;
            DateTime rentDate = DateTime.Now;          
            DateTime dueDate = rentDate.AddDays(7);    
            decimal rentPrice = 15.00m;                 
            
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                SqlTransaction transaction = null; 
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(); 

                    
                    string checkQuery = "SELECT Stock, Rent FROM MOVIE WHERE MovieID = @MovieID";
                    int currentStock = 0;
                    int currentRent = 0;
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection, transaction)) 
                    {
                        checkCmd.Parameters.AddWithValue("@MovieID", movieId);
                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                currentStock = reader.GetInt32(0); 
                                currentRent = reader.GetInt32(1);  
                            }
                            else
                            {
                                throw new Exception("Selected movie cannot be found at database."); 
                            }
                        } 
                    } 

                    
                    if (currentStock <= currentRent)
                    {
                        MessageBox.Show("The selected movie is currently out of stock or all copies are rented out.", "Out of stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        transaction.Rollback(); 
                        return; 
                    }

                   
                    string insertQuery = "INSERT INTO RENT (MovieID, MemberID, RentDate, DueDate, RentPrice) VALUES (@MovieID, @MemberID, @RentDate, @DueDate, @RentPrice)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection, transaction)) // Transaction'a dahil et
                    {
                        insertCmd.Parameters.AddWithValue("@MovieID", movieId);
                        insertCmd.Parameters.AddWithValue("@MemberID", memberId);
                        insertCmd.Parameters.AddWithValue("@RentDate", rentDate);
                        insertCmd.Parameters.AddWithValue("@DueDate", dueDate);
                        insertCmd.Parameters.AddWithValue("@RentPrice", rentPrice);
                        insertCmd.ExecuteNonQuery(); 
                    }

                   
                    string updateQuery = "UPDATE MOVIE SET Rent = Rent + 1 WHERE MovieID = @MovieID";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction)) 
                    {
                        updateCmd.Parameters.AddWithValue("@MovieID", movieId);
                        updateCmd.ExecuteNonQuery(); 
                    }

                    
                    transaction.Commit();

                    MessageBox.Show("Movie is rented succesfully.", "Succesful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadRentals();          
                    PopulateFilmComboBox(); 
                    comboBox1.SelectedIndex = -1; 
                    comboBox2.SelectedIndex = -1;

                }
                catch (Exception ex)
                {
                    
                    try
                    {
                        if (transaction != null) 
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception rbEx)
                    {
                        
                        MessageBox.Show("an error occurred while Rollback: " + rbEx.Message);
                    }
                    MessageBox.Show("an error occurred while renting: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             
            }
        }





        #endregion



        #region Refund button event
        private void button4_Click(object sender, EventArgs e)
        {
            
            if (dvgRent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a movie that will be refunded from table.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
            int rentalIdValue = 0;
            int movieIdValue = 0;  
            try
            {
                DataGridViewRow selectedRow = dvgRent.SelectedRows[0]; 

                
                rentalIdValue = Convert.ToInt32(selectedRow.Cells["RentalID"].Value); 
                movieIdValue = Convert.ToInt32(selectedRow.Cells["MovieID"].Value);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Could not read required IDs from the selected row.\nPlease ensure 'RentalID' and 'MovieID' columns exist in the data source and are correctly named.\nError: " + ex.Message, "Data Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (rentalIdValue <= 0 || movieIdValue <= 0)
            {
                MessageBox.Show("Invalid rental or movie ID obtained from the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            using (SqlConnection connection = new SqlConnection(connectionF))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                   
                    string deleteQuery = "DELETE FROM RENT WHERE RentID = @OriginalRentID"; 
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection, transaction))
                    {
                        deleteCmd.Parameters.AddWithValue("@OriginalRentID", rentalIdValue); 
                        int rowsAffected = deleteCmd.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            throw new Exception($"Rental record with ID={rentalIdValue} could not be found or deleted from RENT table.");
                        }
                    }

                    
                    string updateQuery = "UPDATE MOVIE SET Rent = Rent - 1 WHERE MovieID = @MovieID AND Rent > 0";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction))
                    {
                        updateCmd.Parameters.AddWithValue("@MovieID", movieIdValue); 
                        updateCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Movie is refunded succesfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadRentals();
                    PopulateFilmComboBox();
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
                    }
                    catch (Exception rbEx)
                    {
                        MessageBox.Show("An error occurred during Rollback: " + rbEx.Message);
                    }
                    MessageBox.Show("An error occurred while refunding: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        #endregion   
    
    
    }

}










