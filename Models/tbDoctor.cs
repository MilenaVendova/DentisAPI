using Microsoft.Data.SqlClient;
using DentisAPI.Services;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Data.Common;
namespace DentisAPI.Models
{
    public class tbDoctorRow
    {
        public int DoctorID { get; set; }
        public string? Name { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.DoctorID = (int)dReader["DoctorID"];
            this.Name = (string)dReader["Name"];
            this.NickName = (dReader["NickName"] != DBNull.Value) ? (string)dReader["NickName"] : null;
            this.Email = (string)dReader["Email"];
            this.Phone = (string)dReader["Phone"];
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "DoctorID" => this.DoctorID,
                "Name" => (this.Name != null) ? this.Name : DBNull.Value,
                "NickName" => (this.NickName != null) ? this.NickName : DBNull.Value,
                "Email" => (this.Email != null) ? this.Email : DBNull.Value,
                "Phone" => (this.Phone != null) ? this.Phone : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbDoctor : List<tbDoctorRow>
    {
        private readonly MyConnection _Connection;
        public tbDoctor(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbDoctor_S";
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
                    tbDoctorRow dr = new tbDoctorRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbDoctor_I  @Name, @NickName, @Email, @Phone; SELECT DoctorID , Name , NickName , Email , Phone FROM tbDoctor WHERE DoctorID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@NickName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "NickName", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbDoctorRow> Insert(tbDoctorRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbDoctor_U @DoctorID, @Name, @Original_Name, @NickName, @Original_NickName, @Email, @Original_Email, @Phone, @Original_Phone; SELECT DoctorID , Name , NickName , Email , Phone FROM tbDoctor WHERE DoctorID = @DoctorID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@NickName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "NickName", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Name", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_NickName", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "NickName", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_NickName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "NickName", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Email", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Phone", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbDoctorRow> Update(tbDoctorRow drOriginal, tbDoctorRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbDoctor_D @Original_DoctorID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0,"DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0,"Name", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_NickName", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0,"NickName", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_NickName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0,"NickName", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Email", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0,"Email", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Phone", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0,"Phone", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbDoctorRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbDoctorRow? drOriginal, tbDoctorRow? drCurrent)
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
