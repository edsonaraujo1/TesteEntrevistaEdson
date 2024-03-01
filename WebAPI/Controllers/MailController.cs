using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Infra.Services;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromBody] SendMail sendMail)
        {
            try
            {
                if (!string.IsNullOrEmpty(sendMail.Emails.ToString()))
                {
                    sendMail.Emails[0] = "edsonaraujo1@outlook.com";
                    sendMail.Subject = "Teste de Envio de Email";
                    sendMail.Body = "Teste de Envio de Email esse e o Corpo <br> <b>SMTP</b>";
                    sendMail.IsHtml = true;
                }
                _mailService.SendMail(sendMail.Emails, sendMail.Subject, sendMail.Body, sendMail.IsHtml);
            }
            catch (System.Exception e)
            {

                //throw e;
                return Ok("Erro: " + e.Message);
            }
            
            
            return Ok("E-Mail Enviado com Sucesso!");
        }
    }
}
