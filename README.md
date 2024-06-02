# Enterprise MVC App

This project is an enterprise-level .NET Core MVC application designed for content management with robust user authentication, role-based access control, version control, logging, and monitoring capabilities. It is deployed and hosted on AWS, leveraging modern tools and frameworks for optimal performance and scalability.

## Features

- **User Management**: User registration, login, and profile management with role-based access control.
- **Content Management**: CRUD operations for content with version control and a rich text editor for content creation.
- **Logging**: Integrated logging with ELK (Elasticsearch, Logstash, Kibana) stack.
- **Monitoring**: Application metrics and monitoring with Prometheus and Grafana.
- **Deployment**: CI/CD pipeline setup using GitHub Actions for seamless deployment to AWS EC2.

## Getting Started

### Prerequisites

- .NET Core 8.0
- MongoDB
- PostgreSQL
- Nginx (for reverse proxy setup)
- AWS Account (for deployment)
- GitHub Account (for CI/CD)

### Setup

1. **Clone the Repository**

   ```sh
   git clone https://github.com/khachatur/EnterpriseApp.git
   cd EnterpriseApp
   ```
2. **Configure Database Connection Strings**

Update the appsettings.json file with your PostgreSQL and MongoDB connection strings.

```json
{
  "ConnectionStrings": {
    "PostgreSqlConnection": "Host=your_postgresql_host;Database=your_database_name;Username=your_username;Password=your_password",
    "MongoDbSettings": {
      "ConnectionString": "your_mongodb_connection_string",
      "DatabaseName": "your_mongodb_database_name"
    }
  }
}
```

3. **Apply Database Migrations**

```sh
dotnet ef database update
```

4. **Run the Application**

```sh
dotnet run
```

5. **Open the Application**

Navigate to http://localhost:5099 in your web browser.

### Deployment

The application is configured for deployment on AWS EC2 with a CI/CD pipeline using GitHub Actions. Follow these steps to deploy your application:

1. **Set Up AWS Infrastructure**

   * Launch an EC2 instance with Ubuntu or your preferred Linux distribution.
   * Install required software (Nginx, .NET SDK) on the EC2 instance.
2. **Configure Nginx**

Update the Nginx configuration file to set up a reverse proxy for your application.

```nginx
server {
    listen 80;
    server_name your_domain_or_ip;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```sh
sudo systemctl restart nginx
```

3. **Set Up CI/CD with GitHub Actions**

    Create a GitHub Actions workflow file (.github/workflows/deploy.yml) in your repository.

```yaml
name: Deploy to AWS

on:
  push:
    branches:
      - main

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v2

    - name: Set up .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '5.0.x'

    - name: Build
      run: dotnet build --configuration Release

    - name: Publish
      run: dotnet publish --configuration Release --output ./publish

    - name: Deploy to AWS EC2
      uses: easingthemes/ssh-deploy@v2.1.5
      env:
        SSH_PRIVATE_KEY: ${{ secrets.SSH_PRIVATE_KEY }}
        ARGS: "-rltgoDzvO --delete"
        SOURCE: "publish/"
        REMOTE_HOST: ${{ secrets.REMOTE_HOST }}
        REMOTE_USER: ${{ secrets.REMOTE_USER }}
        TARGET: "/var/www/myapp"
  
    - name: Apply Database Migrations
      run: |
        ssh -i ${{ secrets.SSH_PRIVATE_KEY }} ${{ secrets.REMOTE_USER }}@${{ secrets.REMOTE_HOST }} "cd /var/www/myapp && dotnet ef database update"
```

Add the required secrets (SSH_PRIVATE_KEY, REMOTE_HOST, REMOTE_USER) to your GitHub repository settings.

### Monitoring and Logging

1. **Set Up Prometheus and Grafana**

   * Install Prometheus and Grafana on your monitoring server.
   * Configure Prometheus to scrape metrics from your application.
   * Set up Grafana dashboards to visualize application metrics.
2. **Configure ELK Stack**

   * Install Elasticsearch, Logstash, and Kibana.
   * Configure Logstash to collect and process logs from your application.
   * Visualize logs in Kibana.

### Contributing

Contributions are welcome! Please fork this repository and submit pull requests with your improvements.

### License

This project is licensed under the MIT License.

### Acknowledgments

Prometheus

Grafana

Elasticsearch

Logstash

Kibana
