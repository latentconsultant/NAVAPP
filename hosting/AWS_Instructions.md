### Migrating to Amazon Web Services (AWS)

This guide provides step-by-step instructions for hosting your .NET application on AWS. Since the application is already containerized, you have several excellent options.

#### Option 1: AWS App Runner (Recommended - Easiest)
AWS App Runner is the simplest way to deploy a containerized web application. It handles the server management, scaling, and load balancing automatically.

1.  **Push your code to GitHub**: Ensure your latest changes are in your GitHub repository.
2.  **AWS Console**: Search for "App Runner" and click **Create App Runner service**.
3.  **Source**:
    - Choose **Repository type**: "Container registry".
    - Choose **Provider**: "GitHub Container Registry" (or use ECR if you prefer).
    - If using GitHub Container Registry, provide the image URI: `ghcr.io/<your-github-username>/navigateapp:latest`.
    - Choose **Deployment settings**: "Automatic" (so it redeploys when the image is updated by GitHub Actions).
4.  **Configuration**:
    - AWS will detect the container settings.
5.  **Service settings**:
    - **Port**: Set to `8080`.
    - **Environment variables**: Add `ASPNETCORE_ENVIRONMENT` = `Production`.
6.  **Create & Deploy**: AWS will pull your image from GHCR and provide a public URL.

#### Option 2: Amazon ECS with Fargate (Professional)
For more control and standard enterprise orchestration.

1.  **Amazon ECR**: Create a repository to store your Docker images.
2.  **Push Image**: Use the `docker-publish.yml` workflow (updated for AWS) to push your image to ECR.
3.  **ECS Cluster**: Create a Fargate cluster.
4.  **Task Definition**: Create a new Task Definition using your ECR image URI.
    - Memory: 0.5 GB, CPU: 0.25 vCPU (minimal for start).
    - Container Port: 8080.
5.  **Service**: Create a service from the Task Definition.
    - Enable "Public IP" if not using a Load Balancer.

#### Database Strategy on AWS (Important)
The application currently uses SQLite (`data/logistics.db`). On AWS services like App Runner or ECS Fargate, local storage is **ephemeral** (wiped on every restart/deployment).

*   **For Development/Testing**: SQLite is fine, but data will reset.
*   **For Production**: 
    1.  **Amazon RDS**: Create a SQL Server instance (Express edition is often free tier eligible).
    2.  **Connection String**: Update your AWS environment variables to include:
        `ConnectionStrings__DefaultConnection` = `Server=your-rds-endpoint;Database=LogisticsDb;User Id=admin;Password=your-password;TrustServerCertificate=True;`
    3.  The application will automatically detect the non-SQLite connection string and switch to SQL Server mode.

#### CI/CD: Automated Deployment to AWS ECR
I have updated `.github/workflows/docker-publish.yml` to include the necessary steps for Amazon ECR. 

1.  **IAM User**: Create an AWS IAM user with `AmazonEC2ContainerRegistryFullAccess`.
2.  **GitHub Secrets**: Add the following to your GitHub repo settings:
    - `AWS_ACCESS_KEY_ID`
    - `AWS_SECRET_ACCESS_KEY`
    - `AWS_REGION` (e.g., `us-east-1`)
    - `ECR_REPOSITORY` (name of your ECR repo)
3.  Uncomment the AWS section in `.github/workflows/docker-publish.yml`.
