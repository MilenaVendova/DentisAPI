using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Fill(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection("sa");
            try
            {
                tbDoctor tb = new(mc!);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> FillByID(int id, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection("sa");
            try
            {
                tbDoctor tb = new(mc!);
                await tb.FillByID(id, ct);
                if (tb.Count == 1)
                {
                    return Ok(tb[0]);
                }
                else
                {
                    return NotFound();
                }
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
        public async Task<IActionResult> Insert(tbDoctorRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection("sa");
            try
            {
                tbDoctor tb = new(mc!);
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
        public class tbDoctorRowUpdate
        {
            public tbDoctorRow? Original { get; set; }
            public tbDoctorRow? Current { get; set; }
        }
        [HttpPut]
        public async Task<IActionResult> Update(tbDoctorRowUpdate dr, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection("sa");
            try
            {
                tbDoctor tb = new(mc!);
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
        public async Task<IActionResult> Delete(tbDoctorRow drOriginal, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection("sa");
            try
            {
                tbDoctor tb = new(mc!);
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
