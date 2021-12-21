using TinCore.Domain.Interfaces;
using TinCore.Infra.Data.Context;

namespace TinCore.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TinCoreContext _context;

        public UnitOfWork(TinCoreContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
