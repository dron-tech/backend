using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ConfirmCodeRepository : BaseRepository<ConfirmCode>, IConfirmCodeRepository
{
    public ConfirmCodeRepository(ContextDb context) : base(context)
    {
    }

}
