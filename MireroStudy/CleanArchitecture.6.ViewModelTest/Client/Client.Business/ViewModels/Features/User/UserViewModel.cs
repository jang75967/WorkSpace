using Client.Business.Core.Application.Features.Users.Queries;
using Client.Business.Core.Domain.Models.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using System.Collections.ObjectModel;

namespace Client.Business.Extensions.ViewModels.Features.User;

public partial class UserViewModel : ObservableObject
{
    [ObservableProperty]
    public IEnumerable<UserModel> _users;

    private readonly IMediator _mediator;
    public UserViewModel(IMediator mediator)
    {
        _mediator = mediator;
        Users = new ObservableCollection<UserModel>();
    }

    [RelayCommand]
    public async Task LoadUsersAsync() => await _mediator.Send(new GetAllUsersQuery()).IfSomeAsync(r => Users = r);

}



//using Client.Business.Core.Application.Features.Users.Queries;
//using Client.Business.Core.Domain.Models.Users;
//using DevExpress.Mvvm;
//using DevExpress.Mvvm.DataAnnotations;
//using MediatR;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;

//public class UserViewModel : ViewModelBase
//{
//    private ObservableCollection<UserModel> _users;
//    public ObservableCollection<UserModel> Users
//    {
//        get => _users;
//        set => SetProperty(ref _users, value, nameof(Users));
//    }

//    private readonly IMediator _mediator;
//    public UserViewModel(IMediator mediator)
//    {
//        _mediator = mediator;
//        Users = new ObservableCollection<UserModel>();
//    }

//    [Command]
//    public async Task LoadUsersAsync()
//    {
//        var users = await _mediator.Send(new GetAllUsersQuery());
//        Users = new ObservableCollection<UserModel>((List<UserModel>)users);
//    }
//}
