using System.ComponentModel;                                                                //                                                                          //
using Deliverr.Models;                                                                      //                                                                          //
                                                                                            //                                                                          //
namespace Deliverr.ViewModels;                                                              //  Namespace verklaring                                                    //
                                                                                            //                                                                          //
public class AccountViewModel : INotifyPropertyChanged                                      //  AccountViewModel                                                        //
{                                                                                           //                                                                          //
    private string user_name;                                                               //  De naam van de gebruiker.                                               //
    private string user_details;                                                            //  De details van de gebruiker.                                            //
    private string user_email;                                                              //  De email van de gebruiker.                                              //
    private Image user_profile_picture;                                                     //  De profielfoto van gebruiker.                                           //
                                                                                            //                                                                          //
    public string Name                                                                      //  Name property die de naam van de gebruiker bevat.                       //
    {                                                                                       //                                                                          //
        get => user_name;                                                                   //  Getter voor de naam van de gebruiker.                                   //
        set                                                                                 //  Setter voor de naam van de gebruiker.                                   //
        {                                                                                   //                                                                          //
            if (user_name != value)                                                         //  Als de huidige naam niet gelijk is aan de nieuwe waarde, dan:           //
            {                                                                               //                                                                          //
                user_name = value;                                                          //  Naam wordt verandert naar de nieuwe waarde.                             //
                OnPropertyChanged(nameof(Name));                                            //  OnPropertyChanged wordt aangeroepen om de verandering te melden.        //
            }                                                                               //                                                                          //
        }                                                                                   //                                                                          //
    }                                                                                       //                                                                          //
    public string Details                                                                   //  Details property die de details van de gebruiker bevat.                 //
    {                                                                                       //                                                                          //
        get => user_details;                                                                //  Getter voor de details van de gebruiker.                                //
        set                                                                                 //  Setter voor de details van de gebruiker.                                //
        {                                                                                   //                                                                          //
            if (user_details != value)                                                      //  Als de huidige details niet gelijk is aan de nieuwe waarde, dan:        //
            {                                                                               //                                                                          //
                user_details = value;                                                       //  Details wordt verandert naar de nieuwe waarde.                          //
                OnPropertyChanged(nameof(Details));                                         //  OnPropertyChanged wordt aangeroepen om de verandering te melden.        //
            }                                                                               //                                                                          //
        }                                                                                   //                                                                          //
    }                                                                                       //                                                                          //
    public string Email                                                                     //  Email property die de naam van de gebruiker bevat.                      //
    {                                                                                       //                                                                          //
        get => user_email;                                                                  //  Getter voor de email van de gebruiker.                                  //
        set                                                                                 //  Setter voor de email van de gebruiker.                                  //
        {                                                                                   //                                                                          //
            if (user_email != value)                                                        //  Als de huidige email niet gelijk is aan de nieuwe waarde, dan:          //
            {                                                                               //                                                                          //
                user_email = value;                                                         //  Email wordt verandert naar de nieuwe waarde.                            //
                OnPropertyChanged(nameof(Email));                                           //  OnPropertyChanged wordt aangeroepen om de verandering te melden.        //
            }                                                                               //                                                                          //
        }                                                                                   //                                                                          //
    }                                                                                       //                                                                          //
    public Image ProfilePicture                                                             //  Profielfoto property die de profielfoto van de gebruiker bevat.         //
    {                                                                                       //                                                                          //
        get => user_profile_picture;                                                        //  Getter voor de profielfoto van de gebruiker.                            //
        set                                                                                 //  Setter voor de profielfoto van de gebruiker.                            //
        {                                                                                   //                                                                          //
            if (user_profile_picture != value)                                              //  Als de huidige profielfoto niet gelijk is aan de nieuwe waarde, dan:    //
            {                                                                               //                                                                          //
                user_profile_picture = value;                                               //  Profielfoto wordt verandert naar de nieuwe waarde.                      //
                OnPropertyChanged(nameof(ProfilePicture));                                  //  OnPropertyChanged wordt aangeroepen om de verandering te melden.        //
            }                                                                               //                                                                          //
        }                                                                                   //                                                                          //
    }                                                                                       //                                                                          //
                                                                                            //                                                                          //
    public event PropertyChangedEventHandler PropertyChanged;
                                                                                            //                                                                          //
    protected void OnPropertyChanged(string propertyName) =>                                //  OnPropertyChanged functie die iets doet veranderen als iets verandert   //
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));          //  Hier kijkt hij of het echt is verandert m.b.v. PropertyChanged.         //
                                                                                            //                                                                          //
    public AccountViewModel(Account account)                                                //  Constructor voor AccountViewModel.                                      //
    {                                                                                       //                                                                          //
        Name = account.Username;                                                            //  De naam van de gebruiker.                                               //
        Details = account.Details;                                                          //  Details van de gebruiker.                                               //
        Email = account.Email;                                                              //  Email van de gebruiker.                                                 //
        ProfilePicture = account.ProfilePicture;                                            //  Profielfoto van de gebruiker.                                           //
    }                                                                                       //                                                                          //
}                                                                                           //                                                                          //
