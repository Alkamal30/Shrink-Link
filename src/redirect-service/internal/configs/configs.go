package configs

type AppConfig struct {
	Url   string
	Grpc  GrpcConfig
	Redis RedisConfig
}

type GrpcConfig struct {
	Url             string
	UseTls          bool
	CertificatePath string
}

type RedisConfig struct {
	Address  string
	Password string
}
