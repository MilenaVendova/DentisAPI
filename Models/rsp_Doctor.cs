using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using DentisAPI.Services;
using Microsoft.Data.SqlClient;
namespace DentisAPI.Models
{
    public class rsp_DoctorRow
    {
        public int CaseID { get; set; }
        public DateTime CaseDate { get; set; }
        public DateTime? Deadline { get; set; }
        public string? DeliveryAddress { get; set; }
        public int DoctorID { get; set; }
        public int ItemTypeID { get; set; }
        public int? MaterialID { get; set; }
        public string? Notes { get; set; }
        public int PatientID { get; set; }
        public int? PaymentStatusID { get; set; }
        public int PracticeID { get; set; }
        public decimal? Price { get; set; }
        public int? PriorityLevelID { get; set; }
        public int? ShadeID { get; set; }
        public int? StatusID { get; set; }
        public int? TechnicianID { get; set; }
        public string? ToothNumber { get; set; }
        public int? CaseCount { get; set; }
        public void SetDataFromSQL(SqlDataReader dReader)
        {
            this.CaseID = (int)dReader["CaseID"];
            this.CaseDate = (DateTime)dReader["CaseDate"];
            this.Deadline = (dReader["Deadline"] != DBNull.Value) ? (DateTime)dReader["Deadline"] : null;
            this.DeliveryAddress = (dReader["DeliveryAddress"] != DBNull.Value) ? (string)dReader["DeliveryAddress"] : null;
            this.DoctorID = (int)dReader["DoctorID"];
            this.ItemTypeID = (int)dReader["ItemTypeID"];
            this.MaterialID = (dReader["MaterialID"] != DBNull.Value) ? (int)dReader["MaterialID"] : null;
            this.Notes = (dReader["Notes"] != DBNull.Value) ? (string)dReader["Notes"] : null;
            this.PatientID = (int)dReader["PatientID"];
            this.PaymentStatusID = (dReader["PaymentStatusID"] != DBNull.Value) ? (int)dReader["PaymentStatusID"] : null;
            this.PracticeID = (int)dReader["PracticeID"];
            this.Price = (dReader["Price"] != DBNull.Value) ? (decimal)dReader["Price"] : null;
            this.PriorityLevelID = (dReader["PriorityLevelID"] != DBNull.Value) ? (int)dReader["PriorityLevelID"] : null;
            this.ShadeID = (dReader["ShadeID"] != DBNull.Value) ? (int)dReader["ShadeID"] : null;
            this.StatusID = (dReader["StatusID"] != DBNull.Value) ? (int)dReader["StatusID"] : null;
            this.TechnicianID = (dReader["TechnicianID"] != DBNull.Value) ? (int)dReader["TechnicianID"] : null;
            this.ToothNumber = (string)dReader["ToothNumber"];
            this.CaseCount = (dReader["CaseCount"] != DBNull.Value) ? (int)dReader["CaseCount"] : null;
        }
        public object GetData(string Name)
        {
            return Name switch
            {
                "CaseID" => this.CaseID,
                "CaseDate" => this.CaseDate,
                "Deadline" => (this.Deadline.HasValue) ? this.Deadline : DBNull.Value,
                "DeliveryAddress" => (this.DeliveryAddress != null) ? this.DeliveryAddress : DBNull.Value,
                "DoctorID" => this.DoctorID,
                "ItemTypeID" => this.ItemTypeID,
                "MaterialID" => (this.MaterialID.HasValue) ? this.MaterialID : DBNull.Value,
                "Notes" => (this.Notes != null) ? this.Notes : DBNull.Value,
                "PatientID" => this.PatientID,
                "PaymentStatusID" => (this.PaymentStatusID.HasValue) ? this.PaymentStatusID : DBNull.Value,
                "PracticeID" => this.PracticeID,
                "Price" => (this.Price.HasValue) ? this.Price : DBNull.Value,
                "PriorityLevelID" => (this.PriorityLevelID.HasValue) ? this.PriorityLevelID : DBNull.Value,
                "ShadeID" => (this.ShadeID.HasValue) ? this.ShadeID : DBNull.Value,
                "StatusID" => (this.StatusID.HasValue) ? this.StatusID : DBNull.Value,
                "TechnicianID" => (this.TechnicianID.HasValue) ? this.TechnicianID : DBNull.Value,
                "ToothNumber" => (this.ToothNumber != null) ? this.ToothNumber : DBNull.Value,
                "CaseCount" => (this.CaseCount.HasValue) ? this.CaseCount : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class rsp_Doctor : List<rsp_DoctorRow>
    {
        private readonly MyConnection _Connection;
        public rsp_Doctor(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "dbo.rsp_Doctor";
                    _SelectCommand.CommandType = CommandType.StoredProcedure;
                    _SelectCommand.Parameters.Add(new SqlParameter("@RETURN_VALUE", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 10, 0, "", DataRowVersion.Current, false, null, "", "", ""));
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
                    rsp_DoctorRow dr = new rsp_DoctorRow();
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
        private static void SetCommandParameterValue(SqlCommand cmd, rsp_DoctorRow? drOriginal, rsp_DoctorRow? drCurrent)
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
