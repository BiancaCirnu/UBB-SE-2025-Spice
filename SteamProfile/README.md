# SteamProfile Project Documentation

## Project Overview
This is a WinUI 3 desktop application that implements a Steam-like profile system. The project follows MVVM (Model-View-ViewModel) architecture pattern and uses SQL Server for data storage.

## Project Structure

### Core Directories
- `Views/` - Contains all XAML UI pages
- `ViewModels/` - Contains the corresponding ViewModels for each View
- `Models/` - Contains data models and DTOs
- `Services/` - Contains business logic and data access services
- `Data/` - Contains database-related files
  - `Procedures/` - Contains SQL stored procedures organized by feature
    - `Achievements/` - Achievement-related procedures
    - `Features/` - Feature-related procedures
    - `Collections/` - Collection-related procedures
    - `Users/` - User-related procedures

## Development Guidelines

### Database Procedures
1. All database operations should be implemented as stored procedures
2. Place new procedures in the appropriate subfolder under `Data/Procedures/`
3. Follow the naming convention: `[Feature]_[Action]` (e.g., `User_GetProfile`, `Achievement_Unlock`)
4. Each procedure should have a corresponding method in the appropriate service class

### Views and ViewModels
1. Each View (XAML page) should have a corresponding ViewModel
2. Views should only contain UI elements and bindings
3. All business logic should be in ViewModels
4. ViewModels should:
   - Implement `INotifyPropertyChanged`
   - Use dependency injection for services
   - Handle user interactions and data updates
   - Communicate with services for data operations

### Services
1. Services handle business logic and data access
2. Each major feature should have its own service class
3. Services should:
   - Be injected into ViewModels
   - Handle database operations through stored procedures
   - Implement proper error handling
   - Return appropriate data models

### Dependency Injection
The application uses dependency injection for services. To add a new service:

1. Create your service class
2. Register it in the service collection (typically in `App.xaml.cs`)
3. Inject it into your ViewModel constructor

Example:
```csharp
// Service registration in App.xaml.cs
services.AddSingleton<IUserService, UserService>();

// ViewModel constructor
public class ProfileViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    public ProfileViewModel(IUserService userService)
    {
        _userService = userService;
    }
}
```

### Adding New Features
1. Create necessary stored procedures in the appropriate `Procedures` subfolder
2. Create/update models in the `Models` folder
3. Create/update services in the `Services` folder
4. Create/update ViewModels in the `ViewModels` folder
5. Create/update Views in the `Views` folder

### Best Practices
1. Always use stored procedures for database operations
2. Keep Views simple and focused on UI
3. Implement proper error handling in services
4. Use async/await for database operations
5. Follow MVVM pattern strictly
6. Use proper naming conventions
7. Add appropriate comments and documentation

### Configuration
- The application uses `appsettings.json` for configuration
- Database connection strings and other settings should be stored here
- Make sure to update the configuration file with your local settings

## Getting Started
1. Clone the repository
2. Create the database and run the stored procedures
3. Update `appsettings.json` with your database connection string
4. Build and run the application

## Common Tasks
- Adding a new page: Create XAML in `Views/` and corresponding ViewModel
- Adding database operations: Create stored procedure and service method
- Adding new features: Follow the MVVM pattern and create necessary components

## Troubleshooting
- Check database connection in `appsettings.json`
- Ensure all stored procedures are created
- Verify service registration in `App.xaml.cs`
- Check ViewModel bindings in XAML 