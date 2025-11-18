package configs

type AppConfig struct {
	Url  string
	Grpc GrpcConfig
}

type GrpcConfig struct {
	Url             string
	UseTls          bool
	CertificatePath string
}
