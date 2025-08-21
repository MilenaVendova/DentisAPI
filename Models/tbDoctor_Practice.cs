using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbDoctor_PracticeRow
    {
        public int PracticeDoctorID { get; set; }
        public int PracitceID { get; set; }
        public int DoctorID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.PracticeDoctorID = (int)dReader["PracticeDoctorID"];
            this.PracitceID = (int)dReader["PracitceID"];
            this.DoctorID = (int)dReader["DoctorID"];
            this.StartDate = (DateTime)dReader["StartDate"];
            this.EndDate = (dReader["EndDate"] != DBNull.Value) ? (DateTime)dReader["EndDate"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "PracticeDoctorID" => this.PracticeDoctorID,
                "PracitceID" => this.PracitceID,
                "DoctorID" => this.DoctorID,
                "StartDate" => this.StartDate,
                "EndDate" => (this.EndDate.HasValue) ? this.EndDate : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbDoctor_Practice : List<tbDoctor_PracticeRow>
    {
        private readonly MyConnection _Connection;
        public tbDoctor_Practice(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbDoctor_Practice_S";
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
                    tbDoctor_PracticeRow dr = new tbDoctor_PracticeRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbDoctor_Practice_I  @PracitceID, @DoctorID, @StartDate, @EndDate; SELECT PracticeDoctorID , PracitceID , DoctorID , StartDate , EndDate FROM tbDoctor_Practice WHERE PracticeDoctorID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@PracitceID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracitceID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbDoctor_PracticeRow> Insert(tbDoctor_PracticeRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbDoctor_Practice_U @PracticeDoctorID, @PracitceID, @Original_PracitceID, @DoctorID, @Original_DoctorID, @StartDate, @Original_StartDate, @EndDate, @Original_EndDate; SELECT PracticeDoctorID , PracitceID , DoctorID , StartDate , EndDate FROM tbDoctor_Practice WHERE PracticeDoctorID = @PracticeDoctorID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PracitceID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracitceID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PracticeDoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeDoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PracitceID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracitceID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_StartDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_EndDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_EndDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PracticeDoctorID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "PracticeDoctorID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbDoctor_PracticeRow> Update(tbDoctor_PracticeRow drOriginal, tbDoctor_PracticeRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbDoctor_Practice_D @Original_PracticeDoctorID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PracticeDoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeDoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PracitceID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracitceID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_StartDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "StartDate", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_EndDate", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_EndDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "EndDate", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbDoctor_PracticeRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbDoctor_PracticeRow? drOriginal, tbDoctor_PracticeRow? drCurrent)
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
