using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameTracker.Shared.DataTransferObjects
{
    public record UserDto(int Id, string? Name, string Email, DateTime CreatedAt, DateTime? UpdatedAt);

}
