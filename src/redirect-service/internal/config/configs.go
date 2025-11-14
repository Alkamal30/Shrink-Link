package config

type GrpcConfig struct {
	Url             string
	UseTls          bool
	CertificatePath string
}

type AppConfig struct {
	Grpc GrpcConfig
}
