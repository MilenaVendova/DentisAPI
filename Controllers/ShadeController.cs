using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShadeController : Controller
    {
        //using Microsoft.AspNetCore.Authorization;
        //[Authorize]

        [HttpGet]
        public async Task<IActionResult> Fill(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbShade tb = new(mc!);
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
        public async Task<IActionResult> Insert(tbShadeRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbShade tb = new(mc!);
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
        public class tbShadeRowUpdate
        {
            public tbShadeRow? Original { get; set; }
            public tbShadeRow? Current { get; set; }
        }
        [HttpPut]
        public async Task<IActionResult> Update(tbShadeRowUpdate dr, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbShade tb = new(mc!);
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
        public async Task<IActionResult> Delete(tbShadeRow drOriginal, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbShade tb = new(mc!);
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
