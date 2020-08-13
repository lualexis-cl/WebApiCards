using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Solution.WebApiCards.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Com.Solution.WebApiCards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly PaymentDetailContext _paymentDetailContext;
        public PaymentDetailController(PaymentDetailContext paymentDetailContext)
        {
            _paymentDetailContext = paymentDetailContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentDetails()
        {
            return await _paymentDetailContext.PaymentDetail.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
            var paymentDetail = await _paymentDetailContext.PaymentDetail.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            return Ok(paymentDetail);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {
            if (id != paymentDetail.PMId)
            {
                return BadRequest();
            }

            _paymentDetailContext.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _paymentDetailContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!PaymentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
            _paymentDetailContext.PaymentDetail.Add(paymentDetail);
            await _paymentDetailContext.SaveChangesAsync();

            return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PMId }, paymentDetail);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentDetail>> DeletePaymentDetail(int id)
        {
            var paymentDetail = await _paymentDetailContext.PaymentDetail.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            _paymentDetailContext.PaymentDetail.Remove(paymentDetail);
            await _paymentDetailContext.SaveChangesAsync();

            return Ok(paymentDetail);
        }

        private bool PaymentDetailExists(int id)
        {
            return 
                _paymentDetailContext.PaymentDetail.Any(a => a.PMId == id);
        }
    }
}
