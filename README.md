# Shrink-Link

## Getting Started

### Containers

1. Clone repository
```shell
git clone https://github.com/Alkamal30/Shrink-Link.git
```

2. Run next command:
```shell
docker compose up -d
```

3. Open `https://localhost:8080/swagger` in browser

### Localhost

1. Clone repository
```shell
git clone https://github.com/Alkamal30/Shrink-Link.git
```

2. Run Postgres Docker container
```shell
docker run --name some-postgres -e POSTGRES_PASSWORD=postgres -d -p "5432:5432" postgres
```

3. Make development certificates trusted
```shell
dotnet dev-certs https --trust
```

4. Run projects