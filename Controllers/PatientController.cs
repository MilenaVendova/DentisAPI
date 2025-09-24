using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        //using Microsoft.AspNetCore.Authorization;
        //[Authorize]

        [HttpGet]
        public async Task<IActionResult> Fill(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbPatient tb = new(mc!);
                await tb.Fill(ct);
                return Ok(tb);
            }
            catch (System.Exception ex)
            {
                return BadRequest(Json(ex.Message));
            }
            finally
            {
                mc?.Release();
            }
        }
        public class PatientCase : tbPatientRow
        {
            private List<tbCaseRow> cases;
            public PatientCase(tbPatientRow patient, List<tbCaseRow> pcases)
            {
                this.PatientID = patient.PatientID;
                this.Name = patient.Name;
                this.BirthDate = patient.BirthDate;
                this.HealthInsuranceNumber = patient.HealthInsuranceNumber;
                this.ShadeID = patient.ShadeID;
                this.Phone = patient.Phone;
                this.Email = patient.Email;
                this.cases = pcases;
            }
            public List<tbCaseRow> Cases
            {
                get { return cases; }
                set { cases = value; }
            }
        }
        [HttpGet]
        [Route("WithCases")]
        public async Task<IActionResult> Fill1(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbPatient tbP = new(mc!);
                await tbP.Fill(ct);

                tbCase tbC = new(mc!);
                await tbC.Fill(ct);

                List<PatientCase> pc = new();

                foreach (tbPatientRow p in tbP)
                {
                    pc.Add(new PatientCase(p, tbC.Where(c => c.PatientID == p.PatientID).ToList()));
                }
                return Ok(pc);
            }
            catch (System.Exception ex)
            {
                return BadRequest(Json(ex.Message));
            }
            finally
            {
                mc?.Release();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(tbPatientRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbPatient tb = new(mc!);
                return Ok(await tb.Insert(drCurrent, ct));
            }
            catch (System.Exception ex)
            {
                return BadRequest(Json(ex.Message));
            }
            finally
            {
                mc?.Release();
            }
        }
        public class tbPatientRowUpdate
        {
            public tbPatientRow? Original { get; set; }
            public tbPatientRow? Current { get; set; }
        }
        [HttpPut]
        public async Task<IActionResult> Update(tbPatientRowUpdate dr, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbPatient tb = new(mc!);
                return Ok(await tb.Update(dr.Original!, dr.Current!, ct));
            }
            catch (System.Exception ex)
            {
                return BadRequest(Json(ex.Message));
            }
            finally
            {
                mc?.Release();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(tbPatientRow drOriginal, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbPatient tb = new(mc!);
                await tb.Delete(drOriginal, ct);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(Json(ex.Message));
            }
            finally
            {
                mc?.Release();
            }
        }

    }
}
