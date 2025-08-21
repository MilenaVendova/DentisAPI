using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbPatientRow
    {
        public int PatientID { get; set; }
        public string? Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? HealthInsuranceNumber { get; set; }
        public int? ShadeID { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.PatientID = (int)dReader["PatientID"];
            this.Name = (string)dReader["Name"];
            this.BirthDate = (dReader["BirthDate"] != DBNull.Value) ? (DateTime)dReader["BirthDate"] : null;
            this.HealthInsuranceNumber = (dReader["HealthInsuranceNumber"] != DBNull.Value) ? (string)dReader["HealthInsuranceNumber"] : null;
            this.ShadeID = (dReader["ShadeID"] != DBNull.Value) ? (int)dReader["ShadeID"] : null;
            this.Phone = (dReader["Phone"] != DBNull.Value) ? (string)dReader["Phone"] : null;
            this.Email = (dReader["Email"] != DBNull.Value) ? (string)dReader["Email"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "PatientID" => this.PatientID,
                "Name" => (this.Name != null) ? this.Name : DBNull.Value,
                "BirthDate" => (this.BirthDate.HasValue) ? this.BirthDate : DBNull.Value,
                "HealthInsuranceNumber" => (this.HealthInsuranceNumber != null) ? this.HealthInsuranceNumber : DBNull.Value,
                "ShadeID" => (this.ShadeID.HasValue) ? this.ShadeID : DBNull.Value,
                "Phone" => (this.Phone != null) ? this.Phone : DBNull.Value,
                "Email" => (this.Email != null) ? this.Email : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbPatient : List<tbPatientRow>
    {
        private readonly MyConnection _Connection;
        public tbPatient(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbPatient_S";
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
                    tbPatientRow dr = new tbPatientRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbPatient_I  @Name, @BirthDate, @HealthInsuranceNumber, @ShadeID, @Phone, @Email; SELECT PatientID , Name , BirthDate , HealthInsuranceNumber , ShadeID , Phone , Email FROM tbPatient WHERE PatientID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@BirthDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@HealthInsuranceNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbPatientRow> Insert(tbPatientRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbPatient_U @PatientID, @Name, @Original_Name, @BirthDate, @Original_BirthDate, @HealthInsuranceNumber, @Original_HealthInsuranceNumber, @ShadeID, @Original_ShadeID, @Phone, @Original_Phone, @Email, @Original_Email; SELECT PatientID , Name , BirthDate , HealthInsuranceNumber , ShadeID , Phone , Email FROM tbPatient WHERE PatientID = @PatientID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@BirthDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@HealthInsuranceNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_BirthDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_BirthDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_HealthInsuranceNumber", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_HealthInsuranceNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Phone", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbPatientRow> Update(tbPatientRow drOriginal, tbPatientRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbPatient_D @Original_PatientID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_BirthDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_BirthDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "BirthDate", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_HealthInsuranceNumber", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_HealthInsuranceNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "HealthInsuranceNumber", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Phone", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbPatientRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbPatientRow? drOriginal, tbPatientRow? drCurrent)
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
