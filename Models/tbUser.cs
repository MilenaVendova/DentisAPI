using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbUserRow
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public int? DoctorID { get; set; }
        public int? TechnicianID { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.UserID = (int)dReader["UserID"];
            this.UserName = (string)dReader["UserName"];
            this.DoctorID = (dReader["DoctorID"] != DBNull.Value) ? (int)dReader["DoctorID"] : null;
            this.TechnicianID = (dReader["TechnicianID"] != DBNull.Value) ? (int)dReader["TechnicianID"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "UserID" => this.UserID,
                "UserName" => (this.UserName != null) ? this.UserName : DBNull.Value,
                "DoctorID" => (this.DoctorID.HasValue) ? this.DoctorID : DBNull.Value,
                "TechnicianID" => (this.TechnicianID.HasValue) ? this.TechnicianID : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbUser : List<tbUserRow>
    {
        private readonly MyConnection _Connection;
        public tbUser(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbUser_S";
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
                    tbUserRow dr = new tbUserRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbUser_I  @UserName, @DoctorID, @TechnicianID; SELECT UserID , UserName , DoctorID , TechnicianID FROM tbUser WHERE UserID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "UserName", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbUserRow> Insert(tbUserRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbUser_U @UserID, @UserName, @Original_UserName, @DoctorID, @Original_DoctorID, @TechnicianID, @Original_TechnicianID; SELECT UserID , UserName , DoctorID , TechnicianID FROM tbUser WHERE UserID = @UserID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "UserName", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_UserID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "UserID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "UserName", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "UserID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbUserRow> Update(tbUserRow drOriginal, tbUserRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbUser_D @Original_UserID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_UserID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "UserID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_UserName", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "UserName", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbUserRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbUserRow? drOriginal, tbUserRow? drCurrent)
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

