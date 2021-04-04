using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence
{
   public interface IUnitOfWork
   {
       public IUserRefreshTokenRepository UserRefreshTokenRepository { get; }
       Task CommitAsync();
       ValueTask RollBackAsync();
   }
}
