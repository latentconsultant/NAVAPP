### Hosting on Windows IIS

If you prefer to host the application on Windows using Internet Information Services (IIS) instead of Docker:

1. **Install Prerequisites**:
   - Ensure the [.NET 10 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/10.0) is installed on the server.
   - Enable IIS on your Windows Server.

2. **Publish the Application**:
   - Run the `./publish.ps1` script or execute:
     ```powershell
     dotnet publish -c Release -o ./publish
     ```

3. **Configure IIS**:
   - Create a new "Website" or "Application" in IIS Manager.
   - Point the Physical Path to the `./publish` folder.
   - Set the Application Pool to "No Managed Code".

4. **Permissions**:
   - Ensure the `IIS AppPool\<YourAppPoolName>` has read/write permissions to the `data/` directory (for the SQLite database) and the `wwwroot/uploads/` directory.

5. **Access**:
   - The application will be accessible via the hostname/IP configured in IIS.
