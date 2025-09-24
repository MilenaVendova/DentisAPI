using DentisAPI.Models;
using DentisAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Material_ItemTypeController : Controller
    {
        //using Microsoft.AspNetCore.Authorization;
        
        //[Authorize]

        [HttpGet]
        public async Task<IActionResult> Fill(CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbMaterial_ItemType tb = new(mc!);
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
        public async Task<IActionResult> Insert(tbMaterial_ItemTypeRow drCurrent, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbMaterial_ItemType tb = new(mc!);
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
        public class tbMaterial_ItemTypeRowUpdate
        {
            public tbMaterial_ItemTypeRow? Original { get; set; }
            public tbMaterial_ItemTypeRow? Current { get; set; }
        }
        [HttpPut]
        public async Task<IActionResult> Update(tbMaterial_ItemTypeRowUpdate dr, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbMaterial_ItemType tb = new(mc!);
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
        public async Task<IActionResult> Delete(tbMaterial_ItemTypeRow drOriginal, CancellationToken ct)
        {
            MyConnection? mc = ConnectionManager.GetConnection(User!.Identity!.Name!);
            try
            {
                tbMaterial_ItemType tb = new(mc!);
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
