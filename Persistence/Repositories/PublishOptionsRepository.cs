using Application.Common.Interfaces;
using Domain.Entities;

namespace Persistence.Repositories;

public class PublishOptionsRepository : BaseRepository<PublishOptions>, IPublishOptionsRepository
{
    public PublishOptionsRepository(ContextDb context) : base(context)
    {
    }
}
