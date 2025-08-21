using DentisAPI.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
namespace DentisAPI.Models
{
    public class tbCaseRow
    {
        public int CaseID { get; set; }
        public int? StatusID { get; set; }
        public int? PriorityLevelID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int PracticeID { get; set; }
        public int? TechnicianID { get; set; }
        public int ItemTypeID { get; set; }
        public int? MaterialID { get; set; }
        public int? ShadeID { get; set; }
        public int? PaymentStatusID { get; set; }
        public DateTime CaseDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string? ToothNumber { get; set; }
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public string? DeliveryAddress { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.CaseID = (int)dReader["CaseID"];
            this.StatusID = (dReader["StatusID"] != DBNull.Value) ? (int)dReader["StatusID"] : null;
            this.PriorityLevelID = (dReader["PriorityLevelID"] != DBNull.Value) ? (int)dReader["PriorityLevelID"] : null;
            this.PatientID = (int)dReader["PatientID"];
            this.DoctorID = (int)dReader["DoctorID"];
            this.PracticeID = (int)dReader["PracticeID"];
            this.TechnicianID = (dReader["TechnicianID"] != DBNull.Value) ? (int)dReader["TechnicianID"] : null;
            this.ItemTypeID = (int)dReader["ItemTypeID"];
            this.MaterialID = (dReader["MaterialID"] != DBNull.Value) ? (int)dReader["MaterialID"] : null;
            this.ShadeID = (dReader["ShadeID"] != DBNull.Value) ? (int)dReader["ShadeID"] : null;
            this.PaymentStatusID = (dReader["PaymentStatusID"] != DBNull.Value) ? (int)dReader["PaymentStatusID"] : null;
            this.CaseDate = (DateTime)dReader["CaseDate"];
            this.Deadline = (dReader["Deadline"] != DBNull.Value) ? (DateTime)dReader["Deadline"] : null;
            this.ToothNumber = (string)dReader["ToothNumber"];
            this.Notes = (dReader["Notes"] != DBNull.Value) ? (string)dReader["Notes"] : null;
            this.Price = (dReader["Price"] != DBNull.Value) ? (decimal)dReader["Price"] : null;
            this.DeliveryAddress = (dReader["DeliveryAddress"] != DBNull.Value) ? (string)dReader["DeliveryAddress"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "CaseID" => this.CaseID,
                "StatusID" => (this.StatusID.HasValue) ? this.StatusID : DBNull.Value,
                "PriorityLevelID" => (this.PriorityLevelID.HasValue) ? this.PriorityLevelID : DBNull.Value,
                "PatientID" => this.PatientID,
                "DoctorID" => this.DoctorID,
                "PracticeID" => this.PracticeID,
                "TechnicianID" => (this.TechnicianID.HasValue) ? this.TechnicianID : DBNull.Value,
                "ItemTypeID" => this.ItemTypeID,
                "MaterialID" => (this.MaterialID.HasValue) ? this.MaterialID : DBNull.Value,
                "ShadeID" => (this.ShadeID.HasValue) ? this.ShadeID : DBNull.Value,
                "PaymentStatusID" => (this.PaymentStatusID.HasValue) ? this.PaymentStatusID : DBNull.Value,
                "CaseDate" => this.CaseDate,
                "Deadline" => (this.Deadline.HasValue) ? this.Deadline : DBNull.Value,
                "ToothNumber" => (this.ToothNumber != null) ? this.ToothNumber : DBNull.Value,
                "Notes" => (this.Notes != null) ? this.Notes : DBNull.Value,
                "Price" => (this.Price.HasValue) ? this.Price : DBNull.Value,
                "DeliveryAddress" => (this.DeliveryAddress != null) ? this.DeliveryAddress : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class tbCase : List<tbCaseRow>
    {
        private readonly MyConnection _Connection;
        public tbCase(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "sp_tbCase_S";
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
                    tbCaseRow dr = new tbCaseRow();
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
                    _InsertCommand.CommandText = "DECLARE @RETURN_VALUE INT; exec @RETURN_VALUE = sp_tbCase_I  @StatusID, @PriorityLevelID, @PatientID, @DoctorID, @PracticeID, @TechnicianID, @ItemTypeID, @MaterialID, @ShadeID, @PaymentStatusID, @CaseDate, @Deadline, @ToothNumber, @Notes, @Price, @DeliveryAddress; SELECT CaseID , StatusID , PriorityLevelID , PatientID , DoctorID , PracticeID , TechnicianID , ItemTypeID , MaterialID , ShadeID , PaymentStatusID , CaseDate , Deadline , ToothNumber , Notes , Price , DeliveryAddress FROM tbCase WHERE CaseID = @RETURN_VALUE";
                    _InsertCommand.CommandType = CommandType.Text;
                    _InsertCommand.Parameters.Add(new SqlParameter("@StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@ItemTypeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ItemTypeID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@CaseDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "CaseDate", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Deadline", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@ToothNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "ToothNumber", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@Price", SqlDbType.Money, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Current, false, null, "", "", ""));
                    _InsertCommand.Parameters.Add(new SqlParameter("@DeliveryAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _InsertCommand;
            }
        }
        public async Task<tbCaseRow> Insert(tbCaseRow drCurrent, CancellationToken ct)
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
                    _UpdateCommand.CommandText = "exec sp_tbCase_U @CaseID, @StatusID, @Original_StatusID, @PriorityLevelID, @Original_PriorityLevelID, @PatientID, @Original_PatientID, @DoctorID, @Original_DoctorID, @PracticeID, @Original_PracticeID, @TechnicianID, @Original_TechnicianID, @ItemTypeID, @Original_ItemTypeID, @MaterialID, @Original_MaterialID, @ShadeID, @Original_ShadeID, @PaymentStatusID, @Original_PaymentStatusID, @CaseDate, @Original_CaseDate, @Deadline, @Original_Deadline, @ToothNumber, @Original_ToothNumber, @Notes, @Original_Notes, @Price, @Original_Price, @DeliveryAddress, @Original_DeliveryAddress; SELECT CaseID , StatusID , PriorityLevelID , PatientID , DoctorID , PracticeID , TechnicianID , ItemTypeID , MaterialID , ShadeID , PaymentStatusID , CaseDate , Deadline , ToothNumber , Notes , Price , DeliveryAddress FROM tbCase WHERE CaseID = @CaseID";
                    _UpdateCommand.CommandType = CommandType.Text;
                    _UpdateCommand.Parameters.Add(new SqlParameter("@StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@ItemTypeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ItemTypeID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@CaseDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "CaseDate", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Deadline", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@ToothNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "ToothNumber", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Notes", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Price", SqlDbType.Money, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@DeliveryAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Current, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_ItemTypeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ItemTypeID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_CaseDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "CaseDate", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Deadline", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Deadline", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_ToothNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "ToothNumber", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Notes", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Notes", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_Price", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_Price", SqlDbType.Money, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@IsNull_DeliveryAddress", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Original, true, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@Original_DeliveryAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Original, false, null, "", "", ""));
                    _UpdateCommand.Parameters.Add(new SqlParameter("@CaseID", SqlDbType.Int, 4, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Current, false, null, "", "", ""));
                }
                return _UpdateCommand;
            }
        }
        public async Task<tbCaseRow> Update(tbCaseRow drOriginal, tbCaseRow drCurrent, CancellationToken ct)
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
                    _DeleteCommand.CommandText = "exec sp_tbCase_D @Original_CaseID";
                    _DeleteCommand.CommandType = CommandType.Text;
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_CaseID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "CaseID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_StatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "StatusID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PriorityLevelID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PriorityLevelID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PatientID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PatientID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DoctorID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DoctorID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PracticeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PracticeID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_TechnicianID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "TechnicianID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_ItemTypeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ItemTypeID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_MaterialID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "MaterialID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_ShadeID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "ShadeID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_PaymentStatusID", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "PaymentStatusID", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_CaseDate", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "CaseDate", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Deadline", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Deadline", SqlDbType.Date, 0, ParameterDirection.Input, 0, 0, "Deadline", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_ToothNumber", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "ToothNumber", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Notes", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Notes", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Notes", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_Price", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_Price", SqlDbType.Money, 0, ParameterDirection.Input, 0, 0, "Price", DataRowVersion.Original, false, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@IsNull_DeliveryAddress", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Original, true, null, "", "", ""));
                    _DeleteCommand.Parameters.Add(new SqlParameter("@Original_DeliveryAddress", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "DeliveryAddress", DataRowVersion.Original, false, null, "", "", ""));
                }
                return _DeleteCommand;
            }
        }
        public async Task<int> Delete(tbCaseRow drOriginal, CancellationToken ct)
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
        private static void SetCommandParameterValue(SqlCommand cmd, tbCaseRow? drOriginal, tbCaseRow? drCurrent)
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

