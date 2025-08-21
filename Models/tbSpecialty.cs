using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbSpecialtyRow
    {
        public int SpecialtyID { get; set; }
        public string? Specialty { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.SpecialtyID = (int)dReader["SpecialtyID"];
            this.Specialty = (string)dReader["Specialty"];
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "SpecialtyID" => this.SpecialtyID,
                "Specialty" => (this.Specialty != null) ? this.Specialty : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbSpecialty : List<tbSpecialtyRow>
    {
        private readonly MyConnection _Connection;
        public tbSpecialty(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbSpecialty_S";
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
                    tbSpecialtyRow dr = new tbSpecialtyRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbSpecialty_I  @Specialty; SELECT SpecialtyID , Specialty FROM tbSpecialty WHERE SpecialtyID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@Specialty", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Specialty", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbSpecialtyRow> Insert(tbSpecialtyRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbSpecialty_U @SpecialtyID, @Specialty, @Original_Specialty; SELECT SpecialtyID , Specialty FROM tbSpecialty WHERE SpecialtyID = @SpecialtyID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Specialty", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Specialty", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_SpecialtyID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SpecialtyID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Specialty", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Specialty", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@SpecialtyID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "SpecialtyID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbSpecialtyRow> Update(tbSpecialtyRow drOriginal, tbSpecialtyRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbSpecialty_D @Original_SpecialtyID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_SpecialtyID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "SpecialtyID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Specialty", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Specialty", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbSpecialtyRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbSpecialtyRow? drOriginal, tbSpecialtyRow? drCurrent)
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

