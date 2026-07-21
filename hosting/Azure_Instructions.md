### Hosting on Microsoft Azure

Azure is the native home for .NET applications. Since your app is containerized, you have several ways to host it.

#### Option 1: Azure App Service (Web App for Containers) - Recommended
This is the easiest way to host a single container.

1.  **Push your code to GitHub**: The project includes a `.github/workflows/docker-publish.yml` file. Once you push your code to the `main` branch, it will automatically build a Docker image and publish it to the GitHub Container Registry (GHCR).
2.  **Azure Portal**: Go to **Create a resource** > **Web App**.
3.  **Basics**:
    - **Publish**: Select "Docker Container".
    - **Operating System**: Linux.
4.  **Docker Tab**:
    - **Options**: "Single Container".
    - **Image Source**: "GitHub Actions" (or "Azure Container Registry" if you prefer).
5.  **Networking**: Ensure port `8080` is specified if requested, though Azure usually detects the `EXPOSE` instruction in the Dockerfile.
6.  **Configuration**: In the Web App settings, add an **Application Setting**:
    - `WEBSITES_PORT` = `8080`
7.  **Access**: Azure will provide a URL like `https://your-app-name.azurewebsites.net`.

#### Option 2: Azure Container Apps (Modern & Serverless)
Ideal for scaling and cost-efficiency.

1.  **Azure Portal**: Search for **Container Apps** and click **Create**.
2.  **Container**: Point it to your Docker image (from Azure Container Registry or Docker Hub).
3.  **Ingress**: 
    - Enable Ingress.
    - Set **Target Port** to `8080`.
    - Set **Accept traffic from**: "Everywhere".

#### Database Strategy on Azure
*   **Azure SQL Database**: A fully managed SQL Server. 
    1. Create an Azure SQL Database.
    2. Get the connection string.
    3. Add it to your App Service "Connection Strings" as `DefaultConnection`.
    4. The app will automatically detect it and use SQL Server instead of SQLite.

#### CI/CD: Automated Deployment to Azure
The provided GitHub Action can be easily extended to deploy to Azure.
1. Download the **Publish Profile** from your Azure Web App.
2. Add it as a secret named `AZURE_WEBAPP_PUBLISH_PROFILE` in GitHub.
3. Use the `azure/webapps-deploy` action in your workflow.
