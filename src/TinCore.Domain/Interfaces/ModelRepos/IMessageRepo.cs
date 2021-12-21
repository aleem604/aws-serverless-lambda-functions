using System.Collections.Generic;
using TinCore.Domain.Models;

namespace TinCore.Domain.Interfaces
{
    public interface IMessageRepo : IRepositoryT<Message>
    {
        //IEnumerable<EntityObject> GetGallaryImages(int Id);
    }
}