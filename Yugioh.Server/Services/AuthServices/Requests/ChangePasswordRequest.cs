using System.ComponentModel.DataAnnotations;

namespace Yugioh.Server.Services.AuthServices.Requests
{
    public record ChangePasswordRequest([Required] string Email,
                                      [Required] string OldPassword,
                                      [Required] string NewPassword);
}
