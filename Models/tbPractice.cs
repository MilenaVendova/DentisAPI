using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbPracticeRow
    {
        public int PracticeID { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Adress { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? TaxID { get; set; }
        public string? OpeningHours { get; set; }
        public string? DelivaryMethod { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.PracticeID = (int)dReader["PracticeID"];
            this.Name = (string)dReader["Name"];
            this.CompanyName = (string)dReader["CompanyName"];
            this.Adress = (string)dReader["Adress"];
            this.Phone = (string)dReader["Phone"];
            this.Email = (dReader["Email"] != DBNull.Value) ? (string)dReader["Email"] : null;
            this.TaxID = (dReader["TaxID"] != DBNull.Value) ? (string)dReader["TaxID"] : null;
            this.OpeningHours = (dReader["OpeningHours"] != DBNull.Value) ? (string)dReader["OpeningHours"] : null;
            this.DelivaryMethod = (dReader["DelivaryMethod"] != DBNull.Value) ? (string)dReader["DelivaryMethod"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "PracticeID" => this.PracticeID,
                "Name" => (this.Name != null) ? this.Name : DBNull.Value,
                "CompanyName" => (this.CompanyName != null) ? this.CompanyName : DBNull.Value,
                "Adress" => (this.Adress != null) ? this.Adress : DBNull.Value,
                "Phone" => (this.Phone != null) ? this.Phone : DBNull.Value,
                "Email" => (this.Email != null) ? this.Email : DBNull.Value,
                "TaxID" => (this.TaxID != null) ? this.TaxID : DBNull.Value,
                "OpeningHours" => (this.OpeningHours != null) ? this.OpeningHours : DBNull.Value,
                "DelivaryMethod" => (this.DelivaryMethod != null) ? this.DelivaryMethod : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbPractice : List<tbPracticeRow>
    {
        private readonly MyConnection _Connection;
        public tbPractice(MyConnection mc) : base()
        {
            _Connection = mc;
        }
        private SqlCommand? _SelectCommand;
        private SqlCommand SelectCommand
        {
            get
            {
                if (_SelectCommand is null)
                {
                    _SelectCommand = new SqlCommand();
                    _SelectCommand.Connection = _Connection.cnn;
                    _SelectCommand.CommandTimeout = ConnectionManager.CommandTimeout;
                    _SelectCommand.CommandText = "sp_tbPractice_S";
                    _SelectCommand.CommandType = CommandType.StoredProcedure;
                }
                return _SelectCommand;
            }
        }
        public async Task<int> Fill(CancellationToken ct)
        {
            ConnectionState cs = _Connection.cnn.State;
            try
            {
                int i = 0;
                if (cs != ConnectionState.Open)
                {
                    await _Connection.cnn.OpenAsync(ct);
                }
                SqlDataReader dReader = await SelectCommand.ExecuteReaderAsync(ct);
                while (await dReader.ReadAsync(ct))
                {
                    tbPracticeRow dr = new tbPracticeRow();
                    dr.SetDataFromSQL(dReader);
                    Add(dr);
                    i += 1;
                }
                await dReader.CloseAsync();
                return i;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cs == ConnectionState.Closed)
                {
                    await _Connection.cnn.CloseAsync();
                }
            }
        }
        private SqlCommand? _InsertCommand;
        private SqlCommand InsertCommand
        {
            get
            {
                if (_InsertCommand is null)
                {
                    _InsertCommand = new SqlCommand();
                    _InsertCommand.Connection = _Connection.cnn;
                    _InsertCommand.CommandTimeout = ConnectionManager.CommandTimeout;
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbPractice_I  @Name, @CompanyName, @Adress, @Phone, @Email, @TaxID, @OpeningHours, @DelivaryMethod; SELECT PracticeID , Name , CompanyName , Adress , Phone , Email , TaxID , OpeningHours , DelivaryMethod FROM tbPractice WHERE PracticeID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "CompanyName", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Adress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Adress", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@TaxID", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@OpeningHours", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@DelivaryMethod", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbPracticeRow> Insert(tbPracticeRow drCurrent, CancellationToken ct)
        {
            ConnectionState cs = _Connection.cnn.State;
            try
            {
                SetCommandParameterValue(InsertCommand, null, drCurrent);
                if (cs != ConnectionState.Open)
                {
                    await _Connection.cnn.OpenAsync(ct);
                }
                SqlDataReader dReader = await InsertCommand.ExecuteReaderAsync(ct);
                while (await dReader.ReadAsync(ct))
                {
                    drCurrent.SetDataFromSQL(dReader);
                }
                await dReader.CloseAsync();
                return drCurrent;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cs == ConnectionState.Closed)
                {
                    await _Connection.cnn.CloseAsync();
                }
            }
        }
        private SqlCommand? _UpdateCommand;
        private SqlCommand UpdateCommand
        {
            get
            {
                if (_UpdateCommand is null)
                {
                    _UpdateCommand = new SqlCommand();
                    _UpdateCommand.Connection = _Connection.cnn;
                    _UpdateCommand.CommandTimeout = ConnectionManager.CommandTimeout;
                    _UpdateCommand.CommandText = "exec sp_tbPractice_U @PracticeID, @Name, @Original_Name, @CompanyName, @Original_CompanyName, @Adress, @Original_Adress, @Phone, @Original_Phone, @Email, @Original_Email, @TaxID, @Original_TaxID, @OpeningHours, @Original_OpeningHours, @DelivaryMethod, @Original_DelivaryMethod; SELECT PracticeID , Name , CompanyName , Adress , Phone , Email , TaxID , OpeningHours , DelivaryMethod FROM tbPractice WHERE PracticeID = @PracticeID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@CompanyName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "CompanyName", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Adress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Adress", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@TaxID", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@OpeningHours", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DelivaryMethod", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_CompanyName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "CompanyName", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Adress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Adress", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Email", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_TaxID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_TaxID", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_OpeningHours", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_OpeningHours", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_DelivaryMethod", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DelivaryMethod", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PracticeID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbPracticeRow> Update(tbPracticeRow drOriginal, tbPracticeRow drCurrent, CancellationToken ct)
        {
            ConnectionState cs = _Connection.cnn.State;
            try
            {
                SetCommandParameterValue(UpdateCommand, drOriginal, drCurrent);
                if (cs != ConnectionState.Open)
                {
                    await _Connection.cnn.OpenAsync(ct);
                }
                SqlDataReader dReader = await UpdateCommand.ExecuteReaderAsync(ct);
                while (await dReader.ReadAsync(ct))
                {
                    drCurrent.SetDataFromSQL(dReader);
                }
                await dReader.CloseAsync();
                return drCurrent;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cs == ConnectionState.Closed)
                {
                    await _Connection.cnn.CloseAsync();
                }
            }
        }
        private SqlCommand? _DeleteCommand;
        private SqlCommand DeleteCommand
        {
            get
            {
                if (_DeleteCommand is null)
                {
                    _DeleteCommand = new SqlCommand();
                    _DeleteCommand.Connection = _Connection.cnn;
                    _DeleteCommand.CommandTimeout = ConnectionManager.CommandTimeout;
                    _DeleteCommand.CommandText = "exec sp_tbPractice_D @Original_PracticeID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_CompanyName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "CompanyName", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Adress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Adress", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Email", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_TaxID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_TaxID", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "TaxID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_OpeningHours", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_OpeningHours", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "OpeningHours", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_DelivaryMethod", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DelivaryMethod", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DelivaryMethod", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbPracticeRow drOriginal, CancellationToken ct)
        {
            ConnectionState cs = _Connection.cnn.State;
            try
            {
                SetCommandParameterValue(DeleteCommand, drOriginal, null);
                if (cs != ConnectionState.Open)
                {
                    await _Connection.cnn.OpenAsync(ct);
                }
                int i = await DeleteCommand.ExecuteNonQueryAsync(ct);
                return i;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cs == ConnectionState.Closed)
                {
                    await _Connection.cnn.CloseAsync();
                }
            }
        }
        private static void SetCommandParameterValue(SqlCommand cmd, tbPracticeRow? drOriginal, tbPracticeRow? drCurrent)
        {
            foreach (SqlParameter p in cmd.Parameters)
            {
                if (!p.SourceColumnNullMapping)
                {
                    if (p.SourceVersion == DataRowVersion.Original)
                    {
                        p.Value = drOriginal!.GetData(p.SourceColumn);
                    }
                    else
                    {
                        p.Value = drCurrent!.GetData(p.SourceColumn);
                    }
                }
                else
                {
                    p.Value = DBNull.Value;
                }
            }
        }
    }
}

