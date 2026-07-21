FROM registry.jetbrains.team/p/junie-cli/containers/demo-base:2

# Install .NET 10 SDK
USER root
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    ca-certificates \
    && curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 10.0 --install-dir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

# Switch back to the default user if known, otherwise root is fine for demo setup
# Base image usually expects a non-root user for running the app, but let's see.
