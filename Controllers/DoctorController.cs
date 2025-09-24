using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : Controller
    {
        //using Microsoft.AspNetCore.Authorization;
        //[Route("api/[controller]")]
        //[ApiController]
        //[Authorize]

        [HttpGet]
        public async Task<IActionResult> Fill(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
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
        [HttpGet]
        [Route("DoctorInfo")]
        public async Task<IActionResult> FillInfo(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
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
        [HttpPost]
        public async Task<IActionResult> Insert(tbDoctorRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
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
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
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
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
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
