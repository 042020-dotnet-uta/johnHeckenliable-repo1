using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamProject_p1.Data.EFCore
{
  // Implementaion of EFCore inspired by tutorial:
  // https://medium.com/net-core/repository-pattern-implementation-in-asp-net-core-21e01c6664d7
  public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
  {

  }
}