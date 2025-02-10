# Danger Alert WebApp

## Overview
Danger Alert WebApp is a real-time emergency alert system built using the Angular framework and integrated with an ASP.NET Core Web API. It helps users send alert signals in emergency situations such as fire emergencies, medical emergencies, accidents, or criminal activities. The application tracks user locations and displays the nearest available helpers using Leaflet maps. It also pushes real-time notifications to nearby service providers and allows users to add custom alerts and emergency contacts.

## Features
- **User Authentication & Security**
  - Secure user registration and login.
  - Password hashing and session management.
  - Role-based access control.
  
- **Emergency Alert System**
  - Send instant alerts in various danger situations.
  - Track user location and notify the nearest available helper.
  - Display nearest helpers using Leaflet map integration.
  
- **Real-Time Notifications**
  - Push notifications to the nearest service provider.
  - Instant updates on alert status.
  
- **Custom Alerts & Emergency Contacts**
  - Users can create custom alert categories.
  - Add custom emergency contacts for quick assistance.
  
- **Database Integration**
  - PostgreSQL database for secure data storage.
  - Tracks user alerts, locations, and responses.
  
- **Security Measures**
  - CSRF protection and encrypted communication.
  - Authentication and authorization using JWT tokens.
  
## Technologies Used
- **Frontend:** Angular
- **Backend:** ASP.NET Core Web API
- **Database:** PostgreSQL
- **Map Integration:** Leaflet.js
- **Authentication:** JWT (JSON Web Token)
- **Real-Time Communication:** SignalR

## Installation & Setup
### Frontend (Angular)
1. **Clone the Angular repository**
   ```sh
   git clone https://github.com/ragnar-harsh/Danger-Alert-Web-App.git
   cd Danger-Alert-Web-App
   ```
2. **Install dependencies**
   ```sh
   npm install
   ```
3. **Run the application**
   ```sh
   ng serve
   ```
   Access the application at `http://localhost:4200/`.

### Backend (ASP.NET Core Web API)
1. **Clone the API repository**
   ```sh
   git clone https://github.com/ragnar-harsh/Danger-Alert-Web-API.git
   cd Danger-Alert-Web-API
   ```
2. **Restore dependencies**
   ```sh
   dotnet restore
   ```
3. **Update database** (Ensure PostgreSQL is configured)
   ```sh
   dotnet ef database update
   ```
4. **Run the API server**
   ```sh
   dotnet run
   ```
   API available at `http://localhost:5000/`.

## Usage
- Register or log in to access the platform.
- Choose the type of emergency alert and send signals.
- The system tracks location and notifies nearby helpers and service providers.
- Users can add emergency contacts and custom alert categories.
- Real-time updates on alerts through push notifications.

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests to improve the project.

## License
This project is licensed under the MIT License.

---
Made with ❤️ using Angular & ASP.NET Core.

