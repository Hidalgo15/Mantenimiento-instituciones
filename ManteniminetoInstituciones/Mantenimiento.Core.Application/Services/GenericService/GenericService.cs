using Mantenimiento.Core.Application.Interfaces.GenericServiceInterfaces;
using Mantenimiento.Core.Domain.RepositoryInterfaces.GenericRepositoryInterfaces;

namespace Mantenimiento.Core.Application.Services.GenericService
{
    public class GenericService<TEntity, TSaveViewModel, TViewModel> : IGenericService<TSaveViewModel, TViewModel>
         where TEntity : class
         where TSaveViewModel : class
         where TViewModel : class
    {
        private readonly IGenericRepository<TEntity> _repository; // Inyección de dependencia del repositorio genérico

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;

        }

        public Task<TSaveViewModel> Add(TSaveViewModel vm)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TViewModel>> GetAllViewModel()
        {
            throw new NotImplementedException();
        }

        public Task<TSaveViewModel> GetByIdSaveViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TViewModel> GetByIdViewModel(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(TSaveViewModel vm)
        {
            throw new NotImplementedException();
        }
    }
}
