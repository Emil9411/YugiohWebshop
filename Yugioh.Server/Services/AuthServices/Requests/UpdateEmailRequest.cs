using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Services.AuthServices.Requests
{
    public record UpdateEmailRequest([Required] string Email,
                                     [Required] string NewEmail);
}
