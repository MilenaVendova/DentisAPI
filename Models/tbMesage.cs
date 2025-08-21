using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbMessageRow
    {
        public int MessageID { get; set; }
        public int? RecieverID { get; set; }
        public int SenderID { get; set; }
        public int? CaseID { get; set; }
        public string? message { get; set; }
        public DateTime date { get; set; }
        public bool Readed { get; set; }
        public bool IsFile { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.MessageID = (int)dReader["MessageID"];
            this.RecieverID = (dReader["RecieverID"] != DBNull.Value) ? (int)dReader["RecieverID"] : null;
            this.SenderID = (int)dReader["SenderID"];
            this.CaseID = (dReader["CaseID"] != DBNull.Value) ? (int)dReader["CaseID"] : null;
            this.message = (string)dReader["message"];
            this.date = (DateTime)dReader["date"];
            this.Readed = (bool)dReader["Readed"];
            this.IsFile = (bool)dReader["IsFile"];
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "MessageID" => this.MessageID,
                "RecieverID" => (this.RecieverID.HasValue) ? this.RecieverID : DBNull.Value,
                "SenderID" => this.SenderID,
                "CaseID" => (this.CaseID.HasValue) ? this.CaseID : DBNull.Value,
                "message" => (this.message != null) ? this.message : DBNull.Value,
                "date" => this.date,
                "Readed" => this.Readed,
                "IsFile" => this.IsFile,
                _ => DBNull.Value,
            };
        }
    }
    public class tbMessage : List<tbMessageRow>
    {
        private readonly MyConnection _Connection;
        public tbMessage(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbMessage_S";
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
                    tbMessageRow dr = new tbMessageRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbMessage_I  @RecieverID, @SenderID, @CaseID, @message, @date, @Readed, @IsFile; SELECT MessageID , RecieverID , SenderID , CaseID , message , date , Readed , IsFile FROM tbMessage WHERE MessageID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@SenderID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SenderID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@message", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "message", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "date", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Readed", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "Readed", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@IsFile", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "IsFile", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbMessageRow> Insert(tbMessageRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbMessage_U @MessageID, @RecieverID, @Original_RecieverID, @SenderID, @Original_SenderID, @CaseID, @Original_CaseID, @message, @Original_message, @date, @Original_date, @Readed, @Original_Readed, @IsFile, @Original_IsFile; SELECT MessageID , RecieverID , SenderID , CaseID , message , date , Readed , IsFile FROM tbMessage WHERE MessageID = @MessageID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@SenderID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SenderID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@message", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "message", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "date", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Readed", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "Readed", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsFile", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "IsFile", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_MessageID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MessageID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_SenderID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SenderID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_message", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "message", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_date", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "date", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Readed", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "Readed", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_IsFile", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "IsFile", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@MessageID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "MessageID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbMessageRow> Update(tbMessageRow drOriginal, tbMessageRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbMessage_D @Original_MessageID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_MessageID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MessageID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_RecieverID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "RecieverID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_SenderID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SenderID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_message", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "message", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_date", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "date", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Readed", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "Readed", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_IsFile", SqlDbType.Bit, 0, ParameterDirection.Input, 0, 0, "IsFile", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbMessageRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbMessageRow? drOriginal, tbMessageRow? drCurrent)
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
