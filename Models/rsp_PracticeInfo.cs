using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using DentisAPI.Services;
using Microsoft.Data.SqlClient;
namespace DentisAPI.Models
{
    public class rsp_PracticeInfoRow
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
        public int? DoctorCount { get; set; }
        public int? CaseCount { get; set; }
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
            this.DoctorCount = (dReader["DoctorCount"] != DBNull.Value) ? (int)dReader["DoctorCount"] : null;
            this.CaseCount = (dReader["CaseCount"] != DBNull.Value) ? (int)dReader["CaseCount"] : null;
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
                "DoctorCount" => (this.DoctorCount.HasValue) ? this.DoctorCount : DBNull.Value,
                "CaseCount" => (this.CaseCount.HasValue) ? this.CaseCount : DBNull.Value,
                _ => DBNull.Value,
            };
        }
    }
    public class rsp_PracticeInfo : List<rsp_PracticeInfoRow>
    {
        private readonly MyConnection _Connection;
        public rsp_PracticeInfo(MyConnection mc) : base()
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
                    _SelectCommand.CommandText = "dbo.rsp_PracticeInfo";
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
                    rsp_PracticeInfoRow dr = new rsp_PracticeInfoRow();
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
        private static void SetCommandParameterValue(SqlCommand cmd, rsp_PracticeInfoRow? drOriginal, rsp_PracticeInfoRow? drCurrent)
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
