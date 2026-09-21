namespace Mantenimiento.Core.Application.Interfaces.GenericServiceInterfaces
{
    public interface IGenericService<SaveViewModel, ViewModel>
                         where SaveViewModel : class
                         where ViewModel : class
    {
        Task Update(SaveViewModel vm);
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Delete(int id);
        Task<SaveViewModel> GetByIdSaveViewModel(int id);
        Task<List<ViewModel>> GetAllViewModel();
        Task<ViewModel> GetByIdViewModel(int id);
    }
}
