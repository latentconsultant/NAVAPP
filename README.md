### Hosting the Application

This application is containerized using Docker, making it easy to host on any computer or server.

#### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) and [Docker Compose](https://docs.docker.com/compose/install/) installed on the host machine.

#### Quick Start

1. **Clone the repository** (if you haven't already).
2. **Navigate to the project root** in your terminal.
3. **Run the application** using Docker Compose:
   ```bash
   docker-compose up -d
   ```
4. **Access the application**:
   - Locally: Open `http://localhost:8080`
   - API Documentation (Swagger): `http://localhost:8080/swagger`
   - Health Status: `http://localhost:8080/health`

#### Accessing from Other Computers

To allow other users to access the application from their computers on the same network:

1. **Use the Sharing Script**:
   - Run the `./share.ps1` script in PowerShell. It will automatically find your IP address and provide the exact link to share.
2. **Manual Discovery**:
   - **Windows**: Run `ipconfig` in CMD/PowerShell and look for `IPv4 Address`.
   - **Linux/Mac**: Run `ifconfig` or `ip addr`.
3. **Open Firewall Ports**:
   - Ensure port `8080` (or your configured port) is open in your computer's firewall settings to allow incoming traffic.
4. **Access URL**:
   - Other users can then access the app via `http://<YOUR_HOST_IP>:8080`.

#### Development Tunneling (Remote Access without IP)

If you are using **JetBrains Rider** or **Visual Studio**, you can use **Dev Tunnels**:
1. Enable "Dev Tunnels" in your run configuration.
2. This will provide a public URL that anyone can use to connect to your local host without complex network configuration.

#### CI/CD and Automated Hosting

A GitHub Actions workflow is provided in `.github/workflows/docker-publish.yml`. It automatically:
- Builds the .NET application.
- Creates a Docker image on every push to the `main` branch.
- Facilitates deployment to hosts like **Azure App Service**, **AWS ECS**, or any Docker-compatible hosting provider.

For specific platform instructions, see:
- [AWS Migration Guide](hosting/AWS_Instructions.md)
- [Azure Hosting Guide](hosting/Azure_Instructions.md)
- [Windows IIS Hosting](hosting/IIS_Instructions.md)

#### Persistence

The application uses a SQLite database for storage. By default, Docker Compose creates a persistent volume called `navigateapp-data` which maps to `/app/data` inside the container. This ensures that your data (including the `logistics.db` file) is preserved even if the container is stopped or removed.

#### Configuration

- **Port**: The application listens on port 8080 by default. You can change this in the `docker-compose.yml` file by modifying the `ports` section.
- **Environment**: The application is configured to run in `Production` mode.
- **Reverse Proxy**: If you are hosting this behind a reverse proxy (like Nginx, Traefik, or Apache), the application is pre-configured to handle `X-Forwarded-For` and `X-Forwarded-Proto` headers.

#### Manual Build (without Docker Compose)

If you prefer to build and run the image manually:

```bash
docker build -t navigateapp .
docker run -d -p 8080:8080 -v navigateapp_data:/app/data navigateapp
```
