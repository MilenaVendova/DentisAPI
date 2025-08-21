using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
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
                tbUser tb = new(mc!);
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
        [HttpPut]
        public async Task<IActionResult> Insert(tbUserRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbUser tb = new(mc!);
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
        public class tbUserRowUpdate
        {
            public tbUserRow? Original { get; set; }
            public tbUserRow? Current { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Update(tbUserRowUpdate dr, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbUser tb = new(mc!);
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
        public async Task<IActionResult> Delete(tbUserRow drOriginal, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbUser tb = new(mc!);
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
